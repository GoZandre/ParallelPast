using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectorButton : MonoBehaviour, ISelectHandler
{
    
    public UnityEvent<Button> _onSelectButton = new UnityEvent<Button>();

    [Header("Data")]
    [SerializeField]
    private SO_LevelData _playedLevel;
    public SO_LevelData PlayedLevel => _playedLevel;

    [Header("References")]
    [SerializeField]
    private LevelSelectorManager _levelSelectorManager;
    [SerializeField]
    private Image _starCompletion;

    [Space(5)]

    [SerializeField]
    private Image[] _starChallenge;

    [SerializeField]
    private string _challengeAnimName;

    [Space(5)]

    [SerializeField]
    private TextMeshProUGUI _levelTextMesh;

    [SerializeField]
    private ParticleSystem _unlockParticles;

    [SerializeField]
    private GameObject _locker;

    [Header("World")]

    [SerializeField]
    public Transform AssociatedWorldPoint;


    private RectTransform _rectTransform;
    private Camera _camera;
    private Canvas _canvas;

    [SerializeField]
    private Animator _animator;


    private LevelState _levelState;

    private UserDataManager userDataManager;

    public bool _comingSoonLevel;

    private void Awake()
    {   
        _levelSelectorManager = transform.parent.parent.GetComponent<LevelSelectorManager>();
        _levelSelectorManager.onLoadDatas.AddListener(LoadLevel);


        _rectTransform = GetComponent<RectTransform>();
        _camera = Camera.main;

        _canvas = transform.root.GetComponentInChildren<Canvas>();
        _levelTextMesh.text = _playedLevel.LevelNumber.ToString();

        _onSelectButton.RemoveAllListeners();
        _onSelectButton.AddListener(_levelSelectorManager.SetPreviousButton);

        _animator.SetBool("Locked", true);

        if (_comingSoonLevel)
        {
            _levelTextMesh.text = "?";
        }
    }


    public void LoadLevel()
    {
        //Set Level state

        userDataManager = GameManager.Instance.userDataManager;

        if (userDataManager.LastFinishedLevel == _playedLevel.LevelNumber)
        {
            _camera.transform.position = AssociatedWorldPoint.position;
            _camera.GetComponent<CameraFocusLevel>().SetCurrentSelectedLevel(AssociatedWorldPoint.position);
            GetComponent<Button>().Select();
        }


        if (userDataManager.UnlockedLevel.Contains(_playedLevel.LevelNumber))
        {
            _levelState = LevelState.Unlocked;
            
            if (userDataManager.PendingCompletedLevel.Contains(_playedLevel.LevelNumber))
            {
                _levelState = LevelState.PendingCompletion;
            }
            if (userDataManager.CompletedLevel.Contains(_playedLevel.LevelNumber))
            {
                _levelState = LevelState.Completed;
            }
            
        }
        else if (userDataManager.PendingUnlockLevel.Contains(_playedLevel.LevelNumber))
        {
            _levelState = LevelState.Unlock;
            Debug.Log("Level to unlock : " +  _playedLevel.LevelNumber);
        }
        else
        {
            _levelState = LevelState.Locked;

            for (int i = 0; i < _playedLevel.NeededLevelToUnlock.Length; i++)
            {
                int testedLevelNumber = _playedLevel.NeededLevelToUnlock[i].LevelNumber;

                if (userDataManager.CompletedLevel.Contains(testedLevelNumber) || userDataManager.PendingCompletedLevel.Contains(testedLevelNumber))
                {
                    _levelState = LevelState.Unlock;
                }

                Debug.Log("Level to unlock : " + _playedLevel.LevelNumber);
            }
        }
        

        switch (_levelState)
        {
            case LevelState.Locked:
                gameObject.SetActive(false);
                break;

            case LevelState.Unlock:
                _animator.SetBool("Locked", true);
                _levelSelectorManager.CompleteLevelEvent.AddListener(UnlockLevelButton);
                _starCompletion.gameObject.SetActive(false);              

                //Desactive button until finish animation
                GetComponent<Button>().interactable = false;
                _levelSelectorManager.CompleteLevelEvent.AddListener(ActiveButton);
                break;

            case LevelState.Unlocked:
                _animator.SetBool("Locked", false);
                _starCompletion.gameObject.SetActive(false);

                //Desactive button until finish animation
                GetComponent<Button>().interactable = false;
                _levelSelectorManager.CompleteLevelEvent.AddListener(ActiveButton);
                break;

            case LevelState.PendingCompletion:
                _animator.SetBool("Locked", false);
                _starCompletion.gameObject.SetActive(false);
                GetComponent<Button>().Select();
                
                _levelSelectorManager.CompleteLevelEvent.AddListener(CompleteLevelButton);               
                break;

            case LevelState.Completed:
                _animator.SetBool("Locked", false);
                _starCompletion.gameObject.SetActive(true);

                //Desactive button until finish animation
                GetComponent<Button>().interactable = false;
                _levelSelectorManager.CompleteLevelEvent.AddListener(ActiveButton);

                break;
        }

        if(_playedLevel.IsPremium && !userDataManager.PremiumMode)
        {
            _locker.SetActive(true);
        }
        else
        {
            _locker.SetActive(false);
        }
        
        GameManager gameManager = GameManager.Instance;

        int completedChallenge = 0;
        if (userDataManager.NoGhostCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }
        if (userDataManager.NoTimerCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }
        if (userDataManager.NoLightCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }

        for(int i = 0; i < _starChallenge.Length; i++)
        {
            if(i < completedChallenge)
            {
                _starChallenge[i].gameObject.SetActive(true);
            }
            else
            {
                _starChallenge[i].gameObject.SetActive(false);
            }
        }

        
    }


    public void UnlockLevelButton()
    {
        Debug.Log("Unlock level : " + _playedLevel.LevelNumber);
        _animator.SetTrigger("Unlock");     
    }
    public void UnlockAnimator()
    {
        _levelState = LevelState.Unlocked;
        _animator.SetBool("Locked", false);

        GameManager.Instance.userDataManager.UnlockLevel(_playedLevel.LevelNumber);

    }

    public void CompleteLevelButton()
    {
        _starCompletion.gameObject.SetActive(true);
        _animator.SetTrigger("Complete");
        _levelState = LevelState.Completed;

        GameManager.Instance.userDataManager.CompletePending(_playedLevel.LevelNumber);
    }

    public void ActiveButton()
    {
        GetComponent<Button>().interactable = true;
    }

    private void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(AssociatedWorldPoint.position);
        float scaleFactor = _canvas.scaleFactor;

        _rectTransform.anchoredPosition = new Vector2(screenPos.x / scaleFactor, screenPos.y / scaleFactor);

        float scaleRatio = 5 / _camera.orthographicSize ;

        _rectTransform.localScale = new Vector2(scaleRatio, scaleRatio);

    }

    public void OpenLevel()
    {
        
        _levelSelectorManager.OpenLevelPreview(this);

        int completedChallenge = 0;
        if (userDataManager.NoGhostCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }
        if (userDataManager.NoTimerCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }
        if (userDataManager.NoLightCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }

        if (userDataManager.LastFinishedLevel == _playedLevel.LevelNumber && completedChallenge > 0 && userDataManager.CompletedLevel.Contains(_playedLevel.LevelNumber))
        {
            _animator.Play(_challengeAnimName);
        }
        else
        {
            _animator.SetTrigger("Pressed");
        }

        
        SelectButton(true);
    }

    public void PlaySmallStarSound(int starNb)
    {
        int completedChallenge = 0;
        if (userDataManager.NoGhostCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }
        if (userDataManager.NoTimerCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }
        if (userDataManager.NoLightCompleted.Contains(_playedLevel.LevelNumber)) { completedChallenge += 1; }

        if(starNb <= completedChallenge)
        {
            GetComponent<SoundPlayer>().PlaySound("SmallStar");
        }

        
    }

    public void SelectButton(bool isSelected)
    {
        _animator.SetBool("IsSelected", isSelected);
    }

    public void PlayLevel()
    {
        if (_playedLevel.IsPremium && userDataManager.PremiumMode)
        {

            GameManager sceneLoader = GameManager.Instance;
            sceneLoader.SetNewLevel(_playedLevel);
            _levelSelectorManager.SetTransitionTyp(_playedLevel.TransitionType);
            _levelSelectorManager.StartTransitionToLoadLevel();
        }
        
    }

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        _camera.GetComponent<CameraFocusLevel>().SetCurrentSelectedLevel(AssociatedWorldPoint.position);

        _onSelectButton.Invoke(GetComponent<Button>());

    }

    public void UnlockNextLevel()
    {
        //_levelSelectorManager.ActiveUnlockLevel();
    }

    public void PlayUnlockParticles()
    {
        Instantiate(_unlockParticles, AssociatedWorldPoint);
    }

}

