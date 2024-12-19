using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ObjectHeight", menuName = "ScriptableObjects/Misc/ObjectHeight")]
public class So_ObjectHeight : ScriptableObject
{
    [SerializeField]
    private float _playerHeight;
    [SerializeField]
    private float _lumberjackHeight;

    public float PlayerHeight => _playerHeight;
    public float LumberjackHeight => _lumberjackHeight;
}
