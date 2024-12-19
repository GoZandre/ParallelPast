using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;

public class SleepingLumberjack : MonoBehaviour
{
    [SerializeField]
    private LumberjackState _state = LumberjackState.Sleeping;

    [SerializeField]
    private LumberjackOrientation _orientation = LumberjackOrientation.Right;

    public float Speed;

    public bool HasAxe;

    [SerializeField]
    private float _checkTreeDistance;

    [SerializeField]
    private UnityEvent _onWakeUpEvent;

    [SerializeField]
    private UnityEvent _onFallAsleep;


    private float _orientationValue = 0;
    private Rigidbody2D _rigidbody2D;


    private bool _canWalk;

    [Header("References")]

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private ParticleSystem _sleepingParticles;

    [SerializeField]
    private GameObject _exclamationParticles;

    [SerializeField]
    private Transform _exclamationSpawnTransform;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator.SetBool("HasAxe", HasAxe);
        _canWalk = true;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _onWakeUpEvent.RemoveAllListeners();

        SwitchOrientation(_orientation);

    }

    private void Update()
    {
        switch (_state)
        {
            case LumberjackState.Sleeping:
                SleepingUpdate();
                break;

            case LumberjackState.Walking:
                WalkingUpdate();
                break;
        }
    }

    private void SleepingUpdate()
    {

    }



    public void StopMovement()
    {
        _canWalk = false;
        _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
        _animator.SetBool("Motionless", true);
    }

    public void RestartMovement()
    {
        _canWalk = true;
        Debug.Log("Lumberjack can walk again");

    }


    Vector3[] worldCastCheck = { new Vector3(0, 1,0), new Vector3(0, 0,0), new Vector3(0, -1,0) };



    private void WalkingUpdate()
    {
        //Update velocity
        if(_rigidbody2D.velocity.y < -1 || _rigidbody2D.velocity.y > 1 || !_canWalk)
        {
            _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);

            _animator.SetBool("Motionless", true);
        }
        else
        {
            _animator.SetBool("Motionless", false);

            _rigidbody2D.velocity = new Vector2(_orientationValue * Speed, _rigidbody2D.velocity.y);

            //Raycast front

            RaycastHit2D CuttingRaycast = Physics2D.Raycast(transform.position, Vector3.right * _orientationValue, _checkTreeDistance, LayerMask.GetMask("LumberjackInteract"));

            if (CuttingRaycast.collider != null && HasAxe)
            {
                _state = LumberjackState.Cutting;

                CuttingTree(CuttingRaycast.collider.GetComponent<FallingTree>());
            }
            else
            {
                foreach (Vector3 check in worldCastCheck)
                {
                    RaycastHit2D WorldCollisionFront = Physics2D.Raycast(transform.position + check, Vector3.right * _orientationValue, _checkTreeDistance, LayerMask.GetMask("World", "LumberjackInteract"));

                    if (WorldCollisionFront.collider != null)
                    {

                        switch (_orientation)
                        {
                            case LumberjackOrientation.Left:
                                SwitchOrientation(LumberjackOrientation.Right);
                                break;

                            case LumberjackOrientation.Right:
                                SwitchOrientation(LumberjackOrientation.Left);
                                break;
                        }


                        return;
                    }

                }
               

            }
        }
        


       
    }

    private FallingTree _currentTree;

    public void DamageTree()
    {
        _currentTree.TakeDamage();
    }

    private void CuttingTree(FallingTree tree)
    {

        _currentTree = tree;

        _animator.SetTrigger("PunchTree");

    }

    public void WakeUp()
    {
        if(_state == LumberjackState.Sleeping)
        {
            _onWakeUpEvent?.Invoke();
            _onWakeUpEvent.RemoveAllListeners();


            SwitchOrientation(_orientation);


            _animator.SetTrigger("WakeUp");

            _sleepingParticles.Stop();
            _sleepingParticles.Clear();

            Instantiate(_exclamationParticles, _exclamationSpawnTransform);

        }
        
    }

    private void SwitchOrientation(LumberjackOrientation newOrientation)
    {

        //Set up orientation value
        switch (newOrientation)
        {
            case LumberjackOrientation.Left:

                _sleepingParticles.transform.localPosition = new Vector3(1.33f, -.6f, 0);
                _orientation = LumberjackOrientation.Left;
                _orientationValue = -1f;
                _spriteRenderer.flipX = true;
               
                break;

            case LumberjackOrientation.Right:

                _sleepingParticles.transform.localPosition = new Vector3(-1.33f, -.6f, 0);
                _orientation = LumberjackOrientation.Right;
                _orientationValue = 1f;
                _spriteRenderer.flipX = false;

                break;
        }
    }

    public void StartWalking()
    {
        //Switching State
        _state = LumberjackState.Walking;
    }

    public void Sleep()
    {
        //Switching State
        _state = LumberjackState.Sleeping;

        _onFallAsleep.Invoke();
        _onFallAsleep.RemoveAllListeners();

        _sleepingParticles.Play();
    }

    public void CollectAxe()
    {
        HasAxe = true;
        _animator.SetBool("HasAxe", HasAxe);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _orientationValue * _checkTreeDistance);

        Gizmos.color = Color.green;
        foreach (Vector3 check in worldCastCheck)
        {
            Gizmos.DrawLine(transform.position + check, (transform.position + check) + (Vector3.right * _orientationValue * _checkTreeDistance));

        }
    }

}

enum LumberjackState
{
    Sleeping,
    Walking,
    Cutting
}

enum LumberjackOrientation
{
    Right,
    Left
}


