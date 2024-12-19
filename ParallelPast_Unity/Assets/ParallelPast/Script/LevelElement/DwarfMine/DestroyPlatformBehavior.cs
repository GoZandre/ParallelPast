using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyPlatformBehavior : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private ParticleSystem _destroyParticle;

    [SerializeField]
    private Transform _particleSpawnTransform;

    [SerializeField]
    private GameObject _selfReference;

    [SerializeField]
    private bool _isShaking;

    private void OnEnable()
    {
        Debug.Log("Active destroy platform");
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("Restart");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReplayManager replayManager = collision.GetComponent<ReplayManager>();
            if(replayManager.CurrentReplayStat == ReplayStat.Recording)
            {
                _isShaking = true;
                BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(boxCollider.size.x * 1.02f, boxCollider.size.y);

                ShakePlatform();
            }
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _isShaking)
        {
           
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (!playerController.SwitchingCollider)
            {
                DestructPlatform();
            }            
        }
    }

    private void ShakePlatform()
    {
        LevelManager.Instance.AddActorToSpawnToSequence(transform.parent.gameObject);
        _animator.SetTrigger("IsShaking");
    }

    private void DestructPlatform()
    {
        LevelManager.Instance.LevelAudioManager.Play("BreakFloor");
        _animator.SetTrigger("IsDestroy");
    }

    public void DestroyPlatform()
    {
        
        transform.parent.gameObject.SetActive(false);
    }

}
