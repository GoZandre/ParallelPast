using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTranslator : MonoBehaviour
{
    public So_LocalizationSentence Sentence;

    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        GameManager.Instance.userDataManager.OnChangeLanguage.AddListener(SetLanguage);
        SetLanguage();
    }

    private void SetLanguage()
    {
        if(_text == null)
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        _text.text = Sentence.GetText();
    }
}
