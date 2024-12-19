using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelPreview : MonoBehaviour
{
    GameManager gameManager;

    public SO_LevelData LevelData;

    [SerializeField]
    private LevelSelectorManager _levelSelectorManager;

    private PlayerControls _playerControls = null;

    [Header("REFERENCES")]
    public So_LocalizationSentence _levelWord;
    [SerializeField]
    private TextMeshProUGUI _levelName;
    [SerializeField]
    private Image _levelPreview;
    public So_LocalizationSentence _playWord;
    [SerializeField]
    private TextMeshProUGUI _playText;

    [Space(10)]

    [SerializeField]
    private Image _completionStar;
    [SerializeField]
    private GameObject _challengeLocker;

    [Space(10)]

    [SerializeField]
    private Image _backgroound;
    [SerializeField]
    private Image[] _decorations;

    [Space(10)]

    [SerializeField]
    private ChallengeBehavior _noGhostChallenge;
    [SerializeField]
    private ChallengeBehavior _noTimerChallenge;
    [SerializeField]
    private ChallengeBehavior _noLightChallenge;

    [Space(10)]

    [SerializeField]
    private Button _playButton;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.LevelSelector.Play.performed += Play_performed;
        _playerControls.LevelSelector.Challenge_A.performed += Challenge_A_performed;
        _playerControls.LevelSelector.Challenge_B.performed += Challenge_B_performed;
        _playerControls.LevelSelector.Challenge_C.performed += Challenge_C_performed;
    }

    private void OnDisable()
    {
        _playerControls.LevelSelector.Play.performed -= Play_performed;
        _playerControls.LevelSelector.Challenge_A.performed -= Challenge_A_performed;
        _playerControls.LevelSelector.Challenge_B.performed -= Challenge_B_performed;
        _playerControls.LevelSelector.Challenge_C.performed -= Challenge_C_performed;
    }

    private void Challenge_C_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        if (!_challengeUnlock)
        {
            return;
        }

        _noLightChallenge.ToggleElement.isOn = !_noLightChallenge.ToggleElement.isOn;
    }

    private void Challenge_B_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!_challengeUnlock)
        {
            return;
        }
        _noTimerChallenge.ToggleElement.isOn = !_noTimerChallenge.ToggleElement.isOn;
    }

    private void Challenge_A_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!_challengeUnlock)
        {
            return;
        }
        _noGhostChallenge.ToggleElement.isOn = !_noGhostChallenge.ToggleElement.isOn;
    }

    private void Play_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayLevel();
    }

    private bool _challengeUnlock;

    public void SetLevel(SO_LevelData levelData)
    {

        LevelData = levelData;

        gameManager = GameManager.Instance;
        UserDataManager userDataManager = gameManager.userDataManager;

        _levelName.text = SetLevelName(levelData.LevelNumber);

        _playText.text = _playWord.GetText();

        _playText.color = LevelData.TransitionType.transitionColor;

        if (LevelData.LevelScreenshot != null)
        {
            _levelPreview.sprite = LevelData.LevelScreenshot;
        }

        _completionStar.gameObject.SetActive(userDataManager.CompletedLevel.Contains(levelData.LevelNumber) || userDataManager.PendingCompletedLevel.Contains(levelData.LevelNumber));

        //Decoration

        _backgroound.color = LevelData.TransitionType.transitionColor;

        Color baseTextColor = LevelData.TransitionType.transitionTextColor;
        baseTextColor = new Color(baseTextColor.r, baseTextColor.g, baseTextColor.b, 0.1f);

        for (int i = 0; i < _decorations.Length; i++)
        {
            _decorations[i].color = baseTextColor;
        }

        //Challenge

        bool isChallengeUnlocked = (userDataManager.CompletedLevel.Contains(levelData.LevelNumber) || userDataManager.PendingCompletedLevel.Contains(levelData.LevelNumber)) && userDataManager.PremiumMode;
        _challengeUnlock = isChallengeUnlocked;

        _challengeLocker.SetActive(!isChallengeUnlocked);

        if(isChallengeUnlocked)
        {
            _noGhostChallenge.CreateChallenge(levelData);
            _noTimerChallenge.CreateChallenge(levelData);
            _noLightChallenge.CreateChallenge(levelData);

            _noGhostChallenge.Unlock();
            _noTimerChallenge.Unlock();
            _noLightChallenge.Unlock();

        }
        else
        {
            _noGhostChallenge.CreateChallenge(levelData);
            _noTimerChallenge.CreateChallenge(levelData);
            _noLightChallenge.CreateChallenge(levelData);

            _noGhostChallenge.Lock();
            _noTimerChallenge.Lock();
            _noLightChallenge.Lock();
        }
    }

    public void UpdateLevel()
    {
        SetLevel(LevelData);
    }

    public void PlayLevel()
    {
        gameManager.SetNewLevel(LevelData);
        _levelSelectorManager.SetTransitionTyp(LevelData.TransitionType);
        _levelSelectorManager.StartTransitionToLoadLevel();
    }


    public string SetLevelName(int levelNumber)
    {
        if (levelNumber < 10)
        {
            return new string(_levelWord.GetText() + " 0" + levelNumber.ToString());
        }
        else
        {
            return new string(_levelWord.GetText() + " " + levelNumber.ToString());
        }
    }
}
