using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameCanvasManager : MonoBehaviour
{
    [SerializeField]
    private ForeGroundTransition _foreGroundTransition;

    [SerializeField]
    private PauseScreenBehavior _pauseScreenBehavior;

    [SerializeField]
    private TutorialCanvasBehavior _tutorialCanvas;

    [SerializeField]
    private NoLightBehavior _noLightBehavior;

    [SerializeField]
    private RectTransform _cooldownContainer;

    [Header("INPUTS")]
    public PlayerControls inputs = null;

    private void Awake()
    {

        inputs = new PlayerControls();

    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Gameplay.Move.performed += Input_Performed; ;
        inputs.Gameplay.Move.canceled += Input_Performed;

        inputs.Gameplay.Interact.performed += Input_Performed;
        inputs.Gameplay.Interact.canceled += Input_Performed;

        inputs.Gameplay.Jump.performed += Input_Performed;
        inputs.Gameplay.Jump.canceled += Input_Performed;

        inputs.Gameplay.Pause.performed += Pause_performed; ;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PauseGame();
        Debug.Log("PAUSE PERFORMED;*");
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Gameplay.Move.performed -= Input_Performed;
        inputs.Gameplay.Move.canceled -= Input_Performed;

        inputs.Gameplay.Interact.performed -= Input_Performed;
        inputs.Gameplay.Interact.canceled -= Input_Performed;

        inputs.Gameplay.Jump.performed -= Input_Performed;
        inputs.Gameplay.Jump.canceled -= Input_Performed;
    }

    private void Input_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!LevelManager.Instance.LevelStarted)
        {
            LevelManager.Instance.StartLevel();
            GetComponent<Animator>().SetTrigger("Play");

            
            inputs.Gameplay.Move.performed -= Input_Performed;
            inputs.Gameplay.Move.canceled -= Input_Performed;

            inputs.Gameplay.Interact.performed -= Input_Performed;
            inputs.Gameplay.Interact.canceled -= Input_Performed;

            inputs.Gameplay.Jump.performed -= Input_Performed;
            inputs.Gameplay.Jump.canceled -= Input_Performed;
        }
    }


    private void Start()
    {
        _menuOpen = false;
        _canPause = true;
        StartCoroutine(_foreGroundTransition.OpenTransition(1f));

        if(GameManager.Instance == null)
        {
            return;
        }


        if (GameManager.Instance.SceneModifiers.NoLightChallenge)
        {
            _noLightBehavior.gameObject.SetActive(true);
        }
        else
        {
            _noLightBehavior.gameObject.SetActive(false);
        }

        if(GameManager.Instance.SceneModifiers.NoTimerChallenge)
        {
            _cooldownContainer.localScale = Vector3.zero;
        }
        else
        {
            _cooldownContainer.localScale = Vector3.one;
        }

        if(!GameManager.Instance.SceneModifiers.OpenTutorial && _tutorialCanvas != null)
        {
            Destroy(_tutorialCanvas.gameObject);
        }
        else
        {
            GameManager.Instance.SceneModifiers.OpenTutorial = false;
        }

    }


    //In Game canvas functions

    private bool _canPause;
    private bool _menuOpen;

    
    public void StartPressed()
    {
        if(_tutorialCanvas != null)
        {
            _tutorialCanvas.Continue();
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Play");
        }
        
    }
    public void EndLevel()
    {
        _canPause = false;
        GetComponent<Animator>().SetTrigger("EndLevel");
    }
    public void PlayGame()
    {
        LevelManager.Instance.StartLevel();
    }

    public void PauseGame()
    {
        if (_pauseScreenBehavior.gameObject.activeSelf == true && _menuOpen == true && _canPause)
        {
            _menuOpen = false;
            _pauseScreenBehavior.ContinueGame();
        }
        else if (_menuOpen == false && _canPause)
        {
            _menuOpen = true;
            _pauseScreenBehavior.gameObject.SetActive(true);
            _pauseScreenBehavior.PauseGame();
        }
    }

    private string _levelToLoad;


    public void SetLevelToLoad(int levelToLoad, string levelName)
    {

        StartCoroutine(_foreGroundTransition.CloseTransition(1f));
    }


    public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_levelToLoad);
    }
}
