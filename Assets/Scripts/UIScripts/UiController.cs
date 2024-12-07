using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting.FullSerializer;

public class UiController : MonoBehaviour
{
    
    public GameObject painelVitoria, painelDerrota, painelInicio, uiPause, painelOptions;
    bool visivel;
    public bool visivelpause = false;
    public TextMeshProUGUI  numFosforo;
    public Slider All, Music, VFX;

    public Slider batterySlider;

    public Image[] hearts; 


    void Start()
    {
        GameController.controller.uiController = this;
        SetValores();
    }


    private void Update()
    {
        UiPause();
    }

    void UiPause()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            visivel = !visivel;
            uiPause.SetActive(visivel);
            if (visivel == true)
            {
                visivelpause = true;
                Time.timeScale = 0.0f;
                GameController.controller.ControlCursor(false);
            }
            else
            {
                visivelpause = false;
                Time.timeScale = 1.0f;
                GameController.controller.ControlCursor(true);
            }
        }
    }

    public void BotaoPause()
    {
        visivel = !visivel;
        uiPause.SetActive(visivel);

        if (visivel)
        {
            visivelpause = true;
            Time.timeScale = 0.0f;
            GameController.controller.ControlCursor(false);
        }
        else
        {
            visivelpause = false;
            Time.timeScale = 1.0f;
            GameController.controller.ControlCursor(true);
        }
    }
    
    public void UpdateFosforo(int fosforo)
    {
        numFosforo.text = $"{fosforo}";
    }

    public void PainelOptions()
    {
        GameController.controller.ControlCursor(false);
        visivel = !visivel;
        painelOptions.SetActive(visivel);
    }
    public void FecharPainelInicio(string scene)
    {
        GameController.controller.ControlCursor(true);
        SceneManager.LoadScene(scene);
        Time.timeScale = 1.0f;
        Cheats.campFireIndex = 0;
    }
    public void MostrarPainelFimDeJogo()
    {
        GameController.controller.ControlCursor(false);
        visivel = !visivel;
        painelDerrota.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void MostrarPainelVitoria()
    {
        GameController.controller.ControlCursor(false);
        visivel = !visivel;
        painelVitoria.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void Resets(int num)
    {
        Time.timeScale = 1.0f;
        SaveGame.ResetRunData();
        SaveGame.Save();
        SceneManager.LoadScene(num);
        Cheats.campFireIndex = 0;
    }

    public void ChangeScene(string levelToGo)
    {
        SceneManager.LoadScene(levelToGo);
        AudioController.audioController.ChangeMusic(levelToGo);
     //   Debug.Log("Nome" + levelToGo);
        Time.timeScale = 1.0f;
        Cheats.campFireIndex = 0;
    }
    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
    void SetValores()
    {
        AudioController.audioController.audios.GetFloat("MasterVolume", out float auxiliar);
        if (All != null) All.value = auxiliar;
        AudioController.audioController.audios.GetFloat("MusicVolume", out float auxiliar2);
        if (Music != null) Music.value = auxiliar2;
        AudioController.audioController.audios.GetFloat("VFXVolume", out float auxiliar3);
        if (VFX != null) VFX.value = auxiliar3;

       // if (uiConfig != null) uiConfig.SetActive(false);
    }
    public void ChangeAllVolume()
    {
        AudioController.audioController.ChangeAllVolume(All.value);
    }
    public void ChangeMUsicVolume()
    {
        AudioController.audioController.ChangeMusicVolume(Music.value);
    }
    public void ChangeVFXVolume()
    {
        AudioController.audioController.ChangeVFXVolume(VFX.value);
    }

}
