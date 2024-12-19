using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDoorBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform _doorTransform;

    [SerializeField]
    private Transform _closePoint;
    [SerializeField]
    private Transform _openPoint;


    private Vector3 _doorObjective;

    [SerializeField]
    private float _doorInterpolation;

    [SerializeField]
    private bool _startOpen;

    private bool _doorLock;
    public bool DoorLock
    {
        get { return _doorLock; } set { _doorLock = value; }
    }

    private void Start()
    {
        _doorLock = false;

        if ( _startOpen)
        {
            _doorObjective = _openPoint.position;
        }
        else
        {
            _doorObjective = _closePoint.position;
        }
       
    }

    public void ResetDoor()
    {
        _doorObjective = _closePoint.position;
        _doorTransform.position = _doorObjective;
    }

    public void OpenDoor()
    {
        _doorObjective = _openPoint.position;

        LevelManager.Instance.LevelAudioManager.Play("OpenDoor");
    }

    public void CloseDoor()
    { 
        _doorObjective = _closePoint.position;
    }

    void Update()
    {
        if (!_doorLock)
        {
            Vector3 currentLocation = Vector3.Slerp(_doorTransform.position, _doorObjective, _doorInterpolation * Time.deltaTime);

            currentLocation = new Vector3(currentLocation.x, transform.position.y, transform.position.z);

            _doorTransform.position = currentLocation;
        }
        
    }
}
