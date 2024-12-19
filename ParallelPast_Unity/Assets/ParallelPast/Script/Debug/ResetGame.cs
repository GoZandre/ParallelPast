using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    public SO_LevelData[] LevelsData;
    public SO_LevelData LevelOneData;

    public void ResetLevels()
    {

        GameManager.Instance.SaveManager.ResetSave();
        GameManager.Instance.SaveGame();
    }

    public void UnlockAllLevel()
    {
        GameManager.Instance.SaveGame();
    }
}


