using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    //Custom Event
    public class PlayerControllerEvent : UnityEvent<PlayerController> { }

    [Header("INPUTS")]
    public PlayerControls inputs = null;
    float inputX;

    private Rigidbody2D _rigidBody2D;
    private ReplayManager _replayManager;
    [Header("References")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private SpriteRenderer _keySpriteRenderer;
    [SerializeField]
    private RuntimeAnimatorController _baseAnimatorController;

    public Rigidbody2D RigidBody2D { get { return _rigidBody2D; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    public Animator Animator { get { return _animator; } }

    [Header("Collisions")]
    [SerializeField]
    private BoxCollider2D _semiAirBoxCollider;

    [SerializeField]
    private BoxCollider2D _groundBoxCollider;
    [SerializeField]
    private CapsuleCollider2D _groundCapsuleCollider;

    [Header("Movement")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _speedInterpolationGround;
    [SerializeField]
    private float _speedInterpolationAir;
    [SerializeField]
    private float _keySpeed;


    [Header("Jump")]
    [SerializeField]
    private float _jumpHigh;
    [SerializeField]
    private float _jumpCheckDistance;

    [SerializeField]
    private AnimationCurve _jumpCurve;
    [SerializeField]
    private float _jumpSpeed;

    [SerializeField]
    private float _jumpCheckOffsetX;
    [SerializeField]
    private float _jumpCheckOffsetY;


    [Header("Interaction")]
    [SerializeField]
    private PlayerControllerEvent m_InteractEvent = new PlayerControllerEvent();
    public PlayerControllerEvent InteractEvent => m_InteractEvent;

    [SerializeField]
    private PlayerControllerEvent m_UninteractEvent = new PlayerControllerEvent();
    public PlayerControllerEvent UninteractEvent => m_UninteractEvent;

    [SerializeField]
    private UnityEvent _onEnterInteraction = new UnityEvent();
    public UnityEvent OnEnterInteraction => _onEnterInteraction;

    [SerializeField]
    private UnityEvent _onExitInteraction = new UnityEvent();
    public UnityEvent OnExitInteraction => _onExitInteraction;


    private bool _switchingCollider;
    public bool SwitchingCollider => _switchingCollider;

    private bool _isInteract;
    public bool IsInteract => _isInteract;

    //Inputs s
    private bool _canReceiveInput;

    private bool _canReceiveMovementInputs;
    public bool CanReceiveMovementInputs { get { return _canReceiveMovementInputs; } set { _canReceiveMovementInputs = value; } }

    //Game modifiers
    private bool _hasKey;
    public bool HasKey => _hasKey;

    private bool _canJump;
    public bool CanJump { get { return _canJump; } set { _canJump = value; } }

    private bool _isJump;
    private float _jumpLerp;
    private bool _jumpDelayed;
    private bool _delayedJumpComplete;

    [SerializeField]
    private KeyBehavior _keyBehavior;

    //Power Use
    [SerializeField]
    private So_CharacterPower _currentPower = null;

    public So_CharacterPower CurrentPower => _currentPower;

    [SerializeField]
    private UnityEvent<So_CharacterPower> _onGetPower = new UnityEvent<So_CharacterPower>();
    public UnityEvent<So_CharacterPower> OnGetPower => _onGetPower;


    private void Awake()
    {
        _hasKey = false;
        _canJump = true;
        _switchingCollider = false;
        _canReceiveMovementInputs = true;
        inputX = 0;

        _rigidBody2D = GetComponent<Rigidbody2D>();
        _replayManager = GetComponent<ReplayManager>();

        m_InteractEvent.RemoveAllListeners();
        m_UninteractEvent.RemoveAllListeners();


        SetupInput();
    }

    #region INPUTS

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Gameplay.Move.performed += Move_performed;
        inputs.Gameplay.Move.canceled += Move_canceled;

        inputs.Gameplay.Interact.performed += Interact_performed;
        inputs.Gameplay.Interact.canceled += Interact_canceled;

        inputs.Gameplay.Jump.performed += Jump_performed;
        inputs.Gameplay.Jump.canceled += Jump_canceled;
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Gameplay.Move.performed -= Move_performed;
        inputs.Gameplay.Move.canceled -= Move_canceled;

        inputs.Gameplay.Interact.performed -= Interact_performed;
        inputs.Gameplay.Interact.canceled -= Interact_canceled;

        inputs.Gameplay.Jump.performed -= Jump_performed;
        inputs.Gameplay.Jump.canceled -= Jump_canceled;
    }

    private void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //_isJump = false;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        inputs.Gameplay.Jump.performed -= Jump_performed;
        StartCoroutine(ResetJump());

        if ((IsGrounded() && _canJump) || (_jumpDelayed && !_isJump))
        {
            _isJump = true;
            _jumpLerp = 0;

            _animator.SetTrigger("Jump");

            LevelManager.Instance.LevelAudioManager.Play("Jump");
        }

    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(.2f);
        inputs.Gameplay.Jump.performed += Jump_performed;
    }
    
    private void Interact_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Uninteract(this);
        _replayManager.SetUninteract(); 
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Interact(this);
        _replayManager.SetIsInteract();

    }

    

   

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        inputX = obj.ReadValue<Vector2>().x;
        
    }
    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        inputX = 0;
    }


    private void SetupInput()
    {
        _canReceiveInput = true;
        inputs = new PlayerControls();
    }

    public void RemoveInput()
    {
        _canReceiveInput = false;
        inputs.Disable();
        inputs.Gameplay.Move.performed -= Move_performed;
        inputs.Gameplay.Move.canceled -= Move_canceled;

        inputs.Gameplay.Interact.performed -= Interact_performed;
        inputs.Gameplay.Interact.canceled -= Interact_canceled;

        inputs.Gameplay.Jump.performed -= Jump_performed;
        inputs.Gameplay.Jump.canceled -= Jump_canceled;
    }



    #endregion

    void FixedUpdate()
    {
        if (_canReceiveInput && _canReceiveMovementInputs)
        {
            Movement();
       
        }

        if (_canReceiveInput)
        {
            if (_hasKey == false)
            {

                if (_isJump & _jumpLerp < 1f)
                {
                    _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Lerp(_rigidBody2D.velocity.y, _jumpHigh, _jumpCurve.Evaluate(_jumpLerp)));

                    _jumpLerp += Time.fixedDeltaTime * _jumpSpeed;
                }
                else
                {
                    _isJump = false;
                    _jumpLerp = 0;
                }

                if(!IsGrounded() && !_isJump)
                {
                    if (!_delayedJumpComplete && !_jumpDelayed)
                    {
                        _jumpDelayed = true;
                        _delayedJumpComplete = true;
                        StartCoroutine(JumpDelay(.1f));
                    }
                }
                else
                {
                    _delayedJumpComplete = false;
                }
            }

        }


        if (IsGrounded() && _animator.GetBool("IsJumping"))
        {
            _animator.SetBool("IsJumping", false);
        }
        else if(!IsGrounded() && !_animator.GetBool("IsJumping"))
        {
            _animator.SetBool("IsJumping", true);
        }

        
    }

    
    private void Movement()
    {

        Vector2 velocity = new Vector2(inputX * _speed, _rigidBody2D.velocity.y);

        if(IsGrounded())
        {
            _rigidBody2D.velocity = Vector2.Lerp(_rigidBody2D.velocity, velocity, Time.fixedDeltaTime * _speedInterpolationGround);

            //_rigidBody2D.sharedMaterial.friction = 0.5f;
        }
        else
        {
            _rigidBody2D.velocity = Vector2.Lerp(_rigidBody2D.velocity, velocity, Time.fixedDeltaTime * _speedInterpolationAir);

            //_rigidBody2D.sharedMaterial.friction = 0f;
        }

        SetMovementAnimation(_rigidBody2D.velocity, 0.5f);
    }

    public void SetMovementAnimation(Vector3 velocity, float tolerance)
    {
        //Animation

        if (velocity.x > 0f + tolerance)
        {
            _spriteRenderer.flipX = false;
            _animator.SetBool("IsWalking", true);
        }
        else if (velocity.x < 0f - tolerance)
        {
            _spriteRenderer.flipX = true;
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        _animator.SetFloat("VerticalVelocity", velocity.y);
    }

    public bool IsGrounded()
    {
        Vector3 checkOffsetR = new Vector3(_jumpCheckOffsetX, _jumpCheckOffsetY, 0);
        Vector3 checkOffsetL = new Vector3(-_jumpCheckOffsetX, _jumpCheckOffsetY, 0);

        var floorTestR = Physics2D.Raycast(transform.position + checkOffsetR, Vector2.down, _jumpCheckDistance, LayerMask.GetMask("World", "LumberjackInteract"));
        var floorTestL = Physics2D.Raycast(transform.position + checkOffsetL, Vector2.down, _jumpCheckDistance, LayerMask.GetMask("World", "LumberjackInteract"));

        bool testR = floorTestR.collider != null && floorTestR.collider.CompareTag("Ground");
        bool testL = floorTestL.collider != null && floorTestL.collider.CompareTag("Ground");

        if(Physics2D.Raycast(transform.position + new Vector3(0,_jumpCheckOffsetY,0), Vector2.down, _jumpCheckDistance, LayerMask.GetMask("World", "LumberjackInteract")))
        {
            _switchingCollider = true;
            if (_groundCapsuleCollider.enabled == false)
            {
                _groundCapsuleCollider.enabled = true;
                _groundBoxCollider.enabled = true;
            }

            if(_semiAirBoxCollider.enabled == true)
            {
                _semiAirBoxCollider.enabled = false;
            }
            _switchingCollider = false;
        }
        else if(!_hasKey)
        {
            _switchingCollider = true;
            if (_semiAirBoxCollider.enabled == false)
            {
                _semiAirBoxCollider.enabled = true;
            }

            if (_groundCapsuleCollider.enabled == true)
            {
                _groundCapsuleCollider.enabled = false;
                _groundBoxCollider.enabled = false;
            }
            _switchingCollider = false;

        }


        return testL || testR;
    }

    public void Interact(PlayerController playerController)
    {
        if(_currentPower != null)
        {
            _currentPower.InvokePower();
        }
        else
        {
            _isInteract = true;

            m_InteractEvent.Invoke(this);
            _animator.SetBool("IsInteracting", true);
        }
        
    }

    public void Uninteract(PlayerController playerController)
    {
        if (_currentPower != null)
        {
            _currentPower.CancelPower();
        }
        else
        {
            _isInteract = false;

            m_UninteractEvent.Invoke(this);
            _animator.SetBool("IsInteracting", false);
        }
    }

    public void StartWinAnimation()
    {
        _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsWinning", true);
        _animator.SetTrigger("LaunchWin");
    }

    public void UnlockKey()
    {

        _keySpriteRenderer.gameObject.SetActive(true);
        _animator.SetTrigger("GetKey");
        _animator.SetBool("HasKey", true);
        _hasKey = true;
        _speed = _keySpeed;

        _rigidBody2D.gravityScale = 8f;

        _groundCapsuleCollider.enabled = true;
        _groundBoxCollider.enabled = true;
        _semiAirBoxCollider.enabled = false;
    }

    public void UseKey()
    {
        _keySpriteRenderer.gameObject.SetActive(false);
        _animator.SetTrigger("LeaveKey");
        _animator.SetBool("HasKey", false);
        _hasKey = false;

        _rigidBody2D.gravityScale = 4f;

        _groundCapsuleCollider.enabled = false;
        _groundBoxCollider.enabled = false;
        _semiAirBoxCollider.enabled = true;
    }

    public void RemoveKeyFromGhost()
    {
        

        if( _hasKey )
        {
            _hasKey = false;
            _keySpriteRenderer.gameObject.SetActive(false);

            _animator.SetTrigger("LeaveKey");
            _animator.SetBool("HasKey", false);

            KeyBehavior newKey = Instantiate(_keyBehavior);
            newKey.transform.position = transform.position + Vector3.up * 2;
        }
        
    }

    //Character power

    public void OnCreatePower(So_CharacterPower power, bool doTransform = true)
    {
        Debug.Log("CREATE ghost power");
        _currentPower = power;

        _currentPower.EnablePower(this, doTransform);
        _onGetPower.Invoke(power);
    }

    public void OnResetPower()
    {
        Debug.Log("Reset ghost power");
        if(_currentPower != null)
        {
            _currentPower.CancelPower();
        }
        
        _currentPower = null;
        _animator.runtimeAnimatorController = _baseAnimatorController;
        _onGetPower.RemoveAllListeners();
    }

    private IEnumerator JumpDelay(float delay)
    {

        //Debug.Log("START DELAYED JUMP");
        yield return new WaitForSeconds(delay);
        _jumpDelayed = false;

        //Debug.Log("END DELAYED JUMP");
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;


        Vector3 checkOffsetR = new Vector3(_jumpCheckOffsetX, _jumpCheckOffsetY, 0);
        Vector3 checkOffsetL = new Vector3(-_jumpCheckOffsetX, _jumpCheckOffsetY, 0);

        Gizmos.DrawLine(transform.position + checkOffsetR, (transform.position + checkOffsetR) + (Vector3.down) * _jumpCheckDistance);
        Gizmos.DrawLine(transform.position + checkOffsetL, (transform.position + checkOffsetL) + (Vector3.down) * _jumpCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(0, _jumpCheckOffsetY, 0), (transform.position + new Vector3(0, _jumpCheckOffsetY, 0)) + (Vector3.down) * _jumpCheckDistance);

        if(_currentPower != null)
        {
            Gizmos.color = Color.blue;

            for (int i = -2; i <= 2; i++)
            {
                Vector2 raycastOffset = new Vector2(Mathf.Round(transform.position.x), transform.position.y) + new Vector2(i, 0);


                Gizmos.DrawLine(raycastOffset, raycastOffset - new Vector2(0, _currentPower.DebugHeight));
            }
        }
        
    }



}
