using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHider : MonoBehaviour
{
    public void HideMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void HideParent()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
}
