using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionPopUp : MonoBehaviour
{
    [SerializeField]
    private Transform _objectToFollow = null;

    public Vector2 Offset;
    public float InterpSpeed = 10f;

    private RectTransform _rectTransform;
    private RectTransform _canvas;
    private Animator _animator;
    private Camera _camera;

    public void SetObjectToFollow(Transform objectToFollow)
    {
        _objectToFollow = objectToFollow;
    }

    private void Awake()
    {
        _camera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        _canvas = transform.parent.GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*
        if(_objectToFollow != null)
        {
            Vector2 viewportPos = _camera.WorldToViewportPoint(_objectToFollow.position);

            Vector2 canvasPos = new Vector2(
            ((viewportPos.x * _canvas.sizeDelta.x)),
            ((viewportPos.y * _canvas.sizeDelta.y) - (_canvas.sizeDelta.y)));

            _rectTransform.anchoredPosition = canvasPos + Offset;
        }
        */
    }

    public void Close()
    {
        _animator.SetTrigger("FadeOut");
    }

    public void DestroyActorOnFinishAnim()
    {
        Destroy(gameObject);
    }
}
