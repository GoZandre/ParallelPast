using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoLightBehavior : MonoBehaviour
{
    private LevelManager _levelManager;
    private Transform _currentCharacter;

    Material _noLightMat;

    private Image _noLightImage;

    // Start is called before the first frame update
    void Start()
    {
        _noLightImage = GetComponent<Image>();

        _levelManager = LevelManager.Instance;
        _levelManager.OnNewGhost.AddListener(SetNewGhost);
        SetNewGhost();
    }

    public void SetNewGhost()
    {
        _currentCharacter = _levelManager.CurrentGhost.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        _noLightImage.material.SetVector("_WorldPosition", _currentCharacter.transform.position);
    }
}
