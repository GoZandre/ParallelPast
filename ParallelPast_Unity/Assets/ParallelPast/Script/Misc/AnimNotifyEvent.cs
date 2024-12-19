using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimNotifyEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _notifyA = new UnityEvent();

    [SerializeField]
    private UnityEvent _notifyB = new UnityEvent();

    [SerializeField]
    private UnityEvent _notifyC = new UnityEvent();

    public void NotifyA()
    {
        _notifyA.Invoke();
        _notifyA.RemoveAllListeners();
    }

    public void NotifyB()
    {
        _notifyB.Invoke();
        _notifyB.RemoveAllListeners();
    }

    public void NotifyC()
    {
        _notifyC.Invoke();
        _notifyC.RemoveAllListeners();
    }

}
