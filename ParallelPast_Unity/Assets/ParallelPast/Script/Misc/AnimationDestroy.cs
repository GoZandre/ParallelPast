using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroy : MonoBehaviour
{

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DestroyParentObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
