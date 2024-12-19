using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        GetComponent<SceneButtonSelector>().SelectNextButton();
    }

    public void ContinueGame()
    {
        _animator.SetTrigger("ClosePause");
    }

    public void SetTimeOn()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
