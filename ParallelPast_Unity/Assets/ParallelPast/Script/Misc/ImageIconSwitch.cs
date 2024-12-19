using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageIconSwitch : MonoBehaviour
{
    public void SetImageSprite(Sprite spriteImage)
    {
        if (spriteImage != null)
        {
            GetComponent<Image>().sprite = spriteImage;

        }
    }
}
