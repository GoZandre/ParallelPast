using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class CollectiblePower : MonoBehaviour
{
    public UnityEvent OnCollectEvent;

    [Header("Datas")]
    public So_CharacterPower CharacterPower;

    [Header("References")]

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Animator _seedAnimator;

    [SerializeField]
    private ParticleSystem _collectParticle;
    [SerializeField]
    private ParticleSystemForceField _collectForceField;

    private GameObject _collectorPlayer;

    private void Awake()
    {
        _collectorPlayer = null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(_collectorPlayer == null)
        {
            if (collider.CompareTag("Player"))
            {
                OnCollectEvent.Invoke();

                //Add listener on sequence init
                LevelManager.Instance.CountdownTimer.InitSequence.AddListener(SetupPowerAsGhost);
                LevelManager.Instance.CountdownTimer.InitSequence.AddListener(collider.GetComponent<PlayerController>().OnResetPower);

                collider.GetComponent<PlayerController>().OnCreatePower(Instantiate(CharacterPower));

                _seedAnimator.SetTrigger("Collect");

                _collectParticle.Play();

                _collectForceField.transform.position = collider.transform.position;
                _collectForceField.gameObject.SetActive(true);
              
                _collectorPlayer = collider.gameObject; 

                GetComponent<CircleCollider2D>().enabled = false;

                
            }
        }
        else
        {
            if (collider.CompareTag("Player"))
            {
                if(collider.gameObject == _collectorPlayer && !LevelManager.Instance.CountdownTimer.IsFirstFrame)
                {
                    collider.GetComponent<PlayerController>().OnCreatePower(Instantiate(CharacterPower), false);
                    _seedAnimator.SetTrigger("Collect");

                    //Add listener on sequence init
                    LevelManager.Instance.CountdownTimer.InitSequence.AddListener(SetupPowerAsGhost);
                    LevelManager.Instance.CountdownTimer.InitSequence.AddListener(collider.GetComponent<PlayerController>().OnResetPower);
                }
            }
        }
    }

    private void Update()
    {
        if(_collectorPlayer != null)
        {
            _collectForceField.transform.position = _collectorPlayer.transform.position;
        }
    }

    public void SetupPowerAsGhost()
    {
        _seedAnimator.SetTrigger("Reset");
        _spriteRenderer.color = new Vector4(1,1,1,.5f);
        _spriteRenderer.GetComponent<Light2D>().enabled = false;
        _spriteRenderer.transform.GetChild(0).gameObject.SetActive(false);

        _collectParticle.gameObject.SetActive(false);
        _collectForceField.gameObject.SetActive(false);

        GetComponent<CircleCollider2D>().enabled = true;

    }
}
