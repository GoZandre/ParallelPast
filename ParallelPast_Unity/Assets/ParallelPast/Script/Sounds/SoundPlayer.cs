using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.audioManager != null)
            {
                GameManager.Instance.audioManager.Play(soundName);
            }
           
        }
        else if(LevelManager.Instance != null)
        {
            LevelManager.Instance.LevelAudioManager.Play(soundName);
        }
        

    }
}
