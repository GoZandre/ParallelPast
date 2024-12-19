using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleElement : MonoBehaviour
{

    private bool _canInteract = true;


    [SerializeField]
    private UnityEvent m_OnInteract;

    [SerializeField]
    private UnityEvent m_OnUninteract;

    [SerializeField]
    private UnityEvent m_InteractFeedback;

    [SerializeField]
    private UnityEvent m_InteractFeedbackOnPlayer;

    [SerializeField]
    private PlayerController _currentActivator;

    public PlayerController CurrentActivator => _currentActivator;


    [Header("Indicator")]

    [SerializeField]
    private InteractIndication _onHoverIndication;
    [SerializeField]
    private InteractIndication _onKeyHoverIndication;
    [SerializeField]
    private Transform _indicationTransform;

    private InteractIndication _currentIndication = null;


    private List<PlayerController> _interactedPlayerController = new List<PlayerController>();
    private List<PlayerController> _currentInteractPlayerController = new List<PlayerController>();

    [Space(5)]
    [SerializeField]
    private InteractNumb _interactNumbPrefab;
    [SerializeField]
    private Vector3 _interactNumbPos = new Vector3(1, 1, 0);
    private int _currentGhostInteractCount;

    [Header("Parameters")]
    [SerializeField]
    private bool _isUniqueInteraction;

    private void Start()
    {
        _currentActivator = null;

        m_OnInteract.RemoveAllListeners();
        m_OnUninteract.RemoveAllListeners();
        m_InteractFeedback.RemoveAllListeners();

        _canInteract = true;

        _currentGhostInteractCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            GhostManager ghostManager = collision.GetComponent<GhostManager>();

            if (playerController.CurrentPower == null)
            {
                playerController.InteractEvent.RemoveAllListeners();
                playerController.UninteractEvent.RemoveAllListeners();

                playerController.InteractEvent.AddListener(Interact);
                playerController.UninteractEvent.AddListener(UnInteract);

                if (playerController.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording && !playerController.HasKey)
                {
                    if (_currentIndication == null)
                    {
                        _currentIndication = Instantiate(_onHoverIndication);
                        _currentIndication.transform.position = _indicationTransform.position;

                        playerController.OnEnterInteraction.Invoke();

                        ghostManager.OnSetupGhost.AddListener(_currentIndication.CloseInteraction);
                    }

                }
                else if (playerController.HasKey && playerController.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
                {
                    if (_currentIndication == null)
                    {
                        _currentIndication = Instantiate(_onKeyHoverIndication);
                        _currentIndication.transform.position = _indicationTransform.position;

                        ghostManager.OnSetupGhost.AddListener(_currentIndication.CloseInteraction);
                    }
                }
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerController playerController = collision.GetComponent<PlayerController>();
            GhostManager ghostManager = collision.GetComponent<GhostManager>();

            if (!playerController.SwitchingCollider)
            {
                if (playerController.IsInteract)
                {
                    UnInteract(playerController);
                }


                playerController.InteractEvent.RemoveAllListeners();
                playerController.UninteractEvent.RemoveAllListeners();

                playerController.OnExitInteraction.Invoke();

                _currentActivator = null;

                if (_currentIndication != null)
                {
                    ghostManager.OnSetupGhost.RemoveListener(_currentIndication.CloseInteraction);

                    _currentIndication.CloseInteraction();
                }
                _currentIndication = null;
            }

            _currentGhostInteractCount = 0;
        }
    }

    public void Interact(PlayerController playerController)
    {

        if (_isUniqueInteraction)
        {
            if (_canInteract)
            {
                m_OnInteract.Invoke();

                _canInteract = false;
                //GetComponent<Collider2D>().enabled = false;

                /*
                if (_currentIndication != null)
                {
                    _currentIndication.CloseInteraction();
                }

                _currentIndication = null;
                */

            }

            CreateInteractCounting();
        }
        else
        {
            _interactedPlayerController.Add(playerController);

            _currentInteractPlayerController.Add(playerController);

            _currentActivator = playerController;


            m_OnInteract.Invoke();
        }

        m_InteractFeedback.Invoke();

        if (playerController.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
        {
            m_InteractFeedbackOnPlayer.Invoke();
        }
    }

    public void UnInteract(PlayerController playerController)
    {

        _currentInteractPlayerController.Remove(playerController);

        _currentActivator = null;

        if (_currentInteractPlayerController.Count <= 0)
        {
            m_OnUninteract.Invoke();
        }

    }

    public void ResetInteraction()
    {
        _canInteract = true;

        //GetComponent<Collider2D>().enabled = true;

    }


    public void CreateInteractCounting()
    {
        if (_currentGhostInteractCount >= 9)
        {
            _currentGhostInteractCount = 1;
        }
        else
        {
            _currentGhostInteractCount++;
        }

        InteractNumb _interactNumb = Instantiate(_interactNumbPrefab);
        _interactNumb.transform.position = transform.position + _interactNumbPos;
        _interactNumb.SetNumber(_currentGhostInteractCount);
    }

    public void PlaySound(string soundName)
    {

        LevelManager.Instance.LevelAudioManager.Play(soundName);

    }
}
