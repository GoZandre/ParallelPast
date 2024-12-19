using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelReward", menuName = "ScriptableObjects/Level/LevelReward")]
public class SO_LevelReward : ScriptableObject
{
    [SerializeField]
    private string _rewardName;

    [SerializeField]
    private Sprite _levelRewardSprite;

    [SerializeField]
    private Color _colorA;

    [SerializeField]
    private Color _colorB;


    public string RewardName => _rewardName;
    public Sprite levelRewardSprite => _levelRewardSprite;
    public Color colorA => _colorA;
    public Color colorB => _colorB;
}
