using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldLocker", menuName = "ScriptableObjects/Level/WorldLocker")]
public class So_WorldLocker : ScriptableObject
{
    [SerializeField]
    private bool _isOpened;

    [SerializeField]
    private List<WorldLockerConditions> _unlockConditions = new List<WorldLockerConditions>();

    public bool isOpened => _isOpened;
    public List<WorldLockerConditions> UnlockConditions => _unlockConditions;

    public bool CheckUnlockConditions()
    {
        for(int i = 0;  i < _unlockConditions.Count; i++)
        {
            if(_unlockConditions[i].IsPossessed == false) 
            {
                return false;
            }
        }

        return true;
    }

    public void UnlockReward(SO_LevelReward reward)
    {
        for(int i = 0; i < _unlockConditions.Count; i++)
        {
            if (_unlockConditions[i].LevelReward == reward)
            {
                _unlockConditions[i].UnlockReward();
            }
        }
    }

    public void Unlock()
    {
        _isOpened = true;
    }

    public void Reset()
    {
        _isOpened = false;
        foreach(WorldLockerConditions condition in _unlockConditions)
        {
            condition.Reset();
        }
    }
}



[System.Serializable]
public class WorldLockerConditions
{
    [SerializeField]
    private SO_LevelReward _levelReward;
    [SerializeField]
    private bool _isPossessed;

    public SO_LevelReward LevelReward => _levelReward;
    public bool IsPossessed => _isPossessed;

    public void UnlockReward()
    {
        _isPossessed = true;
    }

    public void Reset()
    {
        _isPossessed = false;
    }
}
