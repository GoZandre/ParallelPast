using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer _audioMixer;

    public Sound[] Sounds;

    [System.NonSerialized]
    public const string MUSIC_VOLUME = "MusicVolume";
    [System.NonSerialized]
    public const string SFX_VOLUME = "SFXVolume";

    private void Awake()
    {
        // init 

        AudioManager[] objs = FindObjectsOfType<AudioManager>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        //
     

        if (Sounds != null)
        {
            if (Sounds.Length > 0)
            {
                for (int i = 0; i < Sounds.Length; i++)
                {
                    Sounds[i].source = gameObject.AddComponent<AudioSource>();
                    Sounds[i].source.clip = Sounds[i].clip;

                    Sounds[i].source.volume = Sounds[i].volume;
                    Sounds[i].source.pitch = Sounds[i].pitch;
                    Sounds[i].source.loop = Sounds[i].loop;
                    Sounds[i].source.outputAudioMixerGroup = Sounds[i].group;
                }
            }
        }
        
    }

    public void InitVolume()
    {
        _audioMixer.SetFloat(SFX_VOLUME, GameManager.Instance.userDataManager.SfxVolume);
        _audioMixer.SetFloat(MUSIC_VOLUME, GameManager.Instance.userDataManager.MusicVolume);
    }


    public void Play(string name)
    {
        if (Sounds != null)
        {
            Sound s = Array.Find(Sounds, sound => sound.name == name);
            if (s == null)
            {
                return;
            }
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        if (Sounds != null)
        {
            Sound s = Array.Find(Sounds, sound => sound.name == name);
            if (s == null)
            {
                return;
            }
            s.source.Stop();
        }
    }

    //Music manager

    private string _currentMusic;
    public string CurrentMusic => _currentMusic;

    public void PlayMusic(string musicName)
    {
        if(musicName != _currentMusic)
        {
            if(_currentMusic != null)
            {
                Stop(_currentMusic);
            }
            
            _currentMusic = musicName;
            Play(musicName);
        }
    }

    //Ambient manager

    private string _currentAmbient;
    public string CurrentAmbient => _currentAmbient;

    public void PlayAmbientSound(string ambientSoundName)
    {
        if (ambientSoundName != _currentAmbient)
        {
            if (_currentAmbient != null)
            {
                Stop(_currentAmbient);
            }

            _currentAmbient = ambientSoundName;
            Play(ambientSoundName);
        }
    }


}
