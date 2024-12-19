using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldLocker : MonoBehaviour
{

    public So_WorldLocker LockerData;
    public UnityEvent OnUnlockEvent;
    public UnityEvent OnOpenEvent;


    private Animator _animator;
    private BoxCollider2D _boxCollider;

    [SerializeField]
    private Transform _unlockCameraPoint;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if(LockerData != null)
        {
            if(LockerData.isOpened)
            {
                Unlock();
            }
        }

        OnUnlockEvent.RemoveAllListeners();
    }

    public void AttachUnlockCamera()
    {
        WorldMapManager.Instance.getUnlockCamera.parent = _unlockCameraPoint;
    }

    public void UnlockReward()
    {
        SO_LevelReward currentReward = WorldMapManager.Instance.CurrentRewardSequence.RewardActivation;

        LockerData.UnlockReward(currentReward);
        _animator.SetTrigger(currentReward.RewardName);
    }

    public void CheckAllUnlockCondition()
    {
        if (LockerData.CheckUnlockConditions())
        {
            _animator.SetTrigger("Open");
        }
        else
        {
            WorldMapManager.Instance.CurrentRewardSequence.ExecuteNextSequenceElem();
        }
    }

    public void Unlock()
    {
        if(_boxCollider != null)
        {
            _boxCollider.enabled = false;
        }

        OnUnlockEvent.Invoke();
        OnUnlockEvent.RemoveAllListeners();
    }

    public void OnFinishOpenAnimation()
    {
        LockerData.Unlock();
        Unlock();

        WorldMapManager.Instance.CurrentRewardSequence.ExecuteNextSequenceElem();
    }

}
