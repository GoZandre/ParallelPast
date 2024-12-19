using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarfRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform[]_scarfPoints;

    [SerializeField]
    private ParticleSystem _scarfParticles;

    private LineRenderer _lineRenderer;

    private Transform _playerTransform;
    public Transform PlayerTransform
    {
        set { _playerTransform = value; }
    }

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _scarfPoints.Length;
    }

  
    void Update()
    {
        for(int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _lineRenderer.SetPosition(i, _scarfPoints[i].position);
        }

        
    }

    public void SwitchScarfAppearance(So_CharacterPower newPower)
    {
        _lineRenderer.materials[0].SetTexture("_Texture", newPower.ScarfSprite);
        _scarfParticles.startColor = newPower.ScarfColorA;
    }
}
