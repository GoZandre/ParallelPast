using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField]
    private GhostManager _characterPrefab;

    [SerializeField]
    private Transform _spawnerTransform;

    private bool _canSpawn;

    private void Start()
    {
        _canSpawn = true;
    }

    public void StopSpawn()
    {
        _canSpawn = false;
    }

    public void CreateNewCharacter()
    {
        if(_canSpawn)
        {
            GhostManager newCharacter = Instantiate(_characterPrefab);
            newCharacter.transform.position = _spawnerTransform.position;

            LevelManager.Instance.TestloseConditions(newCharacter);
        }
    }
}
