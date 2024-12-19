using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsOptionManager : MonoBehaviour
{


    [SerializeField]
    private UnityEvent _onControllerXbox;
    [SerializeField]
    private UnityEvent _onControllerPs5;
    [SerializeField]
    private UnityEvent _onKeyboard;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;

        InputSystem.onDeviceChange += OnDeviceChange;

        DetectActiveDevices();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                Debug.Log($"Device added: {device.name}");
                DetectActiveDevices();
                break;
            case InputDeviceChange.Removed:
                Debug.Log($"Device removed: {device.name}");
                DetectActiveDevices();
                break;
            case InputDeviceChange.UsageChanged:
                Debug.Log($"Device usage changed: {device.name}");
                DetectActiveDevices();
                break;
        }
    }

    private void DetectActiveDevices()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad gamepad)
            {
                if (gamepad.name.Contains("DualSense"))
                {
                    Debug.Log("PS5 Controller detected");
                    _gameManager.SetControlType(ControlType.PS5);
                    _onControllerPs5.Invoke();
                }
                else if (gamepad.name.Contains("Xbox"))
                {
                    Debug.Log("Xbox Controller detected");
                    _gameManager.SetControlType(ControlType.Xbox);
                    _onControllerXbox.Invoke();
                }
                else
                {
                    Debug.Log($"Other gamepad detected: {gamepad.name}");
                    _gameManager.SetControlType(ControlType.Xbox);
                    _onControllerXbox.Invoke();
                }
            }
            else if (device is Keyboard)
            {
                Debug.Log("Keyboard detected");
                _gameManager.SetControlType(ControlType.Keyboard);
                _onKeyboard.Invoke();
            }
            else
            {
                Debug.Log($"Other device detected: {device.name}");
                _gameManager.SetControlType(ControlType.Keyboard);
                _onKeyboard.Invoke();
            }
        }
    }

    public enum ControlType
    {
        Keyboard,
        PS5,
        Xbox
    }

}
