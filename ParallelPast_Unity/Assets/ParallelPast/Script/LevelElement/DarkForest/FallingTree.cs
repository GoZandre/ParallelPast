using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : MonoBehaviour
{

    


    public int TreeLife;

    [Header("References")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _frontSpriteRenderer;
    [SerializeField]
    private BoxCollider2D _upperBoxCollider;

    public bool TakeDamage()
    {
        TreeLife--;

        if(TreeLife <= 0)
        {
            FallTree();

            return true;
        }
        else
        {
            return false;
        }
    }

    private void FallTree()
    {
        _upperBoxCollider.enabled = false;


        _frontSpriteRenderer.gameObject.SetActive(false);

        _animator.SetTrigger("Falling");

        LevelManager.Instance.LevelAudioManager.Play("TreeFalling");
    }
}

enum BreakableTreeState
{
    Standing,
    Cutted
}
