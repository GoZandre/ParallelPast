using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainerManager : MonoBehaviour
{
    [SerializeField]
    private ForeGroundTransition _foreGroundTransition;

    private void Start()
    {
        if(FindObjectOfType<GameManager>() != null)
        {
            _foreGroundTransition.OnCloseTransitionExecute.AddListener(FindObjectOfType<GameManager>().LoadTransitionScene);
        }
        else
        {
            GameManager sceneLoader = new GameManager();
            _foreGroundTransition.OnCloseTransitionExecute.AddListener(sceneLoader.LoadTransitionScene);
        }

        StartCoroutine(_foreGroundTransition.OpenTransition(1f));
    }

    public void StartTransitionToLoadLevel()
    {
        _foreGroundTransition.gameObject.SetActive(true);
        StartCoroutine(_foreGroundTransition.CloseTransition(1f, 0f, true));
    }
}
