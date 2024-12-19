using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputsData", menuName = "ScriptableObjects/Misc/InputsData")]
public class So_IconsDatas : ScriptableObject
{
    [SerializeField]
    private Sprite _errorSprite;

    [Header("Xbox")]

    [SerializeField]
    private Sprite _xBoxNorthButton;
    [SerializeField]
    private Sprite _xBoxSouthButton;
    [SerializeField]
    private Sprite _xBoxEastButton;
    [SerializeField]
    private Sprite _xBoxWestButton;

    [Header("Ps4")]

    [SerializeField]
    private Sprite _ps4NorthButton;
    [SerializeField]
    private Sprite _ps4SouthButton;
    [SerializeField]
    private Sprite _ps4EastButton;
    [SerializeField]
    private Sprite _ps4WestButton;

    [Header("Pc")]

    [SerializeField]
    private Sprite _pcNorthButton;
    [SerializeField]
    private Sprite _pcSouthButton;
    [SerializeField]
    private Sprite _pcEastButton;
    [SerializeField]
    private Sprite _pcWestButton;



    public Sprite GetInputIcon(ControllerType controllerType, InputType inputType) 
    {
        switch(controllerType)
        {
            case (ControllerType.Xbox):    
                return GetXboxInputIcon(inputType);
            case (ControllerType.Ps4):
                return GetPs4InputIcon(inputType);
            case (ControllerType.Pc):
                return GetPcInputIcon(inputType);
        }

        return _errorSprite;
    }
    public Sprite GetXboxInputIcon(InputType inputType)
    {
        switch (inputType)
        {
            case (InputType.NorthButton): return _xBoxNorthButton;
            case (InputType.SouthButton): return _xBoxSouthButton;
            case (InputType.WestButton): return _xBoxWestButton;
            case (InputType.EastButton): return _xBoxEastButton;
        }

        return _errorSprite;
    }

    public Sprite GetPs4InputIcon(InputType inputType)
    {
        switch(inputType)
        {
            case (InputType.NorthButton): return _ps4NorthButton;
            case (InputType.SouthButton): return _ps4SouthButton;
            case (InputType.WestButton): return _ps4WestButton;
            case (InputType.EastButton): return _ps4EastButton;
        }

        return _errorSprite;
    }

    public Sprite GetPcInputIcon(InputType inputType)
    {
        switch (inputType)
        {
            case (InputType.NorthButton): return _pcNorthButton;
            case (InputType.SouthButton): return _pcSouthButton;
            case (InputType.WestButton): return _pcWestButton;
            case (InputType.EastButton): return _pcEastButton;
        }

        return _errorSprite;
    }

}

public enum ControllerType
{
    Xbox,
    Ps4,
    Pc
}

public enum InputType
{
    NorthButton,
    SouthButton,
    EastButton,
    WestButton
}
