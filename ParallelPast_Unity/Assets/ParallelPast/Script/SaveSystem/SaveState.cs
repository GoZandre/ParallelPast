using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState
{
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
    public string PremiumKey;

    public bool IsFirstConnexion;

    //Language
    public Language CurrentLanguage;

}
