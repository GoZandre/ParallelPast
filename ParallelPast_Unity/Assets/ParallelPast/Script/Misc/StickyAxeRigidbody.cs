using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyAxeRigidbody : MonoBehaviour
{



    private Rigidbody2D _rigidbody2D;
    private LumberjackAxe _lumberjackAxe;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lumberjackAxe = transform.GetChild(0).GetComponent<LumberjackAxe>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector3.zero;

            Vector2 collisionDirection = collision.contacts[0].normal;
            collisionDirection = collisionDirection.normalized;

            switch (collisionDirection)
            {
                case Vector2 v when v.Equals(Vector2.up):
                    _lumberjackAxe.HitRotation = 270;
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    _lumberjackAxe.HitRotation = 90;
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    _lumberjackAxe.HitRotation = 0;
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    _lumberjackAxe.HitRotation = 180;
                    break;
            }

        }
    }
}
