using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeBehavior : MonoBehaviour
{
    
    private Toggle _toggle;
    public Toggle ToggleElement => _toggle;

    [SerializeField]
    private ChallengeType _challengeType;

    [Header("REFERENCES")]
    [SerializeField]
    private Image _completionStar;

    private SO_LevelData levelData;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    public void CreateChallenge(SO_LevelData level)
    {
        UserDataManager userDataManager = GameManager.Instance.userDataManager;
        levelData = level;

        switch(_challengeType)
        {
            case ChallengeType.NoGhost:
                
                _toggle.isOn = levelData.NoGhostActive;
                ActiveChallenge();
                SetCompletion(userDataManager.NoGhostCompleted.Contains(levelData.LevelNumber));
                break;

            case ChallengeType.NoTimer:
                
                _toggle.isOn = levelData.NoTimerActive;
                ActiveChallenge();
                SetCompletion(userDataManager.NoTimerCompleted.Contains(levelData.LevelNumber));
                break;
            
            case ChallengeType.NoLight:
                
                _toggle.isOn = levelData.NoLightActive;
                ActiveChallenge();
                SetCompletion(userDataManager.NoLightCompleted.Contains(levelData.LevelNumber));
                break;
        }
    }

    private void SetCompletion(bool _isCompleted)
    {
        if (_isCompleted)
        {
            _completionStar.gameObject.SetActive(true);
        }
        else
        {
            _completionStar.gameObject.SetActive(false);
        }
    }

    public void ActiveChallenge()
    {
        bool toggleValue = _toggle.isOn;

        Debug.Log(toggleValue);

        switch (_challengeType)
        {
            case ChallengeType.NoGhost:
                levelData.NoGhostActive = toggleValue;
                GameManager.Instance.SceneModifiers.SetNoGhostChallegne(toggleValue);
                break;

            case ChallengeType.NoTimer:
                levelData.NoTimerActive = toggleValue;
                GameManager.Instance.SceneModifiers.SetNoTimerChallenge(toggleValue);
                break;

            case ChallengeType.NoLight:
                levelData.NoLightActive = toggleValue;
                GameManager.Instance.SceneModifiers.SetNoLightChallenge(toggleValue);
                break;
        }
    }


    public void Lock()
    {
        if(_toggle == null)
        {
            _toggle = GetComponent<Toggle>();
        }
        _toggle.interactable = false;
        _toggle.isOn = false;
        ActiveChallenge();
    }
    public void Unlock()
    {
        if (_toggle == null)
        {
            _toggle = GetComponent<Toggle>();
        }
        _toggle.interactable = true;
    }
}

public enum ChallengeType
{
    NoGhost,
    NoTimer,
    NoLight
}
