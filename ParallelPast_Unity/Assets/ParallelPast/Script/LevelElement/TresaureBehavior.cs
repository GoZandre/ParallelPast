using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TresaureBehavior : MonoBehaviour
{
    [Header("QuestReward")]
    [SerializeField]
    private GameObject _questReward;

    [SerializeField]
    private SpriteRenderer _rewardIcon;

    [SerializeField]
    private SpriteRenderer _halo01;

    [SerializeField]
    private SpriteRenderer _halo02;

    [Header("ChestReward")]
    [SerializeField]
    private GameObject _chestReward;

    [SerializeField]
    private Sprite _openChest;

    [SerializeField]
    private ParticleSystem _winTreasureParticles;

    private void Start()
    {
        SO_LevelData levelData = LevelManager.Instance.LevelData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                _chestReward.GetComponent<SpriteRenderer>().sprite = _openChest;
                _winTreasureParticles.gameObject.SetActive(true);
                LevelManager.Instance.OnWinLevel.Invoke();
            }
            
        }
    }
    
}
