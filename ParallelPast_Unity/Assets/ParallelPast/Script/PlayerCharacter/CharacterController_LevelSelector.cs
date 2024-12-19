using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController_LevelSelector : MonoBehaviour
{

    public float CharacterSpeed;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    //  ↓  Replace this with a canvas manager  ↓
    [SerializeField]
    private Canvas _mainCanvas;

    public Canvas getMainCanvas { get { return _mainCanvas; } }
    //

    private PlayerControls _playerControls = null;

    private Vector2 _controllerVector;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        //_playerControls.Enable();
        //_playerControls.LevelSelector.Movement.performed += OnMovementPerformed;
        //_playerControls.LevelSelector.Movement.canceled += OnMovementCanceled; 
    }

    private void OnDisable()
    {
        //_playerControls.Disable();
        //_playerControls.LevelSelector.Movement.performed -= OnMovementPerformed;
        //_playerControls.LevelSelector.Movement.canceled -= OnMovementCanceled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _controllerVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        _controllerVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _controllerVector * CharacterSpeed;

        _animator.SetFloat("VelocityX", _rigidbody.velocity.x);
        _animator.SetFloat("VelocityY", _rigidbody.velocity.y);
    }

    //GUI

    [Header("GUI")]
    [SerializeField]
    private ActionPopUp _signPopup;

    public ActionPopUp EncouterSign(Transform encounteredSign)
    {
        ActionPopUp SignPopUp = Instantiate(_signPopup, _mainCanvas.transform);
        SignPopUp.SetObjectToFollow(encounteredSign);

        return SignPopUp;

    }
}
