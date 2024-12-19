using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerColliders : MonoBehaviour
{
    private void Start()
    {
        IgnorePlayerCollisions();
        LevelManager.Instance.CountdownTimer.InitSequence.AddListener(IgnorePlayerCollisions);
    }

    public void IgnorePlayerCollisions()
    {
        //Ignore Player Collision
        foreach (Collider2D collider1 in LevelManager.Instance.CurrentGhost.GetComponents<Collider2D>())
        {
            foreach (Collider2D collider2 in GetComponents<Collider2D>())
            {
                Physics2D.IgnoreCollision(collider1, collider2);
            }

        }

    }
}
