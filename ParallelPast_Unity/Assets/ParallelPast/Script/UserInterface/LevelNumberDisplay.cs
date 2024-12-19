using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelNumberDisplay : MonoBehaviour
{
    public So_LocalizationSentence _levelWord;
    [SerializeField]
    private TextMeshProUGUI _levelText;
    void Start()
    {
        if(_levelText != null)
        {
            int levelCount = LevelManager.Instance.LevelData.LevelNumber;

            if (levelCount < 10)
            {
                _levelText.text = _levelWord.GetText() + " 0" + levelCount;
            }
            else
            {
                _levelText.text = _levelWord.GetText() + " " + levelCount;
            }
        }
       
    }

}
