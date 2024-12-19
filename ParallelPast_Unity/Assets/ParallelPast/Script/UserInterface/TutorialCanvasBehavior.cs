using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvasBehavior : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Continue()
    {
        _animator.SetTrigger("FadeOut");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
