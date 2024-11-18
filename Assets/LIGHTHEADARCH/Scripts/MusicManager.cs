using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Audio")]

    [SerializeField] private AudioSource _source; 
    [SerializeField] private AudioMixer _mixer;

    [Header("UI")]

    [SerializeField] private Slider _masterSlider;
    [SerializeField] private float _initMasterVol=.5f;
   
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private float _initMusicVol = .5f;

    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private float _initSFXVol = 1f;

    private void Start()
    {
        _masterSlider.value = _initMasterVol;
        SetMasterVolume(_initMasterVol);

        _musicSlider.value = _initMusicVol;
        SetMasterVolume(_initMusicVol);

        _sfxSlider.value = _initSFXVol;
        SetMasterVolume(_initSFXVol);
    }

    public void SetMasterVolume(float value)
    {
        _mixer.SetFloat("Mastervol", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        _mixer.SetFloat("Musicvol", Mathf.Log10(value) * 20);
    }
    public void SetSFXVolume(float value)
    {
        _mixer.SetFloat("SFXvol", Mathf.Log10(value) * 20);
    }

    public void PlayAudio(AudioClip clip)
    {
        if (clip == _source.clip) return;

        _source.Stop();
        _source.clip = clip;
        _source.Play();
    }
}
