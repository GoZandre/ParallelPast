using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneModifiers : MonoBehaviour
{
    
    private GameManager gameManager;

    [Header("PARAMETERS")]
    [SerializeField]
    private bool _noGhostChallenge;
    [SerializeField]
    private bool _noLightChallenge;
    [SerializeField]
    private bool _noTimerChallenge;

    [Space(10)]
    [SerializeField]
    private bool _openTutorial;

    public bool NoGhostChallenge => _noGhostChallenge;
    public bool NoLightChallenge => _noLightChallenge;
    public bool NoTimerChallenge => _noTimerChallenge;

    public bool OpenTutorial {get{return _openTutorial;} set { _openTutorial = value; } }

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void SetNoGhostChallegne(bool value)
    {
        _noGhostChallenge = value;
    }

    public void SetNoLightChallenge(bool value)
    {
        _noLightChallenge = value;
    }

    public void SetNoTimerChallenge(bool value)
    {
        _noTimerChallenge = value;
    }

    public void ResetChallenge()
    {
        _noGhostChallenge = false;
        _noLightChallenge = false;
        _noTimerChallenge = false;
    }
}
