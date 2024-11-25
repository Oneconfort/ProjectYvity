using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour
{

    private BoxCollider box;
    public GameObject gelo;
    int blendShapeIndex = 0;
    SkinnedMeshRenderer skinnedMeshRenderer;
   
    void Start()
    {
        box = GetComponent<BoxCollider>();
        skinnedMeshRenderer = gelo.GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        AtualizarAparenciaCaixa();
    }

    private void AtualizarAparenciaCaixa()
    {
        if (LanternaPlayer.lanternaPlayer.caixaDetectada)
        {
            Escalar();
        }
        else
        {
            Desescalar();
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

    void Escalar()
    {
        float currentBlendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);

        if (currentBlendShapeWeight < 100)
        {
            float newBlendShapeWeight = Mathf.Min(currentBlendShapeWeight + (60 * Time.deltaTime), 100);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, newBlendShapeWeight);
        }
    }
    void Desescalar()
    {
        float currentBlendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);

        if (currentBlendShapeWeight > 0)
        {
            float newBlendShapeWeight = Mathf.Max(currentBlendShapeWeight - (60 * Time.deltaTime), 0);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, newBlendShapeWeight);
        }
    }
}
