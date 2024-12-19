using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSelector : MonoBehaviour
{
    [SerializeField] private Button _button;

    public void SelectButton()
    {
        _button.Select();
    }
}
