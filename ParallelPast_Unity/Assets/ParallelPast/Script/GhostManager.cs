using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GhostManager : MonoBehaviour
{

    private GameManager gameManager;

    //References
    private ReplayManager _replayManager;
    private PlayerController _playerController;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D[] _boxColliders;
    private CapsuleCollider2D _capsuleCollider;
    private Rigidbody2D _body2D;

    [SerializeField]
    private ScarfRenderer _scarfPrefab;

    public ScarfRenderer ScarfRenderer => _scarfPrefab;

    public PlayerController attachController => _playerController;

    //values

    [SerializeField]
    private Color _ghostColor;


    private UnityEvent _onSetupGhost = new UnityEvent();
    public UnityEvent OnSetupGhost => _onSetupGhost;

    private void Awake()
    {
        _replayManager = GetComponent<ReplayManager>();
        _playerController = GetComponent<PlayerController>();
        _boxColliders = GetComponents<BoxCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _body2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;


        _scarfPrefab = Instantiate(_scarfPrefab, _spriteRenderer.transform);
        _scarfPrefab.PlayerTransform = transform;

    }

    public void SetUpAsGhost()
    {

        transform.parent = null;

        if(gameManager.SceneModifiers.NoGhostChallenge)
        {
            _spriteRenderer.color = _ghostColor * 0;
        }
        else
        {
            _spriteRenderer.color = _ghostColor;
        }
        

        _spriteRenderer.sortingOrder = -1;

        _playerController.RemoveKeyFromGhost();

        _playerController.Uninteract(_playerController);

        _playerController.RemoveInput();
        _playerController.UninteractEvent.Invoke(_playerController);

        _playerController.OnEnterInteraction.RemoveAllListeners();
        _playerController.OnExitInteraction.Invoke();
        _playerController.OnExitInteraction.RemoveAllListeners();

        _replayManager.StartReplay();

        for (int i = 0; i < _boxColliders.Length; i++)
        {
            if (_boxColliders[i] != null)
            {
                _boxColliders[i].isTrigger = true;
                _boxColliders[i].size = _boxColliders[i].size * 1.5f;
            }
        }


        _capsuleCollider.isTrigger = true;
        _capsuleCollider.size = _capsuleCollider.size * 1.5f;

        _body2D.gravityScale = 0;
        _body2D.isKinematic = true;
        _body2D.velocity = Vector3.zero;

        _scarfPrefab.gameObject.SetActive(false);

        foreach (CapsuleCollider2D capsuleCollider in GetComponents<CapsuleCollider2D>())
        {
            capsuleCollider.enabled = false;
        }


        _onSetupGhost.Invoke();
        _onSetupGhost.RemoveAllListeners();
    }

    public void ResetGhostReplay()
    {

        _playerController.Uninteract(_playerController);
        _replayManager.StopReplaying();

    }
}
