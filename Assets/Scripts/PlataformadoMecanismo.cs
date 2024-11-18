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
            rendererPlataforma.material.color = Color.green;
            boxCollider.isTrigger = false;
        }
        else
        {
            rendererPlataforma.material.color = Color.white;
            boxCollider.isTrigger = true;
        }
    }


}
