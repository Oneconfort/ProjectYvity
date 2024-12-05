using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleShapeKey : MonoBehaviour
{
    int blendShapeIndex = 0;
    SkinnedMeshRenderer skinnedMeshRenderer;
    bool ativar = false;
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

    }

    
    void Update()
    {
        Escalar();
    }
    void Escalar()
    {
        if (ativar)
        {
            float currentBlendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);

            if (currentBlendShapeWeight < 100)
            {
                float newBlendShapeWeight = Mathf.Min(currentBlendShapeWeight + (17 * Time.deltaTime), 100);
                skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, newBlendShapeWeight);
            }
        }
        
    }
    void Ativar()
    {
        ativar = true;
        Destroy(gameObject, 5);
    }
    /*private void OnDestroy()
    {
        if (SceneManager.GetActiveScene().name == "Nivel1")
        {
            int gelo = +1;
            GerenciadorDeConquistas.instance.Melting(gelo);
        }
        if (SceneManager.GetActiveScene().name == "Nivel2")
        {
            int tentaculo = +1;
            GerenciadorDeConquistas.instance.Tentacles(tentaculo);
        }
    }*/
}
