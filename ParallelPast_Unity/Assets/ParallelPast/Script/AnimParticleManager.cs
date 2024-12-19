using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimParticleManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private Transform _particleSpawnTransform;


    public void SpawnParticles()
    {
        ParticleSystem particles = Instantiate(_particleSystem);
        particles.transform.position = _particleSpawnTransform.position;    
    }
}
