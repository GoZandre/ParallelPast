using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{

    private GameManager _sceneLoader;

    [Header("Level datas")]
    [SerializeField]
    private int _maxGhosts;

    public SO_LevelData LevelData;

    private static LevelManager instance = null;

    public static LevelManager Instance
    { 
        get 
        { 
            return instance; 
        } 
    }

    private void Awake()
    {
        if(instance != null && instance != this) 
        {
            Destroy(this.gameObject);
        }

        instance = this;

        //Find Audio manager
        LevelAudioManager = GameManager.Instance.audioManager;
        if (LevelAudioManager == null)
        {
            LevelAudioManager = new GameObject("EmptyAudioManager").AddComponent<AudioManager>();
        }

        _characterSpawner = GetComponent<CharacterSpawner>();
        _countdownTimer = GetComponent<CountdownTimer>();

        LevelStarted = false;
    }

    [Header("Level")]

    public AudioManager LevelAudioManager;

    private CharacterSpawner _characterSpawner;
    private CountdownTimer _countdownTimer;

    [SerializeField]
    private List<GhostManager> _allGhosts = new List<GhostManager>();

    [SerializeField]
    private GhostManager _currentGhost;
    public GhostManager CurrentGhost => _currentGhost;

    [SerializeField]
    private GhostIndication _ghostIndicationObject;

   public CountdownTimer CountdownTimer => _countdownTimer;

    [Header("Win / Lose")]
    [SerializeField]
    private UnityEvent m_OnWinLevel;

    [SerializeField]
    private UnityEvent m_OnLoseLevel;
    public UnityEvent OnWinLevel => m_OnWinLevel;
    public UnityEvent OnLose => m_OnLoseLevel;

    private bool _hasLoseLevel = false;
    private bool _hasWinLevel = false;

    [Header("Level")]
    [SerializeField]
    private UnityEvent _onStartLevel;
    public UnityEvent OnStartLevel => _onStartLevel;

    public bool LevelStarted;

    [SerializeField]
    private int _nextLevel;
    public int NextLevel => _nextLevel;
    [SerializeField]
    private int _levelNumber;
    public int LevelNumber => _levelNumber;

    [SerializeField]
    private string _levelName;
    public string LevelName => _levelName;

    [SerializeField]
    private string _nextLevelName;
    public string NextLevelName => _nextLevelName;

    [SerializeField]
    private UnityEvent _onSequenceFinishEvent;
    public UnityEvent OnSequenceFinsihEvent => _onSequenceFinishEvent;

    private UnityEvent _onNewGhost = new UnityEvent();
    public UnityEvent OnNewGhost => _onNewGhost;


    [Header("Sequences")]
    [SerializeField]
    private List<SequenceParameters> _sequences = new List<SequenceParameters>();

    private void Start()
    {

        if (FindObjectOfType<GameManager>() != null)
        {
            _sceneLoader = FindObjectOfType<GameManager>();
        }

        //Audio
        LevelAudioManager.PlayMusic(LevelData.MusicName);

        if(LevelData.AmbientSoundName != string.Empty)
        {
            LevelAudioManager.PlayAmbientSound(LevelData.AmbientSoundName);
        }

        _ghostIndicationObject.SetGhostCount(_maxGhosts);

        m_OnWinLevel.RemoveAllListeners();
        m_OnWinLevel.AddListener(WinLevel);

        m_OnLoseLevel.RemoveAllListeners();
        m_OnLoseLevel.AddListener(LoseLevel);

        _onStartLevel.RemoveAllListeners();

        _sequences.Clear();
        _sequences.Add(new SequenceParameters(_currentGhost));
    }

    public void StartLevel()
    {
        if (LevelStarted)
        {
            return;
        }

        LevelAudioManager.Play("StartLevel");
        _onStartLevel.Invoke();
        LevelStarted = true;
    }


    public void AddCharacter(GhostManager ghost)
    {
        _countdownTimer.OnTimerEnd.RemoveAllListeners();
        _countdownTimer.OnTimerEnd.AddListener(ghost.SetUpAsGhost);

        _allGhosts.Add(ghost);
        _currentGhost = ghost;

        _onNewGhost.Invoke();
        

        SetRemainginGhostText();
    }

    public void TestloseConditions(GhostManager ghost)
    {
        if(_allGhosts.Count > _maxGhosts - 1)
        {
            m_OnLoseLevel.Invoke();
            Destroy(ghost.gameObject);
        }
        else
        {
            _onSequenceFinishEvent.Invoke();
            _onSequenceFinishEvent.RemoveAllListeners();

            AddCharacter(ghost);
            SetUpNextSequence(ghost);
        }
    }

    public void SetRemainginGhostText()
    {
        _ghostIndicationObject.UseGhost();
    }

    private void WinLevel()
    {
        if (!_hasLoseLevel)
        {
            LevelAudioManager.Play("OnWin");

            _hasWinLevel = true;
            _currentGhost.GetComponent<PlayerController>().StartWinAnimation();

            _currentGhost.GetComponent<PlayerController>().enabled = false;

            _characterSpawner.StopSpawn();
            _countdownTimer.StopCountDown();
            DesactiveAllGhost();

            if (!GameManager.Instance.userDataManager.UnlockedLevel.Contains(LevelData.LevelNumber))
            {
                _sceneLoader.SetLastFinishedLevel(LevelData);
            }

            SceneModifiers sceneModifier = GameManager.Instance.SceneModifiers;

            GameManager.Instance.userDataManager.CompleteLevel(LevelData.LevelNumber, sceneModifier.NoGhostChallenge, sceneModifier.NoTimerChallenge, sceneModifier.NoLightChallenge) ;
        }

        
        GameManager.Instance.SaveGame();

    }

    private void LoseLevel()
    {
        if (!_hasWinLevel)
        {
            LevelAudioManager.Play("OnLose");

            _currentGhost.GetComponent<PlayerController>().enabled = false;

            _hasLoseLevel = true;
            _characterSpawner.StopSpawn();
            _countdownTimer.StopCountDown();
            DesactiveAllGhost();
        }
        
    }

    private void DesactiveAllGhost()
    {
        for(int i = 0; i < _allGhosts.Count; i++)
        {
            _allGhosts[i].GetComponent<ReplayManager>().enabled = false;
            _allGhosts[i].GetComponent<PlayerController>().enabled = false;
            _allGhosts[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }
    }

    //Sequence

    public void SetUpNextSequence(GhostManager newGhost)
    {
        _sequences.Add(new SequenceParameters(newGhost));
    }

    public void AddActorToDestroyToSequence(GameObject gameObject)
    {
        _sequences[_sequences.Count - 1].AddActorToDestroy(gameObject);
    }

    public void AddActorToSpawnToSequence(GameObject gameObject)
    {
        if(gameObject != null)
        {
            _sequences[_sequences.Count - 1].AddActorToSpawn(gameObject);
        }
        
    }

    public void ReloadLastSequence()
    {
        if(_sequences.Count > 0)
        {
            SequenceParameters currentSequence = _sequences[_sequences.Count - 1];

            Destroy(currentSequence.SequenceGhost.gameObject);

            for (int i = 0; i < currentSequence.ActorToDestroy.Count - 1; i++)
            {
                Destroy(currentSequence.ActorToDestroy[i]);
            }

            for (int i = 0; i <= currentSequence.ActorToSpawn.Count - 1; i++)
            {
                currentSequence.ActorToSpawn[i].SetActive(false);
                currentSequence.ActorToSpawn[i].SetActive(true);
            }

            for (int i = 0; i <= currentSequence.ActorToInteract.Count - 1; i++)
            {
                currentSequence.ActorToInteract[i].Interact(currentSequence.SequenceGhost.attachController);
            }

            for (int i = 0; i <= currentSequence.ActorToUninteract.Count - 1; i++)
            {
                currentSequence.ActorToUninteract[i].UnInteract(currentSequence.SequenceGhost.attachController);
            }

            _allGhosts.RemoveAt(_allGhosts.Count - 1);
            _sequences.RemoveAt(_sequences.Count - 1);

            for(int i = 0; i <= _allGhosts.Count -1; i++)
            {
                _allGhosts[i].ResetGhostReplay();
            }

            GetComponent<CountdownTimer>().ResetTimer();
            GetComponent<CharacterSpawner>().CreateNewCharacter();
        }
        
    }

}


[Serializable]
public class SequenceParameters
{
    private GhostManager _sequenceGhost;

    private List<GameObject> _actorToSpawn = new List<GameObject>();
    private List<GameObject> _actorToDestroy = new List<GameObject>();

    private List<InteractibleElement> _actorToInteract = new List<InteractibleElement>();
    private List<InteractibleElement> _actorToUninteract = new List<InteractibleElement>();

    public GhostManager SequenceGhost => _sequenceGhost;
    public List<GameObject> ActorToSpawn => _actorToSpawn;
    public List<GameObject> ActorToDestroy => _actorToDestroy;
    public List<InteractibleElement> ActorToInteract => _actorToInteract;
    public List<InteractibleElement> ActorToUninteract => _actorToUninteract;

    public SequenceParameters(GhostManager currentGhost)
    {
        _sequenceGhost = currentGhost;
    }

    public void AddActorToSpawn(GameObject actor)
    {
        _actorToSpawn.Add(actor);
    }

    public void AddActorToDestroy(GameObject actor)
    {
        _actorToDestroy.Add(actor);
    }

    public void AddElementToInteract(InteractibleElement interactibleElement)
    {
        _actorToInteract.Add(interactibleElement);
    }

    public void AddElementToUninteract(InteractibleElement interactibleElement)
    {
        _actorToUninteract.Add(interactibleElement);
    }
}
