using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputIconManager : MonoBehaviour
{
    private Image _image;

    [SerializeField]
    private So_IconsDatas _iconsDatas;


    public ControllerType ControllerType;
    public InputType InputType;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = _iconsDatas.GetInputIcon(ControllerType, InputType);
    }
}
