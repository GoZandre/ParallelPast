using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    private static GameManager _sceneLoaderInstance = null;

    [Header("Audio")]
    [SerializeField]
    private AudioManager _audioManager;
    public AudioManager audioManager => _audioManager;

    

    [Header("Levels")]


    [SerializeField]
    private SO_LevelData _currentLevel;

    [SerializeField]
    private SO_LevelData _lastFinishedLevel = null;

    [Space(10)]
    [SerializeField]
    private SO_LevelData _firstLevel;

    public SO_LevelData CurrentLevel => _currentLevel;
    public SO_LevelData LastFinishedLevel => _lastFinishedLevel;

    

    private SaveManager _saveManager;
    public SaveManager SaveManager => _saveManager;

    private UserDataManager _userDataManager;
    public UserDataManager userDataManager => _userDataManager;

    private SceneModifiers _sceneModifiers;
    public SceneModifiers SceneModifiers => _sceneModifiers;


    //Controls
    [SerializeField]
    private ControlsOptionManager.ControlType _controlType;
    public ControlsOptionManager.ControlType ControlType => _controlType;

    //Setup Instance
    public static GameManager Instance
    {
        get
        {

            if (_sceneLoaderInstance == null)
            {
                _sceneLoaderInstance = FindObjectOfType<GameManager>();

                if (_sceneLoaderInstance == null)
                {
                    GameObject singleton = new GameObject("SceneManager");
                    _sceneLoaderInstance = singleton.AddComponent<GameManager>();
                }
            }

            return _sceneLoaderInstance;
        }
    }

    private void Awake()
    {
        if (_sceneLoaderInstance != null && _sceneLoaderInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _sceneLoaderInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        _saveManager = GetComponent<SaveManager>();
        _userDataManager = GetComponent<UserDataManager>();
        _sceneModifiers = GetComponent<SceneModifiers>();

    }

    public void SetLastFinishedLevel(SO_LevelData level)
    {
        _lastFinishedLevel = level;
    }

    public void ResetLastFinishedLevel()
    {
        _lastFinishedLevel = null;
    }

    public void SetNewLevel(SO_LevelData newLevel)
    {
        _currentLevel = newLevel;
        GameManager.Instance.SaveGame();
    }

    public void SetNextLevel()
    {
        //SetNewLevel(_currentLevel.nextLevel[0]);
    }

    public void SetPreviousLevel()
    {
        SetNewLevel(_currentLevel.NeededLevelToUnlock[0]);
    }

    public void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void LoadTransitionScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TransitionScene");
    }

    public void LoadLevelSelectorScene()
    {
        Debug.Log("Load level selector scene");
        LoadGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelector");
    }


    //Saving system

    public void SaveGame()
    {
        _saveManager.Save();
    }

    public void LoadGame()
    {
        _saveManager.Load();
    }


    public bool IsLevelCompleted(int levelNumber)
    {
        // return _saveManager.IsLevelCompleted(levelNumber);
        return false;
    }


    public void SetControlType(ControlsOptionManager.ControlType newControl)
    {
        _controlType = newControl;
    }

    
}
