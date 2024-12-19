using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDoorPlayerDetector : MonoBehaviour
{
    [SerializeField]
    private HorizontalDoorBehavior _horizontalDoorBehavior;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                _horizontalDoorBehavior.DoorLock = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                _horizontalDoorBehavior.DoorLock = false;
            }
        }
    }
}
