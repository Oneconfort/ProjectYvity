using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformadoMecanismo : MonoBehaviour
{
    private Renderer rendererPlataforma;
    BoxCollider boxCollider;

    void Start()
    {
        rendererPlataforma = GetComponent<Renderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        AtualizarPlataforma();
    }

    private void AtualizarPlataforma()
    {
        if (LanternaPlayer.lanternaPlayer.detectedPlataformas.Contains(gameObject))
        {
            //pode ficar em ci
            rendererPlataforma.enabled = false;
            boxCollider.isTrigger = true;
        }
        else
        {
            rendererPlataforma.enabled = true;
            boxCollider.isTrigger = false;
        }
    }


}
