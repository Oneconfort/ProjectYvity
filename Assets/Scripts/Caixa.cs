using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour
{

    private BoxCollider box;
    public GameObject gelo;

    void Start()
    {
        box = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        AtualizarAparenciaCaixa();
    }

    private void AtualizarAparenciaCaixa()
    {
        if (LanternaPlayer.lanternaPlayer.caixaDetectada)
        {
            gelo.SetActive(false);
        }
        else
        {
            gelo.SetActive(true);
        }
        if (InteracaoComItem.interacaoComItem.pegouCaixa)
        {
            box.enabled = false;
        }
        else
        {
            box.enabled = true;
        }
    }
}
