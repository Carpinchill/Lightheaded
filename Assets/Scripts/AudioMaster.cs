using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMasater : MonoBehaviour
{
    public AudioMixer audiomixer;
    [Header("Musica")]

    public Slider MusicSlider;
    public Toggle MuteMus;
    
    [Header("SFX")]

    public Slider SFXSlider;
    public Toggle MuteSFX;
   
    [Header("SFX")]

    public Slider GenSlider;
    public Toggle MuteGen;
    // Start is called before the first frame update
    void Start()
    {

        AudioSettings();
    }

    // Update is called once per frame
  public void AudioSettings()
    {
        float musicVolume;

        audiomixer.GetFloat("MusicVolume", out musicVolume);
        MusicSlider.value = musicVolume;
        MuteMus.isOn = musicVolume > -80;

        float SFXVolume;

        audiomixer.GetFloat("SFXVolume", out SFXVolume);
        SFXSlider.value = SFXVolume;
        MuteSFX.isOn = SFXVolume > -80;
        
        float GenVolume;

        audiomixer.GetFloat("General", out GenVolume);
        GenSlider.value = GenVolume;
        MuteGen.isOn = GenVolume > -80;
    }

    public void SetMusicVolume()
    {
        float musicVolume = MusicSlider.value;

        audiomixer.SetFloat("MusicVolume", musicVolume);

        if(musicVolume <= -80f)
        {
            MuteMus.isOn = false;
        }
        else
        {
            MuteMus.isOn = true;
        }
    }
    public void SetSFXVolume()
    {
        float SFXVolume = SFXSlider.value;

        audiomixer.SetFloat("SFXVolume", SFXVolume);

        if (SFXVolume <= -80f)
        {
            MuteSFX.isOn = false;
        }
        else
        {
            MuteSFX.isOn = true;
        }
    }
    public void SetGeneralVolume()
    {
        float GenVolume = GenSlider.value;

        audiomixer.SetFloat("General", GenVolume);

        if (GenVolume <= -80f)
        {
            MuteGen.isOn = false;
        }
        else
        {
            MuteGen.isOn = true;
        }
    }

    public void MusicOnMute()
    {
        float Volume = MusicSlider.value;
        if(Volume<=-80f)
        {
            MuteMus.interactable = false;
            return;
        }
        else
        {
            MuteMus.interactable = true;
        }

        if(MuteMus.isOn==true)
        {
            audiomixer.SetFloat("MusicVolume", Volume);
        }
        else
        {
            audiomixer.SetFloat("MusicVolume", -80f);
        }
    }
    public void SFXOnMute()
    {
        float Volume = SFXSlider.value;
        if (Volume <= -80f)
        {
            MuteSFX.interactable = false;
            return;
        }
        else
        {
            MuteSFX.interactable = true;
        }

        if (MuteSFX.isOn == true)
        {
            audiomixer.SetFloat("SFXVolume", Volume);
        }
        else
        {
            audiomixer.SetFloat("SFXVolume", -80f);
        }
    }

    public void GenOnMute()
    {
        float Volume = GenSlider.value;
        if (Volume <= -80f)
        {
            MuteGen.interactable = false;
            MuteSFX.interactable = false;
            MuteMus.interactable = false;
            return;
        }
        else
        {
            MuteGen.interactable = true;
            MuteSFX.interactable = true;
            MuteMus.interactable = true;
        }

        if (MuteGen.isOn == true)
        {
            audiomixer.SetFloat("General", Volume);
            MuteSFX.interactable = true;
            MuteMus.interactable = true;
        }
        else
        {
            audiomixer.SetFloat("General", -80f);
            MuteSFX.interactable = false;
            MuteMus.interactable = false;
        }
    }
}
