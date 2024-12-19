using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _winTreasureParticles;

    [SerializeField]
    private Vector2 _yPosAnimation;
    [SerializeField]
    private AnimationCurve _animCurve;
    [SerializeField]
    private float _animDuration;
    [SerializeField]
    private Transform _keySpriteTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                LevelManager.Instance.LevelAudioManager.Play("GetKey");

                ParticleSystem collectParticles = Instantiate(_winTreasureParticles);
                collectParticles.transform.position = transform.position;
                collision.GetComponent<PlayerController>().UnlockKey();

                LevelManager.Instance.AddActorToSpawnToSequence(gameObject);
                gameObject.SetActive(false);
            }          
        }
    }

    private float time = 0;
    private void Update()
    {
        if(time > _animDuration)
        {
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
            _keySpriteTransform.localPosition = Vector3.Lerp(Vector3.up * _yPosAnimation.x, Vector3.up * _yPosAnimation.y, _animCurve.Evaluate(time/_animDuration));
        }
    }
}
