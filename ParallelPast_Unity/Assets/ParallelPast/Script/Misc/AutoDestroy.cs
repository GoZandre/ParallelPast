using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float _timeBeforeDestroy;

    private float _time;
    void Update()
    {
        if(_time >= _timeBeforeDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            _time += Time.deltaTime;
        }
    }
}
