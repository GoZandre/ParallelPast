using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelTransition", menuName = "ScriptableObjects/Level/LevelTransition")]
public class So_LevelTransitionType : ScriptableObject
{
    [SerializeField]
    private string _transitionName;

    [Space(20)]

    [SerializeField]
    private Material _transitionMaterial;

    [SerializeField]
    private Color _transitionColor;
    [SerializeField]
    private Color _transitionTextColor;

    public string transitionName => _transitionName;
    public Material transitionMaterial => _transitionMaterial;
    public Color transitionColor => _transitionColor;
    public Color transitionTextColor => _transitionTextColor;
}
