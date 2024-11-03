using System;
using System.IO;
using UnityEngine;

public class SaveData
{
    public int playerLives;
    public string level;
    public int lastSavePointReached;
    public bool hasFlashlight;
    public float flashlightBattery;
}

public static class SaveGame
{
    public static SaveData data;

    // Loads saved data right before the 'Unity' splash screen appears
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void GameInit()
    {
        LoadOrCreateSave();
    }

    public static void Save()
    {
        string path = Application.dataPath + "/save.txt";

        // Update saved data
        if (GameController.controller) // Gameplay data
        {
            data.playerLives = GameController.controller.lifePlayer;
            data.level = GameController.controller.currentLevel;
            data.flashlightBattery = LanternaPlayer.lanternaPlayer.bateriaAtual;

            if (InteracaoComItem.interacaoComItem)
            {
                data.hasFlashlight = InteracaoComItem.interacaoComItem.pegouLanterna;
            }

            if (GameController.controller.campFires != null)
            {
                for (int i = 0; i < GameController.controller.campFires.Length; i++)
                {
                    if (GameController.controller.campFires[i] == GameController.controller.Player.lastSavePointReached)
                    {
                        data.lastSavePointReached = i;
                    }
                }
            }
        }

        // Saves the data
        string s = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, s);
    }

    public static void Load()
    {
        string path = Application.dataPath + "/save.txt";
        string s = File.ReadAllText(path);
        data = JsonUtility.FromJson<SaveData>(s);
        if (data == null) // Prevents the save file from being empty
        {
            Debug.Assert(false, "Save file is empty. A new one will be created. Note that this is an unexpected behaviour and should not happen.");
            CreateSave();
        }
    }

    static void CreateSave()
    {
        // Creates new data
        data = new SaveData();
        data.playerLives = 3;
        data.level = "Nivel1";
        data.lastSavePointReached = 0;
        data.hasFlashlight = false;
        data.flashlightBattery = 0f;

        // Saves the data
        Save();
    }

    static void LoadOrCreateSave()
    {
        try
        {
            Load();
        }
        catch (Exception e)
        {
            CreateSave();
        }
    }

    public static void ResetRunData()
    {
        GameController.controller.lifePlayer = GameController.controller.lifeMax;
        GameController.controller.Player.lastSavePointReached = GameController.controller.campFires[0];
        InteracaoComItem.interacaoComItem.pegouLanterna = false;
        LanternaPlayer.lanternaPlayer.bateriaAtual = 0f;
    }

    public static void DeleteSave()
    {
        File.Delete(Application.dataPath + "/save.txt");
    }
}
