using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    [SerializeField]
    private ForeGroundTransition _foreGroundTransition;
    
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        GameManager sceneLoader = FindObjectOfType<GameManager>();
        sceneLoader.SetNextLevel();

        _foreGroundTransition.gameObject.SetActive(true);
        _foreGroundTransition.SceneLoader = sceneLoader;

        StartCoroutine(_foreGroundTransition.CloseTransition(1f));
    }

    public void LoadPreviousLevel()
    {
        Time.timeScale = 1f;
        GameManager sceneLoader = FindObjectOfType<GameManager>();
        sceneLoader.SetPreviousLevel();

        _foreGroundTransition.gameObject.SetActive(true);
        _foreGroundTransition.SceneLoader = sceneLoader;

        StartCoroutine(_foreGroundTransition.CloseTransition(1f));
    }

    public void LoadSameLevel()
    {
        Time.timeScale = 1f;
        GameManager sceneLoader = FindObjectOfType<GameManager>();

        _foreGroundTransition.gameObject.SetActive(true);
        _foreGroundTransition.SceneLoader = sceneLoader;

        StartCoroutine(_foreGroundTransition.CloseTransition(1f));
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameManager sceneLoader = FindObjectOfType<GameManager>();
        sceneLoader.LoadMenu();
    }

    public void LoadLevelSelector()
    {
        _foreGroundTransition.gameObject.SetActive(true);
        Time.timeScale = 1f;

        _foreGroundTransition.OnCloseTransitionExecute.AddListener(GameManager.Instance.LoadLevelSelectorScene);
        StartCoroutine(_foreGroundTransition.CloseTransition(1f));
    }


}
