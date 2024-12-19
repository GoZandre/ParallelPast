using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectorManager : MonoBehaviour
{
    [SerializeField]
    private Button _previousLevelButton;

    [SerializeField] 
    private LevelSelectorButton _currentLevel;

    [SerializeField]
    private MainMenu_Manager _mainMenuManager;

    [SerializeField]
    private ForeGroundTransition _foreGroundTransition;

    [SerializeField]
    private LevelPreview _levelPreview;
    [SerializeField]
    private CameraFocusLevel _cameraFocus;

    //Load datas
    private UnityEvent _onLoadDatas = new UnityEvent();
    public UnityEvent onLoadDatas => _onLoadDatas;


    private UnityEvent _completeLevelEvent = new UnityEvent();
    private UnityEvent _unlockLevelEvent = new UnityEvent();
    
    private bool _completeAnimation = false;
    private bool _unlockAnimation = false;
    public bool CompleteAnimation { set { _completeAnimation = value; } }
    public bool UnlockAnimation { set { _unlockAnimation = value; } }



    public UnityEvent CompleteLevelEvent => _completeLevelEvent;
    public UnityEvent UnlockLevelEvent => _unlockLevelEvent;

    public void ActiveCompleteLevel() 
    { 
        _completeLevelEvent.Invoke(); 
        _completeLevelEvent.RemoveAllListeners();

        UserDataManager user = GameManager.Instance.userDataManager;

        if (user.PendingCompletedLevel.Count <= 0)
        {
            ActiveUnlockLevel();
        }

        SelectPreviousButton();
    }
    public void ActiveUnlockLevel() { _unlockLevelEvent.Invoke(); _unlockLevelEvent.RemoveAllListeners(); }


    [Header("StartLevel")]
    [SerializeField]
    private ForeGroundTransition _foreGroundTransitionStartLevel;

    private PlayerControls _playerControls = null;

    private bool _canBack;

    private void Awake()
    {
        Time.timeScale = 1f;
        _playerControls = new PlayerControls();
        
    }

    private void Start()
    {
        
        _canBack = true;
            

        //GameManager.Instance.LoadGame();
        _onLoadDatas.Invoke();
        _onLoadDatas.RemoveAllListeners();

        _playerControls.Enable();
        _playerControls.LevelSelector.Back.performed += Back_performed;

        StartCoroutine(_foreGroundTransition.OpenTransition(1f));

        GameManager.Instance.SceneModifiers.OpenTutorial = true;
    }

    private void Back_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
    }

    public void OpenLevelPreview(LevelSelectorButton levelButton)
    {
        if(_currentLevel != null)
        {
            _currentLevel.SelectButton(false);
        }
        

        _levelPreview.gameObject.SetActive(true);
        _levelPreview.SetLevel(levelButton.PlayedLevel);

        _currentLevel = levelButton;

        _cameraFocus.Offset = new Vector3(3f,0,0);
    }

    private void StartLevel()
    {
        GameManager sceneLoader = GameManager.Instance;
        sceneLoader.LoadTransitionScene();
    }

    public void SetTransitionTyp(So_LevelTransitionType type)
    {
        _foreGroundTransitionStartLevel.SetUpTransition(type);
    }

    public void StartTransitionToLoadLevel()
    {

        _foreGroundTransitionStartLevel.gameObject.SetActive(true);
        _canBack = false;

        _foreGroundTransitionStartLevel.OnCloseTransitionExecute.AddListener(StartLevel);
        StartCoroutine(_foreGroundTransitionStartLevel.CloseTransition(1f,0f, true));
    }

    public void SelectPreviousButton()
    {
        if(_previousLevelButton != null)
        {
            _previousLevelButton.Select();            
        }
        
    }

    public void SetPreviousButton(Button newButton)
    {
        _previousLevelButton = newButton;
    }

    private void Update()
    {
        
  
        
    }

    public void BackToMenu()
    {
        if (_canBack)
        {
            _canBack = false;
            _foreGroundTransition.gameObject.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);

            GameManager.Instance.LoadMenu();

            //StartCoroutine(_foreGroundTransition.CloseTransition(1f, 0.25f));
        }
    }
    public void DesactiveSelf()
    {
        gameObject.SetActive(false);
    }



}
