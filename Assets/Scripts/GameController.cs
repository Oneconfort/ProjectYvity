using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController controller;
    public UiController uiController;
    public Player Player;
    public int lifeMax, lifePlayer;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }

    }

    private void Start()
    {
        lifePlayer = lifeMax;
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
}
