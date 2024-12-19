using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMenuSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuA;

    [SerializeField] 
    private GameObject _menuB;

    public void SwitchMenu()
    {
        if(_menuA.activeSelf)
        {
            _menuA.SetActive(false);
            _menuB.SetActive(true);
        }
        else if (_menuB.activeSelf)
        {
            _menuA.SetActive(true);
            _menuB.SetActive(false);
        }

        if(GetComponent<ForeGroundTransition>().isActiveAndEnabled)
        {
            StartCoroutine(GetComponent<ForeGroundTransition>().OpenTransition(1f));
        }
        
    }
}
