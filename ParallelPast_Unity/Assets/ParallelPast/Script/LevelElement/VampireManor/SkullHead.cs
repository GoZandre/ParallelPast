using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkullHead : MonoBehaviour
{

    [Header("Parameters")]

    [SerializeField]
    private float _maxRotSpeed;

    [SerializeField]
    private float _maxRotByVelocity;

    [SerializeField]
    private float _plantAxeRotation;

    [SerializeField]
    private float _gravityScale;
    [SerializeField]
    private float _launchStrength;

    [SerializeField]
    private float _arrowsSensibility;
    [SerializeField]
    private AnimationCurve _arrowYValue;

    

    [SerializeField]
    private Vector2 _defaultOrientation = new Vector2(1, 1);

    [Header("References")]
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private Transform _axeSprite;


    private PlayerControls _playerControls = null;
    private LineRenderer _lineRenderer;


    //Vars
    [SerializeField]
    private List<PlayerController> _savedControllers = new List<PlayerController>();
    [SerializeField]
    private List<Vector2> _savedLaunchCoordinate = new List<Vector2>();

    private Vector2 _launchCoordinate;


    private float _hitRotation;
    public float HitRotation { get { return _hitRotation; } set { _hitRotation = value; } }

    private bool _isLaunched;
    public bool IsLaunched => _isLaunched;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _isPlanted = false;

    }

    private void Start()
    {
        _rigidbody2D.gravityScale = _gravityScale;
        _isCurrentGhost = true;

        _launchCoordinate = _defaultOrientation;
        CreateTrajectory();

        _playerControls.Enable();
        _playerControls.AxeLauncher.LaunchDirection.performed += OnDirectionPerformed;

        _playerControls.AxeLauncher.LaunchController.performed += OnControllerPerformed;
        _playerControls.AxeLauncher.LaunchController.canceled += OnControllerCanceled;
    }

    private bool _isPlanted;

    private void FixedUpdate()
    {
        //Rotate axe

        if (_rigidbody2D.velocity.magnitude >= 1)
        {
            float speed = _maxRotSpeed * Mathf.Clamp(_rigidbody2D.velocity.magnitude / _maxRotByVelocity, 0, 1);
            _axeSprite.rotation = Quaternion.Euler(0f, 0f, _axeSprite.rotation.eulerAngles.z - speed);
            _isPlanted = true;
        }
        else
        {
            _axeSprite.rotation = Quaternion.Euler(new Vector3(0, 0, _plantAxeRotation + _hitRotation));

            if (_isPlanted)
            {
                _isPlanted = false;
                LevelManager.Instance.LevelAudioManager.Play("AxePlant");
                LevelManager.Instance.LevelAudioManager.Stop("AxeThrowLoop");
            }
        }
    }


    private bool _isCurrentGhost;

    public void LaunchSkull()
    {
        if (!_isLaunched)
        {
            _isLaunched = true;

            _rigidbody2D.velocity = _launchCoordinate * _launchStrength;

            _playerControls.Disable();
            _playerControls.AxeLauncher.LaunchDirection.performed -= OnDirectionPerformed;

            _playerControls.AxeLauncher.LaunchController.performed -= OnControllerPerformed;
            _playerControls.AxeLauncher.LaunchController.canceled -= OnControllerCanceled;

            _lineRenderer.enabled = false;

        }

    }


    private void OnDirectionPerformed(InputAction.CallbackContext value)
    {

        _launchCoordinate = value.ReadValue<Vector2>();
        CreateTrajectory();

    }


    private float axisValue;
    private bool inputReceive;

    private void OnControllerPerformed(InputAction.CallbackContext value)
    {
        inputReceive = true;
        StopCoroutine(ArrowMoveTrajectory(value.ReadValue<Vector2>()));
        StartCoroutine(ArrowMoveTrajectory(value.ReadValue<Vector2>()));
    }

    private void OnControllerCanceled(InputAction.CallbackContext value)
    {
        inputReceive = false;
    }

    public IEnumerator ArrowMoveTrajectory(Vector2 inputValue)
    {
        yield return null;

        //Add axis value
        axisValue += inputValue.x * 0.001f * _arrowsSensibility;

        axisValue = Mathf.Clamp(axisValue, -1.0f, 1.0f);

        _launchCoordinate = new Vector2(axisValue, _arrowYValue.Evaluate(axisValue));

        //Trajectory calculation

        CreateTrajectory();

        //Restart function on called

        if (inputReceive)
        {
            StartCoroutine(ArrowMoveTrajectory(inputValue));
        }
    }


    public void CreateTrajectory()
    {
        //Trajectory calculation

        _lineRenderer.enabled = true;

        Vector2 launchVelocity = _launchCoordinate * _launchStrength;

        Vector2[] trajectory = Plot(_rigidbody2D, (Vector2)transform.position, launchVelocity, 400);
        _lineRenderer.positionCount = trajectory.Length;

        Vector3[] linePosition = new Vector3[trajectory.Length];

        for (int i = 0; i < linePosition.Length; i++)
        {
            linePosition[i] = trajectory[i];
        }

        _lineRenderer.SetPositions(linePosition);
    }



    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedUnscaledDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }
}
