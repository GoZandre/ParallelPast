using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialParamAnimation : MonoBehaviour
{
    [Header("References")]
    public Material MaterialRef;
    public string ParameterName;

    [Header("Parameters")]
    public float AnimDuration;
    public AnimationCurve AnimCurve;

    private bool _startAnim;
    private float _animLerp;

    public bool AutoStart;
    private void Awake()
    {
        _startAnim = false;
        MaterialRef.SetFloat(ParameterName, 0f);
    }

    private void Start()
    {
        if (AutoStart)
        {
            StartMaterialAnimation();
        }
    }
    public void StartMaterialAnimation()
    {
        _animLerp = 0f;
        _startAnim = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_startAnim)
        {
            if(_animLerp >= AnimDuration)
            {
                MaterialRef.SetFloat(ParameterName, 1f);
            }
            else
            {
                MaterialRef.SetFloat(ParameterName, AnimCurve.Evaluate(_animLerp/AnimDuration));
                _animLerp += Time.fixedDeltaTime;
            }
        }
    }
}
