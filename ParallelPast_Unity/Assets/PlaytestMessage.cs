using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestMessage : MonoBehaviour
{
    private Canvas canvas;
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
