using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider _sfxSlider;

    [SerializeField]
    private Slider _musicSlider;

    public const string MUSIC_VOLUME = "MusicVolume";
    public const string SFX_VOLUME = "SFXVolume";

    private void Start()
    {
        InitAudioController();

    }

    private void OnEnable()
    {
        InitAudioController();
    }

    private void OnDisable()
    {
        float sfxVolume;
        audioMixer.GetFloat(SFX_VOLUME, out sfxVolume);
        float musicVolume;
        audioMixer.GetFloat(MUSIC_VOLUME, out musicVolume);

        GameManager.Instance.userDataManager.SetAudioVolume(sfxVolume, musicVolume);

        Debug.Log("Save volume parameters datas");
    }

    public void InitAudioController()
    {
        /* MUSIC VOLUME*/

        //Get values
        float musicVolume;
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        audioMixer.GetFloat(MUSIC_VOLUME, out musicVolume);

        //Set up fill percent & handler position
        float musicVolumeFillPercent = Mathf.Pow(10, (musicVolume / 20f));

        _musicSlider.value = (musicVolumeFillPercent);
        //Invoke function
        _musicSlider.onValueChanged.Invoke(musicVolumeFillPercent);




        /* SFX VOLUME*/

        //Get values
        float sfxVolume;
        _sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        audioMixer.GetFloat(SFX_VOLUME, out sfxVolume);

        //Set up fill percent & handler position
        float sfxVolumeFillPercent = Mathf.Pow(10, (sfxVolume / 20f));

        _sfxSlider.value = sfxVolumeFillPercent;
        //Invoke function
        _sfxSlider.onValueChanged.Invoke(sfxVolumeFillPercent);
    }

    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(value) * 20);
    }

    public void HideMenu()
    {
        this.gameObject.SetActive(false);
    }
}
