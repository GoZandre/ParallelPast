using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class PusherBehavior : MonoBehaviour
{
    [Header("Local References")]
    [SerializeField]
    private Transform _handleTransform;
    [SerializeField]
    private Transform _gearTransform;

    [Space(5)]
    [SerializeField]
    private Transform _handleStart;
    [SerializeField]
    private Transform _handleEnd;

    [SerializeField]
    private ParticleSystem _handleDownParticle;

    [Header("Effect References")]

    [SerializeField]
    private PusherBehavior _linkedPusherBehavior;
    public PusherBehavior LinkedPusherBehavior => _linkedPusherBehavior;

    [SerializeField]
    private UnityEvent _onActiveEvent;
    public UnityEvent OnActiveEvent => _onActiveEvent;

    [Header("Parameters")]

    [SerializeField]
    private bool _startActive;

    private PusherState _currentPushState;

    [Space(20)]

    [SerializeField]
    private float _pushTime;
    [SerializeField]
    private AnimationCurve _pushCurve;

    [SerializeField]
    private float _gearRotation;

    


    private void Start()
    {
        if (_linkedPusherBehavior != null)
        {
            if (_linkedPusherBehavior.LinkedPusherBehavior != this)
            {
                Debug.LogError("The pusher " + _linkedPusherBehavior.gameObject.name + " has the wrong connected pusher with : " + this.gameObject.name);
            }
        }
        else
        {
            Debug.LogError("No Pusher linked for component " + this.gameObject.name);
        }

        if (_startActive)
        {
            _currentPushState = PusherState.Active;
            _handleTransform.position = _handleEnd.position;
        }
        else
        {         
            _currentPushState = PusherState.Unactive;
            _handleTransform.position = _handleStart.position;
        }
        
        _onActiveEvent.RemoveAllListeners();
    }

    public void StartPush()
    {
        if (_currentPushState == PusherState.Unactive)
        {
            _timer = 0;
            _currentPushState = PusherState.Push;
            _onActiveEvent.Invoke();
            _onActiveEvent.RemoveAllListeners();

            _linkedPusherBehavior.ReloadPush();
        }

    }

    public void StopPush()
    {
        _currentPushState = PusherState.Active;

        ParticleSystem newParticles = Instantiate(_handleDownParticle);
        newParticles.transform.position = transform.position;

    }

    public void ReloadPush()
    {
        _timer = 0;
        _currentPushState = PusherState.Reload;
    }

    public void ResetPush()
    {
        _currentPushState = PusherState.Unactive;
    }


    private float _timer;


    private void Update()
    {
        if (_currentPushState == PusherState.Push)
        {
            if (_timer >= _pushTime)
            {
                StopPush();
            }
            else
            {
                _timer += Time.deltaTime;

                //Handle animation
                Vector3 handlePos = Vector3.Lerp(_handleStart.position, _handleEnd.position, _pushCurve.Evaluate(_timer / _pushTime));
                _handleTransform.position = handlePos;

                Quaternion gearRot = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, 0, _gearRotation), _timer / _pushTime));
                _gearTransform.rotation = gearRot;

            }
        }
        else if (_currentPushState == PusherState.Reload)
        {
            if (_timer >= _pushTime)
            {
                ResetPush();
            }
            else
            {
                _timer += Time.deltaTime;

                //Handle animation
                Vector3 handlePos = Vector3.Lerp(_handleEnd.position, _handleStart.position, _pushCurve.Evaluate(_timer / _pushTime));
                _handleTransform.position = handlePos;

                Quaternion gearRot = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, _gearRotation), Vector3.zero, _timer / _pushTime));
                _gearTransform.rotation = gearRot;

            }
        }

    }
}

public enum PusherState
{
    Unactive,
    Push,
    Active,
    Reload
}
