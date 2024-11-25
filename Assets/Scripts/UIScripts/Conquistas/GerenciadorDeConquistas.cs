using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class GerenciadorDeConquistas : MonoBehaviour
{
    public static GerenciadorDeConquistas instance;
    public List<Conquista> conquistas;
    string Conquistas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

 
    public void ResetarConquistas()
    {
        foreach (Conquista conquista in conquistas)
        {
            conquista.conquistaAtivada = false;
        }

        SalvarConquistas();
    }
    private void Start()
    {
        //  ResetarConquistas();
        Conquistas = Application.persistentDataPath + "/Conquistas.json";
        CarregarConquistas();
    }

     public void Heated(int fogueira)
     {
         if (ValidarSeConquistaFoiHabilitada(1) == true) return;
         
         if (fogueira  == 5f)
         {
             conquistas[1].conquistaAtivada = true;
           //  GameManager.gameManager.PointMaster.SetActive(true);
             SalvarConquistas();
         }
     }

    public void Camping()
    {
        if (ValidarSeConquistaFoiHabilitada(3) == true) return;

        conquistas[3].conquistaAtivada = true;
        // GameController.controller.imagemDeadlyMarathon.SetActive(true);

        SalvarConquistas();

    }

     public void Melting(int gelo)
     {
         if (ValidarSeConquistaFoiHabilitada(0) == true) return;
         if (gelo == 10)
        {
             conquistas[0].conquistaAtivada = true;
            // GameManager.gameManager.MasterofTime.SetActive(true);

             SalvarConquistas();
        }
     }
    public void Tentacles(int tentaculos)
    {
        if (ValidarSeConquistaFoiHabilitada(2) == true) return;
        if (tentaculos == 10)
        {
            conquistas[2].conquistaAtivada = true;
            // GameManager.gameManager.MasterofTime.SetActive(true);

            SalvarConquistas();
        }
    }
    

    public bool ValidarSeConquistaFoiHabilitada(int id)
    {
        bool resultado = false;
        for (int i = 0; i < conquistas.Count; i++)
        {
            if (conquistas[i].ID == id && conquistas[i].conquistaAtivada == true)
            {

                resultado = true;
            }
        }
        return resultado;
    }
    [ContextMenu("SalvarConquistas")]
    public void SalvarConquistas()
    {
        string json = JsonHelper.ToJson(conquistas.ToArray());
        File.WriteAllText(Conquistas, json);
    }

    [ContextMenu("CarregarConquistas")]
    public void CarregarConquistas()
    {
        if (File.Exists(Conquistas) == false) return;
        string jsonSalvo = File.ReadAllText(Conquistas);
        Conquista[] conquistasContainer = JsonHelper.FromJson<Conquista>(jsonSalvo);

        for (int i = 0; i < conquistasContainer.Length; i++)
        {
            conquistas[i].codIcone = conquistasContainer[i].codIcone;
            conquistas[i].nome = conquistasContainer[i].nome;
            conquistas[i].dscConquista = conquistasContainer[i].dscConquista;
            conquistas[i].conquistaAtivada = conquistasContainer[i].conquistaAtivada;
            conquistas[i].ID = conquistasContainer[i].ID;
        }
    }
}

[System.Serializable]
public class Conquista
{
    public int ID = 0;
    public string nome;
    public string dscConquista;
    public int codIcone;
    public bool conquistaAtivada;

}

public class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
