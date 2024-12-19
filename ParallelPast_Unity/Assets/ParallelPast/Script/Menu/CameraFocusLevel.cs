using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class CameraFocusLevel : MonoBehaviour
{
    private Vector3 _currentSelectedLevelPosition;
    private float _wantedSize;

    private Camera _camera;

    

    [Header("Controls")]
    [SerializeField]
    private bool _focusLevel;

    [SerializeField]
    private float _cameraLerpSpeed;

    [SerializeField]
    private Vector3 _offset;
    public Vector3 Offset { get { return _offset; } set { _offset = value; } }

    [Space(10)]
    [SerializeField]
    private float _cameraZoomSensibility;

    [SerializeField]
    private float _cameraZoomLerpSpeed;

    [SerializeField]
    private Vector2 _minMaxZoomValue;

    private PlayerControls _playerControls = null;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _playerControls = new PlayerControls();

        _wantedSize = _camera.orthographicSize;

        _focusLevel = true;

    }

    private void Start()
    {
        _playerControls.Enable();
        _playerControls.LevelSelector.Zoom.performed += Zoom_performed;

        _focusLevel = true;
   
    }

    private void Zoom_performed(InputAction.CallbackContext obj)
    {
        float zoomValue = obj.ReadValue<float>();

        if (zoomValue < 0 && _wantedSize < _minMaxZoomValue.y)
        {
            _wantedSize += zoomValue * _cameraZoomSensibility * -0.001f;
        }
        else if (zoomValue > 0 && _wantedSize > _minMaxZoomValue.x)
        {
            _wantedSize += zoomValue * _cameraZoomSensibility * -0.001f;
        }
        
    }

    public void SetCurrentSelectedLevel(Vector2 newLevelPosition)
    {
        _currentSelectedLevelPosition = new Vector3(newLevelPosition.x, newLevelPosition.y, -10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _currentSelectedLevelPosition + _offset, _cameraLerpSpeed);

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _wantedSize, _cameraZoomLerpSpeed);
    }

}
