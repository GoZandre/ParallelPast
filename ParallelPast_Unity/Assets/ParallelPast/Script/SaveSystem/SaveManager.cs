using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;


public class SaveManager : MonoBehaviour
{
    public SaveState state;

    private UserDataManager _userDataManager;

    private string saveName = "parrallelpast_datas";

    private string premiumKey = "yHSY4+G3~)m%lP<nchj3e?0ea(B[IK)0rf&r";
    public string PremiumKey => premiumKey;

    private void Awake()
    {
        _userDataManager = GetComponent<UserDataManager>();

        Load();
    }

    public void Save()
    {

        state.MusicVolume = _userDataManager.MusicVolume;
        state.SfxVolume = _userDataManager.SfxVolume;

        state.GuiScale = _userDataManager.GuiScale;
        state.IsArrow = _userDataManager.IsArrow;

        state.PendingUnlockLevel = _userDataManager.PendingUnlockLevel;
        state.UnlockedLevel = _userDataManager.UnlockedLevel;
        state.PendingCompletedLevel = _userDataManager.PendingCompletedLevel;
        state.CompletedLevel = _userDataManager.CompletedLevel;

        state.NoGhostCompleted = _userDataManager.NoGhostCompleted;
        state.NoTimerCompleted = _userDataManager.NoTimerCompleted;
        state.NoLightCompleted = _userDataManager.NoLightCompleted;

        state.LastFinishedLevel = _userDataManager.LastFinishedLevel;

        state.PremiumKey = _userDataManager.PremiumCode;

        state.IsFirstConnexion = _userDataManager.IsFirstConnexion;

        state.CurrentLanguage = _userDataManager.CurrentLanguage;


        PlayerPrefs.SetString(saveName, Helper.Serialize<SaveState>(state));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(saveName))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString(saveName));
        }
        else
        {
            state = new SaveState();

            Save();
            Debug.Log("Create a new save state");
        }
    }

    public void ResetSave()
    {

    }
}


