using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LockerRewardAnim : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image _lockerRewardMainImage;
    private Material _lockerRewardMaterial;

    [SerializeField]
    private CanvasGroup _lockerRewardBackground;

    [Header("FadeParameters")]
    public float FadeDuration = 0.5f;

    public AnimationCurve FadeCurve;

    public float BackgroundOpacity;

    private void Awake()
    {
        _lockerRewardMaterial = _lockerRewardMainImage.material;
    }

    private void Start()
    {
        _lockerRewardMaterial.SetFloat("_RevealValue", 0);
        _lockerRewardMaterial.SetInt("_IsInvert", 0);

        _lockerRewardBackground.alpha = 0f;
    }

    public void ShowLockerReward()
    {
        _lockerRewardMaterial.SetInt("_IsInvert", 0);
        _lockerRewardMaterial.SetFloat("_RevealValue", 0);
        StartCoroutine(LockerRewardFade());

        _lockerRewardBackground.LeanAlpha(BackgroundOpacity, FadeDuration / 2);
    }

    public void HideLockerReward()
    {
        _lockerRewardMaterial.SetInt("_IsInvert", 1);
        _lockerRewardMaterial.SetFloat("_RevealValue", 0);
        StartCoroutine(LockerRewardFade());

        _lockerRewardBackground.LeanAlpha(0f, FadeDuration / 2);
    }

    public IEnumerator LockerRewardFade(float duration = 0)
    {
        yield return null;

        if(duration >= FadeDuration)
        {
            _lockerRewardMaterial.SetFloat("_RevealValue", 1);
            WorldMapManager.Instance.CurrentRewardSequence.ExecuteNextSequenceElem();
        }
        else
        {
            _lockerRewardMaterial.SetFloat("_RevealValue", FadeCurve.Evaluate(duration / FadeDuration));
            StartCoroutine(LockerRewardFade(duration + Time.deltaTime));
        }
    }


}
