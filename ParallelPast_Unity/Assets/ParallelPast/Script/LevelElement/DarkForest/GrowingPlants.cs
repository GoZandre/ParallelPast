using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

using UnityEngine.Events;

public class GrowingPlants : MonoBehaviour
{
    [Header("Animations")]

    [SerializeField]
    private Sprite[] _animationsSprites;

    [SerializeField]
    private float _growingDuration;

    [SerializeField]
    private AnimationCurve _growingCurve;

    public UnityEvent OnStartGrowing;

    public UnityEvent OnGrowingComplete;

    [Header("Parameters")]

    [SerializeField]
    private float _growingHeight;

   

    [Header("References")]

    [SerializeField]
    private Collider2D _growingCollider;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Transform _growingLight;

    [SerializeField]
    private ParticleSystem _growingParticles;


    //private values
    private bool _canGrowing;

    private bool _isGrowing;
    public bool IsGrowing => _isGrowing;

    private float _lerpValue;

    private GrowingOrientation _orientation = GrowingOrientation.Up;

    private int _spriteIndex;

    private void Awake()
    {
        //Initialize private values
        _canGrowing = true;
        _isGrowing = true;
        _orientation = GrowingOrientation.Up;
        _spriteIndex = 1;
        _spriteRenderer.sprite = _animationsSprites[_spriteIndex - 1];
        _lerpValue = 0;

        //OnStartGrowing.Invoke();
    }

    

    private void Start()
    {
        LevelManager.Instance.LevelAudioManager.Play("GrowingPlant");
        OnGrowingComplete.RemoveAllListeners();      
    }

    private void Update()
    {
        //Update Sprite

        float indexLerp = (float)_spriteIndex / (float)_animationsSprites.Length;
        float timeLerp = _lerpValue / _growingDuration + 0.025f;

        if (indexLerp < timeLerp && _orientation == GrowingOrientation.Up)
        {
            if (_spriteIndex >= _animationsSprites.Length)
            {
                //Stop param
                _canGrowing = false;
                StopGrowingParticles();

                _isGrowing = false;

                OnGrowingComplete.Invoke();
                OnGrowingComplete.RemoveAllListeners();

            }
            else
            {
                _spriteIndex++;
                _spriteRenderer.sprite = _animationsSprites[_spriteIndex - 1];
            }

        }
        else if (indexLerp > timeLerp && _orientation == GrowingOrientation.Down)
        {
            if (_spriteIndex <= 1)
            {
                //Stop param

                _isGrowing = false;

                OnGrowingComplete.Invoke();
                Destroy(gameObject);
            }
            else
            {
                _spriteIndex--;
                _spriteRenderer.sprite = _animationsSprites[_spriteIndex - 1];
            }
        }

        //Update Collider

        Vector2 upwardPosition = new Vector2(0, Mathf.Lerp(0, _growingHeight, _growingCurve.Evaluate(timeLerp)));
        _growingLight.localPosition = upwardPosition;


        //Growing conditions
        if (_orientation == GrowingOrientation.Up)
        {
            Vector2 originPosition = new Vector2(transform.position.x, transform.position.y + 0.2f);
            RaycastHit2D hit01 = Physics2D.Raycast(originPosition + upwardPosition + marginVector, transform.up, 0.2f, LayerMask.GetMask("World"));
            RaycastHit2D hit02 = Physics2D.Raycast(originPosition + upwardPosition + marginVector + Vector2.right * .5f, transform.up, 0.2f, LayerMask.GetMask("World"));
            RaycastHit2D hit03 = Physics2D.Raycast(originPosition + upwardPosition + marginVector - Vector2.right * .5f, transform.up, 0.2f, LayerMask.GetMask("World"));

            if (hit01.collider != null || hit02.collider != null || hit03.collider != null)
            {
                if(_spriteIndex > 2)
                {
                    _canGrowing = false;
                    _isGrowing = false;
                    StopGrowingParticles();

                    OnGrowingComplete.Invoke();
                    OnGrowingComplete.RemoveAllListeners();
                }
                

            }
            else
            {
                _isGrowing = true;
                _canGrowing = true;
                PlayGrowingParticles();
            }
        }



        //Update lerp value → Down / Up
        if (_lerpValue > 0 && _orientation == GrowingOrientation.Down)
        {
            _lerpValue -= Time.deltaTime;
        }
        else if (_lerpValue < _growingDuration && _orientation == GrowingOrientation.Up && _canGrowing)
        {
            _lerpValue += Time.deltaTime;
        }

    }

    private void PlayGrowingParticles()
    {
        if (!_growingParticles.isPlaying)
        {
            _growingParticles.Play();
        }
    }

    private void StopGrowingParticles()
    {
        if (_growingParticles.isPlaying)
        {
            _growingParticles.Stop();
        }
    }


    public void DisableCharacterCollision(Transform playerTransform)
    {
        //ignore current character collision
        foreach (Collider2D collider in playerTransform.GetComponents<Collider2D>())
        {
            foreach (Collider2D selfColliders in GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(collider, selfColliders);
            }

        }
    }


    public void DestroyGrowingPlant()
    {
        foreach(BoxCollider2D collider in GetComponentsInChildren<BoxCollider2D>())
        {
            collider.enabled = false;
        }

        _orientation = GrowingOrientation.Down;
        StopGrowingParticles();
    }

    //Growing plant margin height

    private List<float> margins = new List<float>();
    private Vector2 marginVector = Vector2.zero;

    private void SetGrowingPlantMargin()
    {

        float maxMargin = 0;

        foreach (float margin in margins)
        {
            if (margin > maxMargin)
            {
                maxMargin = margin;
            }
        }

        marginVector = new Vector2(0, maxMargin);
    }

    public void AddGrowingPlantMargin(float margin)
    {
        margins.Add(margin);
        SetGrowingPlantMargin();
    }

    public void RemoveGrowingPlantMargin(float margin)
    {
        margins.Remove(margin);
        SetGrowingPlantMargin();
    }


}

enum GrowingOrientation
{
    Up,
    Down
}
