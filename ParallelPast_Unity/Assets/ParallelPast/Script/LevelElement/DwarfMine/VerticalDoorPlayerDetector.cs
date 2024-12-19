using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDoorPlayerDetector : MonoBehaviour
{
    [SerializeField]
    private VerticalDoorBehavior _verticalDoorBehavior;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                _verticalDoorBehavior.DoorLock = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                _verticalDoorBehavior.DoorLock = false;
            }
        }
    }
}
