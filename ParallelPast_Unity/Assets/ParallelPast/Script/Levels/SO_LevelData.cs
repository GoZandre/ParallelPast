using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level/LevelData")]
[Serializable]
public class SO_LevelData : ScriptableObject
{
    [Header("SelfData")]
    [SerializeField]
    private int _levelNumber;
    [SerializeField]
    private string _levelName;
    [SerializeField]
    private string _levelSubName;

    [SerializeField]
    private int _levelMaxGhost;
    [SerializeField]
    private int _levelCountdown;

    [Space(10)]
    [SerializeField]
    private bool _isPremiumLevel;

    [Space(10)]
    [SerializeField]
    private Sprite _levelScreenshot;

    [Header("Unlock Parameters")]
    [SerializeField]
    private SO_LevelData[] _neededLevelToUnlock;
    [SerializeField]
    private bool _isInclusive;


    private SO_LevelReward _reward;

    [SerializeField]
    private So_LevelTransitionType _transitionType;

    [Header("Challenge")]
    [SerializeField]
    private bool _noGhostActive;
    [SerializeField]
    private bool _noTimerActive;
    [SerializeField]
    private bool _noLightActive;

    [Header("Music & Sound")]
    [SerializeField]
    private string _musicName;

    [SerializeField]
    private string _ambientSoundName;

    public int LevelNumber => _levelNumber;
    public string LevelName => _levelName;
    public string LevelSubName => _levelSubName;

    public int LevelMaxGhost => _levelMaxGhost;
    public int LevelCountdown => _levelCountdown;

    public bool IsPremium => _isPremiumLevel;

    public Sprite LevelScreenshot => _levelScreenshot;

    public SO_LevelData[] NeededLevelToUnlock => _neededLevelToUnlock;

    public SO_LevelReward Reward => _reward;

    public So_LevelTransitionType TransitionType => _transitionType;


    public bool NoGhostActive { get { return _noGhostActive; } set { _noGhostActive = value; } }
    public bool NoTimerActive { get { return _noTimerActive; } set { _noTimerActive = value; } }
    public bool NoLightActive { get { return _noLightActive; } set { _noLightActive = value; } }

    public string MusicName => _musicName;
    public string AmbientSoundName => _ambientSoundName;

    public void OpenLevel()
    {
        GameManager sceneLoader = FindObjectOfType<GameManager>();

        if (sceneLoader != null)
        {
            sceneLoader.SetNewLevel(this);
        }
    }

    public void LoadLevel()
    {
        string sceneToLoad;

        if (_levelNumber < 10)
        {
            sceneToLoad = "Level_" + _levelSubName + "0" + _levelNumber;
        }
        else
        {
            sceneToLoad = "Level_" + _levelSubName + _levelNumber;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}


public enum LevelState
{
    Locked,
    Unlock,
    Unlocked,
    PendingCompletion,
    Completed
}
