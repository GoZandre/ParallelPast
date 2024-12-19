using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu_Manager : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private LevelSelectorManager _levelSelectorManager;

    [SerializeField]
    private AudioManager _audioManager;

    [SerializeField]
    private ForeGroundTransition _foreGroundTransition;

    [SerializeField]
    private GameObject _startForeGround;

    bool _canSelectSomething = true;

    [SerializeField]
    private Button _buyGameButton;

    [Header("Save Init")]
    [SerializeField]
    private SO_LevelData _firstLevel;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();

        _audioManager.PlayMusic("MenuMusic");

        if (GameManager.Instance.SaveManager.state.PremiumKey == "yHSY4+G3~)m%lP<nchj3e?0ea(B[IK)0rf&r")
        {
            _buyGameButton.gameObject.SetActive(false);

            Debug.Log("REMOVE BUY BUTTON");
        }
    }

    public void SelectPlayButton()
    {
        _playButton.Select();
    }


    public void OpenLevelMenu()
    {
        _canSelectSomething = false;

        _foreGroundTransition.gameObject.SetActive(true);
        StartCoroutine(_foreGroundTransition.CloseTransition(1f, 0.25f));        
    }

    public void LoadLevelSelector()
    {
        GameManager.Instance.LoadLevelSelectorScene();


    }

    public void DesactiveSelf()
    {
        gameObject.SetActive(false);
    }


    public void DestroyForeGround()
    {
        Destroy(_startForeGround);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
