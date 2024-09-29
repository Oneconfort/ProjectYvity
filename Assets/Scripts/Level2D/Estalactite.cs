using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estalactite : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] int blendShapeIndex = 0;
   

    void Update()
    {
        float currentBlendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);

        if ( currentBlendShapeWeight > 0)
        {
            float newBlendShapeWeight = Mathf.Max(currentBlendShapeWeight - 50 * Time.deltaTime, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, newBlendShapeWeight);
        }
    }
}