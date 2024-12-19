using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTransition : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelNumber;
    [SerializeField]
    private TextMeshProUGUI _levelName;

    [SerializeField]
    private TextMeshProUGUI _levelMaxGhost;

    [SerializeField]
    private Image _backgroundImage;

    private GameManager sceneLoader;

    private void Awake()
    {
        sceneLoader = FindObjectOfType<GameManager>();

        _backgroundImage.color = sceneLoader.CurrentLevel.TransitionType.transitionColor;
        _levelName.color = sceneLoader.CurrentLevel.TransitionType.transitionTextColor;
    }
    void Start()
    {
        

        string levelName;

        if (sceneLoader.CurrentLevel.LevelNumber < 10)
        {
            levelName = "Level 0" + sceneLoader.CurrentLevel.LevelNumber;
        }
        else
        {
            levelName = "Level " + sceneLoader.CurrentLevel.LevelNumber;
        }

        _levelNumber.text = levelName;
        _levelName.text = sceneLoader.CurrentLevel.LevelName;
        _levelMaxGhost.text = sceneLoader.CurrentLevel.LevelMaxGhost.ToString();
    }

    public void LoadScene()
    {
        sceneLoader.CurrentLevel.LoadLevel();
    }


    
}
