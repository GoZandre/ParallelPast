using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform _leverPivot;

    [SerializeField]
    private Vector3 _leverActiveRotation;
    [SerializeField]
    private Vector3 _leverUnactiveRotation;

    private Quaternion _leverObjective;

    [SerializeField]
    private float _leverInterpolation;

    [Header("Interaction")]
    [SerializeField]
    private UnityEvent m_OnActiveLever;
    [SerializeField]
    private UnityEvent m_OnDeactiveLever;

    private void Start()
    {
        m_OnActiveLever.RemoveAllListeners();
        m_OnDeactiveLever.RemoveAllListeners();

        _leverPivot.rotation = Quaternion.Euler(_leverUnactiveRotation);
        DeactiveLever();
    }

    public void ActiveLever()
    {
        LevelManager.Instance.LevelAudioManager.Play("LeverOn");
        m_OnActiveLever.Invoke();
        _leverObjective = Quaternion.Euler(_leverActiveRotation);
    }

    public void DeactiveLever()
    {
        LevelManager.Instance.LevelAudioManager.Play("LeverOff");
        m_OnDeactiveLever.Invoke();
        _leverObjective = Quaternion.Euler(_leverUnactiveRotation);
    }

    private void Update()
    {
        if (_leverObjective != _leverPivot.rotation)
        {
            _leverPivot.rotation = Quaternion.Slerp(_leverPivot.rotation, _leverObjective, _leverInterpolation * Time.deltaTime);
        }

    }
}
