using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UiController : MonoBehaviour
{
    
    public GameObject painelVitoria, painelDerrota, painelInicio, uiPause, painelOptions;
    bool visivel;
    public bool visivelpause = false;
    public TextMeshProUGUI lifePlayer, numFosforo;
    public Slider All, Music, VFX;

    public Slider batterySlider;

    public Image[] hearts; 


    void Start()
    {
        GameController.controller.uiController = this;
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
    public void UpdateVida(int life)
    {
        lifePlayer.text = $"Life: {life}";
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
        Debug.Log("Nome" + levelToGo);
        Time.timeScale = 1.0f;
        Cheats.campFireIndex = 0;
    }
    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
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
