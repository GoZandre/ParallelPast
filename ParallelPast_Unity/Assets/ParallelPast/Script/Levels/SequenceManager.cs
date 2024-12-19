using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequenceManager : MonoBehaviour
{
    public List<Sequence> Sequences = new List<Sequence>();

    public Sequence GetSequence(SO_LevelReward levelReward)
    {
        for (int i = 0; i < Sequences.Count; i++)
        {
            if (Sequences[i].RewardActivation == levelReward)
            {
                return Sequences[i];
            }
        }
        return null;
    }

    public bool HasReward(SO_LevelReward levelReward)
    {
        for(int i = 0; i < Sequences.Count; i++)
        {
            if (Sequences[i].RewardActivation == levelReward)
            {
                return true;
            }
        }
        return false;
    }

    public void DebugLogHello()
    {
        Debug.Log("Hello I'm the next sequence");
    }
}

[System.Serializable]
public class Sequence
{
    [SerializeField]
    private SO_LevelReward _rewardActivation;

    [SerializeField]
    private List<UnityEvent> _squenceEvents = new List<UnityEvent>();

    public SO_LevelReward RewardActivation => _rewardActivation;
    public List<UnityEvent> SequenceEvents => _squenceEvents;


    private int _sequenceIndex;
    public void InitSequence()
    {
        _sequenceIndex = 0;
    }

    public void ExecuteNextSequenceElem()
    {
        if(_sequenceIndex < _squenceEvents.Count)
        {
            _squenceEvents[_sequenceIndex].Invoke();
            _squenceEvents[_sequenceIndex].RemoveAllListeners();

            _sequenceIndex++;
        }       
    }
}
