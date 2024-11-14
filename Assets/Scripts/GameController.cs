using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController controller;
    public UiController uiController;
    public Player Player;
    public int lifeMax, lifePlayer, fosforo = 0;
    public CampFire[] campFires;
    public string currentLevel;



    [ContextMenu("Find All Camp Fires")]
    public void FindAllCampFires()
    {
        campFires = FindObjectsOfType<CampFire>();
    }

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }

        currentLevel = SceneManager.GetActiveScene().name;
    }
  

    public void UpdateHearts()
    {
        for (int i = 0; i < uiController.hearts.Length; i++)
        {
            uiController.hearts[i].enabled = i < lifePlayer;
        }
    }



    public void TomaDano(int damage)
    {
        lifePlayer += damage;
        UpdateHearts();
        if (lifePlayer <= 0)
        {
            controller.Player.Morrer();
        }
        if (lifePlayer >= lifeMax)
        {
            lifePlayer = lifeMax;
        }
    }

    public void PararJogo()
    {
        Time.timeScale = 0.0f;
        uiController.MostrarPainelFimDeJogo();
    }
    public void Vitoria()
    {
        Time.timeScale = 0.0f;
        uiController.MostrarPainelVitoria();
    }

    public void ControlCursor(bool cursor)
    {
        if (cursor == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnApplicationQuit()
    {
        SaveGame.Save();
    }


}
