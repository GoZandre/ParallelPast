using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    private float _timerDuration;

    private float _time;

    [SerializeField]
    private bool _activeCountdown = false;


    [SerializeField]
    private UnityEvent m_OnTimerEnd;
    [SerializeField]
    private UnityEvent m_InitSequence;

    public UnityEvent OnTimerEnd => m_OnTimerEnd;
    public UnityEvent InitSequence => m_InitSequence;

    [Header("UserInterface")]
    [SerializeField]
    private TextMeshProUGUI _cooldownText;

    [SerializeField]
    private Image _cooldownImage;

    private Material _cooldownMaterial;


    public bool IsFirstFrame { get { return _time == _timerDuration; } }

    private void Start()
    {
        _cooldownMaterial = _cooldownImage.material;
        _cooldownMaterial.SetFloat("_SegmentCount", (int)_timerDuration);

        ResetTimer();
    }

    private void Awake()
    {
        m_OnTimerEnd.RemoveAllListeners();
        m_OnTimerEnd.AddListener(OnTimerEvent);
        m_InitSequence.RemoveAllListeners();
    }

    public void ResetTimer()
    {
        _time = _timerDuration;
    }

    public void StopCountDown()
    {
        _activeCountdown = false;
    }

    public void StartCountDown()
    {
        _activeCountdown = true;
    }


    private void OnTimerEvent()
    {

    }

    private void Update()
    {
        if (_time <= 0f)
        {
            m_OnTimerEnd.Invoke();
            m_InitSequence.Invoke();
            ResetTimer();
        }
        else if(_activeCountdown)
        {
            _time -= Time.deltaTime;
            
        }

        _cooldownMaterial.SetFloat("_FillPercent", Mathf.Clamp(_time / _timerDuration, 0, 1));
        _cooldownText.text = ((int)_time).ToString();
        //_cooldownMsText.text = ((int)((_time - (int)_time)*100)).ToString();


    }
}
