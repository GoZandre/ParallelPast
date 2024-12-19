using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PadLockBehavior : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_OnUnlockEvent;

    [SerializeField]
    private UnityEvent m_OnResetEvent;


    [SerializeField]
    private InteractIndication _onHoverIndication;
    [SerializeField]
    private Transform _indicationTransform;

    private InteractIndication _currentIndication = null;

    private Animator _padAnimator;

    private void Start()
    {
        _padAnimator = GetComponent<Animator>();

        m_OnResetEvent.RemoveAllListeners();
        m_OnUnlockEvent.RemoveAllListeners();
        m_OnUnlockEvent.AddListener(UnlockPad);
    }

    private void OnEnable()
    {
        _padAnimator = GetComponent<Animator>();
        _padAnimator.SetTrigger("Restart");

        m_OnResetEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording && playerController.HasKey)
            {
                playerController.UseKey();

                m_OnUnlockEvent.Invoke();   
            }
            else if(playerController.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                _currentIndication = Instantiate(_onHoverIndication);
                _currentIndication.transform.position = _indicationTransform.position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReplayManager replayManager = collision.GetComponent<ReplayManager>();

            if (replayManager.CurrentReplayStat == ReplayStat.Recording && _currentIndication != null)
            {
                _currentIndication.CloseInteraction();
            }
        }
    }

    private void UnlockPad()
    {
        LevelManager.Instance.AddActorToSpawnToSequence(gameObject);
        LevelManager.Instance.LevelAudioManager.Play("PadLock");
        _padAnimator.SetTrigger("OpenPad");
    }

    public void DestroyPad()
    {
        gameObject.SetActive(false);
    }
}
