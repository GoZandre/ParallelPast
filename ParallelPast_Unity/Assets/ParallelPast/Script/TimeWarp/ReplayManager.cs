using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReplayManager : MonoBehaviour
{
    PlayerController controller;

    [SerializeField]
    private ReplayStat _currentReplayStat;
    public ReplayStat CurrentReplayStat => _currentReplayStat;

    private MemoryStream _memoryStreamTransform = null;
    private BinaryWriter _binaryWriterTransform = null;
    private BinaryReader _binaryReaderTransform = null;

    private MemoryStream _memoryStreamInteraction = null;
    private BinaryWriter _binaryWriterInteraction = null;
    private BinaryReader _binaryReaderInteraction = null;

    private bool _recordingInitialized = false;

    //Recording values

    private bool _isInteract;
    private bool _isUninteract;


    public void SetIsInteract()
    {
        _isInteract = true;
    }
    public void SetUninteract()
    {
        _isUninteract = true;
    }
    private void ResetInteract()
    {
        _isInteract = false;
        _isUninteract = false;
    }


    float _lastXPosition;
    float _lastYPosition;

    public void StartReplay()
    {
        _lastXPosition = transform.position.x;
        _lastYPosition = transform.position.y;


        _currentReplayStat = ReplayStat.Replaying;
    }

    private void Start()
    {

        controller = GetComponent<PlayerController>();

        if (_currentReplayStat == ReplayStat.Recording)
        {
            StartRecording();
        }
        if (_currentReplayStat == ReplayStat.Replaying)
        {
            StartReplaying();
        }

        ResetInteract();

    }



    private void FixedUpdate()
    {
        if (_currentReplayStat == ReplayStat.Recording)
        {
            UpdateRecording();
        }
        else if (_currentReplayStat == ReplayStat.Replaying)
        {
            UpdateReplaying();
            CalculateVelocity();
        }
    }



    private void CalculateVelocity()
    {
        float velocityX = transform.position.x - _lastXPosition;
        float velocityY = transform.position.y - _lastYPosition;

        Vector3 velocity = new Vector3(velocityX, velocityY, 0);


        _lastXPosition = transform.position.x;
        _lastYPosition = transform.position.y;

        controller.SetMovementAnimation(velocity, 0.05f);
    }


    //Recording & Replaying system

    private void InitializeRecording()
    {
        _memoryStreamTransform = new MemoryStream();
        _binaryWriterTransform = new BinaryWriter(_memoryStreamTransform);
        _binaryReaderTransform = new BinaryReader(_memoryStreamTransform);


        _memoryStreamInteraction = new MemoryStream();
        _binaryWriterInteraction = new BinaryWriter(_memoryStreamInteraction);
        _binaryReaderInteraction = new BinaryReader(_memoryStreamInteraction);

        _recordingInitialized = true;
    }

    private void ResetReplayFrame()
    {
        _memoryStreamTransform.Seek(0, SeekOrigin.Begin);
        _binaryWriterTransform.Seek(0, SeekOrigin.Begin);

        _memoryStreamInteraction.Seek(0, SeekOrigin.Begin);
        _binaryWriterInteraction.Seek(0, SeekOrigin.Begin);
    }

    public void StartRecording()
    {
        if (!_recordingInitialized)
        {
            InitializeRecording();
        }
        else
        {
            _memoryStreamTransform.SetLength(0);
        }
    }

    private void StartReplaying()
    {
        ResetReplayFrame();
    }

    public void StopReplaying()
    {
        ResetReplayFrame();
    }

    private void UpdateRecording()
    {
        SaveTransform(transform);
        SaveInteraction();
    }

    private void UpdateReplaying()
    {

        bool transformEnd = _memoryStreamTransform.Position >= _memoryStreamTransform.Length;
        bool interactionEnd = _memoryStreamInteraction.Position >= _memoryStreamInteraction.Length;

        if (transformEnd && interactionEnd)
        {
            StopReplaying();
            return;
        }
        if (!transformEnd)
        {
            LoadTransform(transform);
        }
        if (!interactionEnd)
        {
            LoadInteraction();
        }

    }


    private void SaveTransform(Transform transform)
    {
        _binaryWriterTransform.Write(transform.position.x);
        _binaryWriterTransform.Write(transform.position.y);

        //Debug.Log("Write position at " + transform.position.x + ";" + transform.position.y);
    }

    private void SaveInteraction()
    {
        float isInteract;

        if (_isInteract)
        {
            isInteract = 1f;
        }
        else
        {
            isInteract = 0f;
        }

        float isUninteract;

        if (_isUninteract)
        {
            isUninteract = 1f;
        }
        else
        {
            isUninteract = 0f;
        }

        _binaryWriterInteraction.Write(isInteract);
        _binaryWriterInteraction.Write(isUninteract);

        //Reset values

        if (_isInteract || _isUninteract)
        {
            ResetInteract();
        }

        //Debug.Log("Write position at " + transform.position.x + ";" + transform.position.y);
    }

    private void LoadTransform(Transform transform)
    {
        //Read values

        float x = _binaryReaderTransform.ReadSingle();
        float y = _binaryReaderTransform.ReadSingle();


        //Setup values

        transform.position = new Vector2(x, y);


    }
    private void LoadInteraction()
    {

        float isInteract = _binaryReaderInteraction.ReadSingle();
        float isUninteract = _binaryReaderInteraction.ReadSingle();


        //Setup values


        if (isInteract != 0)
        {
            controller.Interact(controller);
        }
        else if (isUninteract != 0)
        {
            controller.Uninteract(controller);
        }

    }

}

public enum ReplayStat
{
    Recording,
    Replaying,
    Stop
}
