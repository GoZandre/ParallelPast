using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvasiveRoot : MonoBehaviour
{
    public UnityEvent OnGrowingRoot;

    [Header("Parameters")]
    [SerializeField]
    private float _lerpTime;
    [SerializeField]
    private AnimationCurve _lerpCurve;

    [Space(5)]

    [SerializeField]
    private int _turnToGrowth;
    [SerializeField]
    private int _currentTurn = 0;
    [SerializeField]
    private int _stopingTurn;

    [Space(5)]

    [SerializeField]
    private float _additionalColliderSize;

    [Header("References")]
    [SerializeField]
    private Transform _rootToGrowth;

    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Transform _endPoint;

    private BoxCollider2D _boxCollider;



    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        LevelManager.Instance.CountdownTimer.OnTimerEnd.AddListener(GrowingRoot);
        _rootToGrowth.localPosition = Vector3.Lerp(_startPoint.localPosition, _endPoint.localPosition, (float)_currentTurn / (float)_turnToGrowth); ;
        SetColliderSize();
    }


    private void GrowingRoot()
    {
        if(_stopingTurn == 0)
        {
            if (_currentTurn < _turnToGrowth)
            {
                Vector3 currentPos = Vector3.Lerp(_startPoint.localPosition, _endPoint.localPosition, (float)_currentTurn / (float)_turnToGrowth);

                _currentTurn++;

                Vector3 nextPos = Vector3.Lerp(_startPoint.localPosition, _endPoint.localPosition, (float)_currentTurn / (float)_turnToGrowth);


                //SetColliderSize();

                OnGrowingRoot.Invoke();
                StartCoroutine(RootGrowing(currentPos, nextPos));
            }
        }
        else if (_currentTurn < _stopingTurn)
        {
            if (_currentTurn < _turnToGrowth)
            {
                Vector3 currentPos = Vector3.Lerp(_startPoint.localPosition, _endPoint.localPosition, (float)_currentTurn / (float)_turnToGrowth);

                _currentTurn++;

                Vector3 nextPos = Vector3.Lerp(_startPoint.localPosition, _endPoint.localPosition, (float)_currentTurn / (float)_turnToGrowth);


                //SetColliderSize();
                OnGrowingRoot.Invoke();
                StartCoroutine(RootGrowing(currentPos, nextPos));
            }
        }
        

    }

    private Vector2 _previousSize;

    private void SetColliderSize()
    {
        //Set collider
        _boxCollider.size = new Vector2(2, (6 * (float)_currentTurn / (float)_turnToGrowth) + _additionalColliderSize) ;
        _boxCollider.offset = new Vector2(0, _boxCollider.size.y / 2);

        _previousSize = _boxCollider.size;
    }

    private IEnumerator RootGrowing(Vector3 startPos, Vector3 endPos, float lerpValue = 0)
    {

        yield return null;



        if (lerpValue < _lerpTime)
        {
            RaycastHit2D collisionTestCast = Physics2D.Raycast(_rootToGrowth.position + _rootToGrowth.up * 6f, _rootToGrowth.up, .5f, LayerMask.GetMask("Default"));

            if (!collisionTestCast)
            {
                _rootToGrowth.localPosition = Vector3.Lerp(startPos, endPos, _lerpCurve.Evaluate(lerpValue / _lerpTime));

                //Set collider
                _boxCollider.size = Vector2.Lerp(_previousSize, new Vector2(2, (6 * (float)_currentTurn / (float)_turnToGrowth) + _additionalColliderSize), _lerpCurve.Evaluate(lerpValue / _lerpTime));
                _boxCollider.offset = new Vector2(0, _boxCollider.size.y / 2);

                StartCoroutine(RootGrowing(startPos, endPos, lerpValue + Time.deltaTime));
            }
            else
            {
                StartCoroutine(RootGrowing(startPos, endPos, lerpValue));
            }

        }

        else if (_currentTurn < _turnToGrowth)
        {
            SetColliderSize();
            LevelManager.Instance.CountdownTimer.OnTimerEnd.AddListener(GrowingRoot);

        }
    }


}
