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
    public AudioMixerGroup soundEffectsMixerGroup;
    public AudioClip[] soundEffects;
    public AudioSource[] soundEffectsSources;

    [ContextMenu("Generate audio sources for all SFX")]
    public void GenerateAudioSourcesForAllSoundEffects()
    {
        // Destroys all previous audio sources
        while (transform.childCount > 0) // Needs this "while" because Unity does not destroy all children even when we ask for it
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        Debug.Assert(transform.childCount <= 0);

#if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

        soundEffectsSources = new AudioSource[soundEffects.Length];
        for (int i = 0; i < soundEffects.Length; i++)
        {
            GameObject go = new GameObject(soundEffects[i].name);
            go.transform.SetParent(transform);
            go.transform.position = Vector3.zero;

            go.AddComponent<AudioSource>().clip = soundEffects[i];
            go.GetComponent<AudioSource>().playOnAwake = false;
            go.GetComponent<AudioSource>().outputAudioMixerGroup = soundEffectsMixerGroup;
            go.transform.SetSiblingIndex(i);

            soundEffectsSources[i] = go.GetComponent<AudioSource>();
        }
    }

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
            case "Caverna":
                gameMusic.clip = myAudios[3];
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

    public void PlaySoundEffectAtIndex(int index)
    {
        if (index >= soundEffectsSources.Length)
        {
            Debug.Assert(false, "This sound effect does not exist!");
            return;
        }

        soundEffectsSources[index].Play();
    }
}
