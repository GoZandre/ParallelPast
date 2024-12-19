using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OptionsSelectorButton : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private UnityEvent _onSelectEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent _onDeselectEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent _onFinishSelectEvent = new UnityEvent();

    private Animator _animtor;

    private void Awake()
    {
        _animtor = GetComponent<Animator>();
    }

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {

        _animtor.SetBool("IsSelected", true);
        _onSelectEvent.Invoke();
        _onSelectEvent.RemoveAllListeners();

    }

    public void DeselectMenu()
    {
        _animtor.SetBool("IsSelected", false);
        _animtor.SetBool("AlreadySelected", false);

        _onDeselectEvent.Invoke();
        _onDeselectEvent.RemoveAllListeners();

    }

    public void FinishSelection()
    {
        _animtor.SetBool("AlreadySelected", true);

        _onFinishSelectEvent.Invoke();
        _onFinishSelectEvent.RemoveAllListeners();
    }

}
