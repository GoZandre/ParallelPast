using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonManager : MonoBehaviour
{
    
    public void UnselectAll()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
