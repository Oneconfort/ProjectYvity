using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour
{

    private Renderer rendererCaixa;

    void Start()
    {
        rendererCaixa = GetComponent<Renderer>();
        rendererCaixa.material.color = Color.magenta;
    }

    private void Update()
    {
        AtualizarAparenciaCaixa();
    }

    private void AtualizarAparenciaCaixa()
    {
        if (LanternaPlayer.lanternaPlayer.caixaDetectada == true)
        {
            rendererCaixa.material.color = Color.green;
        }
        else
        {
            rendererCaixa.material.color = Color.gray;
        }
    }
}
