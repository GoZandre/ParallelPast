using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ScaffoldingBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform _scaffoldingTransform;

    [SerializeField]
    private Transform _closePoint;
    [SerializeField]
    private Transform _openPoint;

    [SerializeField]
    private ObjectAttachement _objectAttachement;

    private Vector3 _scaffoldingOrigin;
    private Vector3 _scaffoldingObjective;

    [SerializeField]
    private BoxCollider2D _colliderMovement;

    [Header("Parameters")]

    [SerializeField]
    private AnimationCurve _activeMovementCurve;
    [SerializeField]
    private AnimationCurve _desactiveMovementCurve;

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private MovementAxe _movementAxe;

    [SerializeField]
    private bool _activateScafolding = false;

    public bool ActivateScafolding => _activateScafolding;

    [SerializeField]
    private float _delayBeforeActivation;

    [SerializeField]
    private float _delayBeforeUnactivation;

    [SerializeField]
    private bool _collisionEnableOnRetracted = true;

    [Header("Collsions")]
    [SerializeField]
    private float _activateColliderYScale;
    [SerializeField]
    private float _deactivateColliderYScale;

    [SerializeField]
    private float _detectionYOffset;

    private void Start()
    {
        _scaffoldingObjective = _openPoint.position;
        _scaffoldingOrigin = _closePoint.position;

        if (_activateScafolding)
        {
            _lerpValue = 1f;
            _scaffoldingTransform.position = _scaffoldingObjective;
        }
        else
        {
            _lerpValue = 0f;
            _scaffoldingTransform.position = _scaffoldingOrigin;
        }

        if (!_collisionEnableOnRetracted)
        {
            float YSize = Mathf.Lerp(0.5f, _activateColliderYScale, _activeMovementCurve.Evaluate(_lerpValue));
            _colliderMovement.size = new Vector2(_colliderMovement.size.x, YSize);
            _colliderMovement.offset = new Vector2(_colliderMovement.offset.x, (YSize * -1f) - 0.1f);
        }
    }

    public void ActiveScaffolding()
    {
        StopAllCoroutines();
        if (_delayBeforeActivation != 0)
        {
            StartCoroutine(DelayBeforeAnim(true));
        }
        else
        {
            StartAnimation(true);
        }

    }

    public void DesactiveScaffolding()
    {
        _objectAttachement.RemoveAttachedObject();

        StopAllCoroutines();
        if (_delayBeforeUnactivation != 0)
        {
            StartCoroutine(DelayBeforeAnim(false));
        }
        else
        {
            StartAnimation(false);
        }
    }

    private IEnumerator DelayBeforeAnim(bool way)
    {
        if (way)
        {
            yield return new WaitForSeconds(_delayBeforeActivation);
        }
        else
        {
            yield return new WaitForSeconds(_delayBeforeUnactivation);
        }

        StartAnimation(way);
    }

    public void StartAnimation(bool way)
    {
        if (way)
        {
            LevelManager.Instance.LevelAudioManager.Play("Elevator");

            SetColliderSizeActivation(true);
            _activateScafolding = true;
        }
        else
        {
            SetColliderSizeActivation(false);
            _activateScafolding = false;
        }

    }

    public void SetColliderSizeActivation(bool active)
    {
        if (_collisionEnableOnRetracted)
        {
            if (active)
            {
                _colliderMovement.size = new Vector2(_colliderMovement.size.x, _activateColliderYScale);
            }
            else
            {
                _colliderMovement.size = new Vector2(_colliderMovement.size.x, _deactivateColliderYScale);
            }
        }
        
    }

    private float _lerpValue;

    void Update()
    {
        if (_activateScafolding)
        {
            if (_lerpValue < 1f)
            {
                bool RaycastCenter = Physics2D.Raycast(_scaffoldingTransform.position, (_scaffoldingObjective - transform.position), 0.05f, LayerMask.GetMask("World")) == false;
                bool RaycastLeft;
                bool RaycastRight;

                if (_movementAxe == MovementAxe.Vertical)
                {
                    RaycastLeft = Physics2D.Raycast(_scaffoldingTransform.position + new Vector3(_detectionYOffset, 0, 0), (_scaffoldingObjective - transform.position) + new Vector3(_detectionYOffset, 0, 0), 0.05f, LayerMask.GetMask("World")) == false;
                    RaycastRight = Physics2D.Raycast(_scaffoldingTransform.position - new Vector3(_detectionYOffset, 0, 0), (_scaffoldingObjective - transform.position) - new Vector3(_detectionYOffset, 0, 0), 0.05f, LayerMask.GetMask("World")) == false;

                }
                else
                {
                    RaycastLeft = Physics2D.Raycast(_scaffoldingTransform.position + new Vector3(0, _detectionYOffset, 0), (_scaffoldingObjective - transform.position) + new Vector3(_detectionYOffset, 0, 0), 0.05f, LayerMask.GetMask("World")) == false;
                    RaycastRight = Physics2D.Raycast(_scaffoldingTransform.position - new Vector3(0, _detectionYOffset, 0), (_scaffoldingObjective - transform.position) - new Vector3(_detectionYOffset, 0, 0), 0.05f, LayerMask.GetMask("World")) == false;
                }

                if (RaycastCenter && RaycastLeft && RaycastRight)
                {
                    Vector3 currentLocation = Vector3.Lerp(_scaffoldingOrigin, _scaffoldingObjective, _activeMovementCurve.Evaluate(_lerpValue));
                    float YSize = Mathf.Lerp(0.5f, _activateColliderYScale, _activeMovementCurve.Evaluate(_lerpValue));

                    if (_movementAxe == MovementAxe.Vertical)
                    {
                        currentLocation = new Vector3(transform.position.x, currentLocation.y, transform.position.z);
                    }
                    else
                    {
                        currentLocation = new Vector3(currentLocation.x, transform.position.y, transform.position.z);
                    }

                    _lerpValue += _movementSpeed * Time.deltaTime;
                    _scaffoldingTransform.position = currentLocation;

                    if (!_collisionEnableOnRetracted)
                    {
                        _colliderMovement.size = new Vector2(_colliderMovement.size.x, YSize);
                        _colliderMovement.offset = new Vector2(_colliderMovement.offset.x, (YSize * -.5f) - 0.1f);
                    }
                }
            }
            else
            {
                _lerpValue = 1f;
            }

        }
        else
        {
            if (_lerpValue > 0f)
            {
                Vector3 currentLocation = Vector3.Lerp(_scaffoldingOrigin, _scaffoldingObjective, _desactiveMovementCurve.Evaluate(_lerpValue));
                float YSize = Mathf.Lerp(0.5f, _activateColliderYScale, _activeMovementCurve.Evaluate(_lerpValue));

                if (_movementAxe == MovementAxe.Vertical)
                {
                    currentLocation = new Vector3(transform.position.x, currentLocation.y, transform.position.z);
                }
                else
                {
                    currentLocation = new Vector3(currentLocation.x, transform.position.y, transform.position.z);
                }

                _lerpValue -= _movementSpeed * Time.deltaTime;
                _scaffoldingTransform.position = currentLocation;

                if (!_collisionEnableOnRetracted)
                {
                    _colliderMovement.size = new Vector2(_colliderMovement.size.x, YSize);
                    _colliderMovement.offset = new Vector2(_colliderMovement.offset.x, (YSize * -.5f) - 0.1f);
                }
            }
            else
            {
                _lerpValue = 0f;
            }
        }

    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(_closePoint.position, _openPoint.position);
    }

}

public enum MovementAxe
{
    Horizontal,
    Vertical
}

