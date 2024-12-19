using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidGravity : MonoBehaviour
{
    
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(_rigidbody2D.velocity.y < 0.25f)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }
    }
}
