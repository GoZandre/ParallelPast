using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    [SerializeField]
    private Image _flagIcon;

    [SerializeField]
    private Sprite[] _flags;
    [SerializeField]
    private Language[] _language;

    private int index;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        switch(gameManager.userDataManager.CurrentLanguage)
        {
            case Language.English:
                index = 0;
                break;
            case Language.French:
                index = 1;
                break;
            case Language.German:
                index = 2;
                break;
            case Language.Portugese:
                index = 3;
                break;
            case Language.Spanish:
                index = 4;
                break;
            case Language.Italian:
                index = 5;
                break;

        }
        _flagIcon.sprite = _flags[index];
    }

    public void SwitchLanguage()
    {
        if(index < _language.Length - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        SetLanguage();
    }

    public void SetLanguage()
    {
        gameManager.userDataManager.SetLanguage(_language[index]);
        _flagIcon.sprite = _flags[index];
    }
}
