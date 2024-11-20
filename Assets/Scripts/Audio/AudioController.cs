using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public static AudioController audioController;
    public AudioClip[] myAudios;
    private AudioSource gameMusic;
    public AudioMixer audio;
    private void Awake()
    {
        if (audioController == null)
        {
            audioController = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        gameMusic = GetComponent<AudioSource>();
    }

    public void ChangeAllVolume(float volume)
    {
        audio.SetFloat("MasterVolume", volume);
    }
    public void ChangeMusicVolume(float volume)
    {
        audio.SetFloat("MusicVolume", volume);
    }
    public void ChangeVFXVolume(float volume)
    {
        audio.SetFloat("VFXVolume", volume);
    }
    public void ChangeMusic(string scene)
    {
        switch (scene)
        {
            default:
                gameMusic.clip = myAudios[0];
                break;
        }
        gameMusic.Play();
    }
}
