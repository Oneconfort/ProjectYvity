using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternaPlayer : MonoBehaviour
{
    public static LanternaPlayer lanternaPlayer;
    public GameObject lanterna, luminaria;
    public GameObject bauInteracao;
    public bool ligar = false;
    float maxBateria = 100f, drenagemBateria = 1f;
    public float bateriaAtual;


    void Awake()
    {
        if (lanternaPlayer == null)
        {
            lanternaPlayer = this;
        }
    }

    private void Update()
    {
        InteragirLanterna();
        Bateria();
    }

    public void Lanterna()
    {
        if (InteracaoComItem.interacaoComItem.pegouLanterna == true)
        {
            ligar = !ligar;
            lanterna.SetActive(ligar);
        }
    }

    void InteragirLanterna()
    {
        if (ligar)
        {
            string[] tags = { "Inimigo1", "Bau" };
            List<GameObject> alvos = new List<GameObject>();

            foreach (string tag in tags)
            {
                GameObject[] objetosComTag = GameObject.FindGameObjectsWithTag(tag);
                alvos.AddRange(objetosComTag);
            }

            foreach (var alvoLanterna in alvos)
            {
                if (EstaNaFrenteEProximo(alvoLanterna.transform.position))
                {
                    switch (alvoLanterna.tag)
                    {
                        case "Inimigo1":
                            Destroy(alvoLanterna.gameObject, 0.5f);
                            break;
                        case "Bau":
                            bauInteracao = alvoLanterna;
                            if (bauInteracao != null)
                            {
                                bauInteracao.SendMessage("SpawnItemBau", SendMessageOptions.DontRequireReceiver);
                                bauInteracao = null;
                            }
                            break;
                    }
                }
            }
        }
    }


    bool EstaNaFrenteEProximo(Vector3 posicaoAlvo)
    {
        if (ligar && InteracaoComItem.interacaoComItem.pegouLanterna)
        {
            Vector3 dir = posicaoAlvo - transform.position;
            float dot = Vector3.Dot(dir.normalized, transform.forward);
            return dot > 0.5f && dir.magnitude < 3.5f;
        }
        if (ligar && InteracaoComItem.interacaoComItem.pegouLanterna == false)
        {
            Vector3 dir = posicaoAlvo - transform.position;
            bool isInSphere = dir.magnitude < 3;
            return isInSphere;
        }
        return false;
    }


    void Start()
    {
        bateriaAtual = maxBateria;
        if (GameController.controller.uiController.batterySlider != null)
        {
            GameController.controller.uiController.batterySlider.maxValue = maxBateria;
            GameController.controller.uiController.batterySlider.value = bateriaAtual;
        }
    }

    void Bateria()
    {
        if (ligar)
        {
            DrenagemBateria();
        }
        if (GameController.controller.uiController.batterySlider != null)
        {
            GameController.controller.uiController.batterySlider.value = bateriaAtual;
        }

        if (bateriaAtual <= 0)
        {
            ligar = false;
            lanterna.SetActive(false);
            luminaria.SetActive(false);
        }
    }

    void DrenagemBateria()
    {
        bateriaAtual -= drenagemBateria * Time.deltaTime;
        bateriaAtual = Mathf.Clamp(bateriaAtual, 0, maxBateria);
    }

    public void RecargaBateria(float quantidade)
    {
        if (InteracaoComItem.interacaoComItem.pegouLanterna == true)
        {
            bateriaAtual += quantidade;
            bateriaAtual = Mathf.Clamp(bateriaAtual, 0, maxBateria);
        }
    }
}