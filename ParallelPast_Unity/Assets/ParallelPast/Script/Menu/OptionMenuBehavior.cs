using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionMenuBehavior : MonoBehaviour
{
    [SerializeField]
    private Button[] _switchMenuButton;

    private int _switchIndex = 0;

    private bool _canSwitchMenu = true;

    private PlayerControls _playerControls = null;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private UnityEvent _onCloseOption;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        _canSwitchMenu = true;

    }

    

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.OptionMenu.SwitchMenu.performed += SwitchMenu_performed;
        _playerControls.OptionMenu.QuitMenu.performed += QuitMenu_performed;
    }

    private void OnDisable()
    {
        _playerControls.OptionMenu.SwitchMenu.performed -= SwitchMenu_performed;
        _playerControls.OptionMenu.QuitMenu.performed -= QuitMenu_performed;
    }



    private void SwitchMenu_performed(InputAction.CallbackContext obj)
    {
        if(_canSwitchMenu)
        {
            if (_switchIndex >= _switchMenuButton.Length - 1)
            {
                _switchIndex = 0;
            }
            else
            {
                _switchIndex++;
            }

            _canSwitchMenu = false;
            _switchMenuButton[_switchIndex].Select();

            StartCoroutine(SelectorDelay());

        }
        
    }

    private void QuitMenu_performed(InputAction.CallbackContext obj)
    {
        _animator.Play("OptionsMenu_FadeOut");
        _onCloseOption.Invoke();
    }

    public void OpenMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CloseMenu()
    {

        _animator.Play("OptionsMenu_FadeOut");
        _onCloseOption.Invoke();
    }



    private IEnumerator SelectorDelay()
    {
        yield return new WaitForSecondsRealtime(.25f);
        _canSwitchMenu = true;
    }
}
