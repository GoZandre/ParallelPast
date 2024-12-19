using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttachement : MonoBehaviour
{
    [SerializeField]
    private ScaffoldingBehavior _scaffoldingBehavior;

    private List<Transform> _objectAttached = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                collision.transform.parent = transform;
            }
        }

        if (collision.CompareTag("Pushable") && _scaffoldingBehavior.ActivateScafolding)
        {
            collision.transform.parent = transform;
            _objectAttached.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<ReplayManager>().CurrentReplayStat == ReplayStat.Recording)
            {
                collision.transform.parent = null;
            }
        }
    }

    public void RemoveAttachedObject()
    {
        for(int i = 0; i < _objectAttached.Count; i++)
        {
            _objectAttached[i].parent = null;
        }
    }
}
