using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UserDataManager : MonoBehaviour
{

    private SaveManager _saveManager;

    [Header("PARAMETERS")]

    //Parameters
    public float MusicVolume;
    public float SfxVolume;

    public float GuiScale;
    public bool IsArrow;

    //Levels
    public List<int> PendingUnlockLevel = new List<int>();
    public List<int> UnlockedLevel = new List<int>();
    public List<int> PendingCompletedLevel = new List<int>();
    public List<int> CompletedLevel = new List<int>();

    //Challenge
    public List<int> NoGhostCompleted = new List<int>();
    public List<int> NoTimerCompleted = new List<int>();
    public List<int> NoLightCompleted = new List<int>();

    public int LastFinishedLevel;

    //Premium
    public bool PremiumMode { get { return true; } }

    //public bool PremiumMode { get { return PremiumCode == "yHSY4+G3~)m%lP<nchj3e?0ea(B[IK)0rf&r"; } }

    public string PremiumCode;

    public bool IsFirstConnexion;

    [Header("ACCESSIBILITY")]

    private UnityEvent _onChangeGUISize = new UnityEvent();
    public UnityEvent OnChangeGuiSize => _onChangeGUISize;

    private UnityEvent _onChangeControlMod = new UnityEvent();
    public UnityEvent OnChangeControlMod => _onChangeControlMod;

    //langage
    public Language CurrentLanguage;

    private UnityEvent _onChangeLanguage = new UnityEvent();
    public UnityEvent OnChangeLanguage => _onChangeLanguage;

    private void Awake()
    {
        _saveManager = GetComponent<SaveManager>();      
    }

    private void Start()
    {
        MusicVolume = _saveManager.state.MusicVolume;
        SfxVolume = _saveManager.state.SfxVolume;

        GuiScale = _saveManager.state.GuiScale;
        IsArrow = _saveManager.state.IsArrow;

        PendingUnlockLevel = _saveManager.state.PendingUnlockLevel;
        UnlockedLevel = _saveManager.state.UnlockedLevel;
        PendingCompletedLevel = _saveManager.state.PendingCompletedLevel;
        CompletedLevel = _saveManager.state.CompletedLevel;

        NoGhostCompleted = _saveManager.state.NoGhostCompleted;
        NoTimerCompleted = _saveManager.state.NoTimerCompleted;
        NoLightCompleted = _saveManager.state.NoLightCompleted;

        LastFinishedLevel = _saveManager.state.LastFinishedLevel;

        PremiumCode = _saveManager.state.PremiumKey;

        IsFirstConnexion = _saveManager.state.IsFirstConnexion;

        CurrentLanguage = _saveManager.state.CurrentLanguage;

        GameManager.Instance.audioManager.InitVolume();

        if (IsFirstConnexion)
        {
            InitLanguage();
        }
        

        CheckPending();
    }

    public void InitLanguage()
    {

        switch(Application.systemLanguage)
        {
            case SystemLanguage.French:
                SetLanguage(Language.French);
                return;
                case SystemLanguage.English:
                SetLanguage(Language.English);
                return;
            case SystemLanguage.German:
                SetLanguage(Language.German);
                return;
            case SystemLanguage.Spanish:
                SetLanguage(Language.Spanish);
                return;
            case SystemLanguage.Italian:
                SetLanguage(Language.Italian);
                return;
            case SystemLanguage.Portuguese:
                SetLanguage(Language.Portugese);
                return;
        }
        SetLanguage(Language.English);

    }

    public void SetLanguage(Language lang)
    {
        CurrentLanguage = lang;
        _saveManager.Save();
        _onChangeLanguage.Invoke();
    }

    public void CheckPending()
    {
        for(int i = 0; i < CompletedLevel.Count; i++)
        {
            if (PendingCompletedLevel.Contains(CompletedLevel[i]))
            {
                PendingCompletedLevel.Remove(CompletedLevel[i]);
            }
        }

        for (int i = 0; i < UnlockedLevel.Count; i++)
        {
            if (PendingUnlockLevel.Contains(UnlockedLevel[i]))
            {
                PendingUnlockLevel.Remove(UnlockedLevel[i]);
            }
        }

        _saveManager.Save();
    }

    public void UnlockPending(int level)
    {
        PendingUnlockLevel.Add(level);
        _saveManager.Save();
    }
    public void UnlockLevel(int level)
    {
        if (UnlockedLevel.Contains(level))
        {
            return;
        }
        PendingUnlockLevel.Remove(level);
        UnlockedLevel.Add(level);
        _saveManager.Save();
    }

    public void CompleteLevel(int level, bool noGhost = false, bool noTimer = false, bool noLight = false)
    {
        PendingCompletedLevel.Add(level);

        if (noGhost) { NoGhostCompleted.Add(level); }
        if (noTimer) { NoTimerCompleted.Add(level); }
        if (noLight) { NoLightCompleted.Add(level); }

        LastFinishedLevel = level;

        _saveManager.Save();
    }
    public void CompletePending(int level)
    {
        PendingCompletedLevel.Remove(level);
        CompletedLevel.Add(level);
        LastFinishedLevel = level;
        _saveManager.Save();
    }

    public void SetAudioVolume(float sfx, float music)
    {
        SfxVolume = sfx;
        MusicVolume = music;
        _saveManager.Save();
    }

    //Parameters

    public void SetGUISize(float size)
    {
        GuiScale = size;
        _onChangeGUISize.Invoke();
    }

    public void ChangeControlMod(bool isArrow)
    {
        IsArrow = isArrow;
        _onChangeControlMod.Invoke();
    }

    public void BuyGame()
    {
        PremiumCode = _saveManager.PremiumKey;
        GetComponent<SaveManager>().Save();
    }

}
