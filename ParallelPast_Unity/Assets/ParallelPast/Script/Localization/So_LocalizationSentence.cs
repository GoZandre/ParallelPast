using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Localization", menuName = "ScriptableObjects/Misc/Localization")]
[Serializable]
public class So_LocalizationSentence : ScriptableObject
{
    [SerializeField]
    private string _english;
    [SerializeField]
    private string _french;
    [SerializeField]
    private string _portugese;
    [SerializeField]
    private string _german;
    [SerializeField]
    private string _spanish;
    [SerializeField]
    private string _italian;

    public string GetText()
    {

        Language language = GameManager.Instance.userDataManager.CurrentLanguage;
        switch (language)
        {
                case Language.English:
                return _english;
            case Language.French:
                return _french;
            case Language.Portugese:
                return _portugese;
            case Language.Spanish:
                return _spanish;
            case Language.German:
                return _german;
            case Language.Italian:
                return _italian;
              
        }
        return _english;
    }
}

public enum Language
{
    English,
    French, 
    Portugese, 
    German, 
    Spanish, 
    Italian
}
