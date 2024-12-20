using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public AudioClip menuMusic; 
    public float volume = 0.5f; 

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = menuMusic;
        _audioSource.volume = volume;
        _audioSource.loop = true; 
        _audioSource.playOnAwake = false;

        PlayMusic();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "ESCENA MENU") 
        {
            StopMusic();
            Destroy(gameObject); 
        }
    }

    public void PlayMusic()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
