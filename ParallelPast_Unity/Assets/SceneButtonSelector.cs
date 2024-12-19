using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneButtonSelector : MonoBehaviour
{
    [SerializeField]
    private Button _nextButton;



    public void SelectNextButton()
    {
        _nextButton.Select();
    }

    private void Update()
    {

    }
}
