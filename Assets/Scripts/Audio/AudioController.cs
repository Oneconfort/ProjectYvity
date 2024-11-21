using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController audioController;
    public AudioClip[] myAudios;
    private AudioSource gameMusic;
    public AudioMixer audios;

    private void Awake()
    {
        if (audioController == null)
        {
            audioController = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        gameMusic = GetComponent<AudioSource>();
        if (gameMusic != null)
        {
            gameMusic.clip = myAudios[0];
        }
        gameMusic.Play();

        // Adiciona um listener para mudanças de cena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Remove o listener quando o objeto é destruído
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Chama o método para mudar a música com base na cena
        ChangeMusic(scene.name);
    }

    public void ChangeMusic(string levelToGo)
    {
        switch (levelToGo)
        {
            case "Menu":
                gameMusic.clip = myAudios[0];
                break;
            case "Nivel1":
                gameMusic.clip = myAudios[1];
                break;
            case "Nivel2":
                gameMusic.clip = myAudios[2];
                break;
            default:
                Debug.LogWarning($"Nenhuma música configurada para a cena: {levelToGo}");
                return;
        }
        gameMusic.Play();
    }

    public void ChangeAllVolume(float volume)
    {
        audios.SetFloat("MasterVolume", volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        audios.SetFloat("MusicVolume", volume);
    }

    public void ChangeVFXVolume(float volume)
    {
        audios.SetFloat("VFXVolume", volume);
    }
}
