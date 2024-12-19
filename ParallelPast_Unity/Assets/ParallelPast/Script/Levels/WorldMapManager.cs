using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldMapManager : MonoBehaviour
{
    private GameManager _sceneLoader;
    private SequenceManager _sequenceManager;

    private Sequence _currentRewardSequence;
    public Sequence CurrentRewardSequence => _currentRewardSequence;

    private static WorldMapManager instance = null;


    public static WorldMapManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        _sequenceManager = GetComponent<SequenceManager>();
    }


    [Header("References")]

    [SerializeField]
    private Transform _unlockCamera;

    public Transform getUnlockCamera { get { return _unlockCamera; } }

    private void Start()
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            _sceneLoader = FindObjectOfType<GameManager>();
        }
    }

    public void TryToExecuteSequence() 
    { 
        if(_sceneLoader != null)
        {
            if(_sceneLoader.LastFinishedLevel != null)
            {
                ExecuteSequenceOnReward(_sceneLoader.LastFinishedLevel.Reward);
            }
        }
    }

    
    private void ExecuteSequenceOnReward(SO_LevelReward levelReward)
    {
        _currentRewardSequence = _sequenceManager.GetSequence(levelReward);

        if( _currentRewardSequence != null )
        {
            _currentRewardSequence.InitSequence();
            _currentRewardSequence.ExecuteNextSequenceElem();
        }
    }


    // User interface reward screen

    public void OpenOnRewardUI()
    {

    }

}
