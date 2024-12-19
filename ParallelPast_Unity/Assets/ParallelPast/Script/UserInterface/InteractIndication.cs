using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIndication : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    public void CloseInteraction()
    {
        _animator.SetBool("Close", true);
    }

    public void DestroyInteraction()
    {
        Destroy(gameObject);
    }
}
