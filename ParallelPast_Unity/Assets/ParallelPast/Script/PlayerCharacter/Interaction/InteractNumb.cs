using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractNumb : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite[] _numbSprites;

    public void SetNumber(int numb)
    {
        if(numb > 0 && numb < 10)
        {
            _spriteRenderer.sprite = _numbSprites[numb-1];
        }   
    }
}
