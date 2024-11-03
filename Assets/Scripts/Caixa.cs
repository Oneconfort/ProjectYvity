using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour
{

    private Renderer rendererCaixa;
    private BoxCollider box;

    void Start()
    {
        rendererCaixa = GetComponentInChildren<Renderer>();

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
            rendererCaixa.material.color = Color.green;
        }
        else
        {
            rendererCaixa.material.color = Color.white;
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
