using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererPlayer : MonoBehaviour
{
    public Camera mainCamera;
    public Camera overlayCamera;
    public Material defaultMaterial;
    public Material shadowMaterial;

    private Renderer playerRenderer;

    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        CheckShadow();
    }

    void CheckShadow()
    {
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        if (Physics.Raycast(transform.position, directionToCamera, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Parede"))
            {
                playerRenderer.material = shadowMaterial;
                overlayCamera.enabled = true;
                mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
            }
        }
        else 
        {
            playerRenderer.material = defaultMaterial;
            mainCamera.cullingMask = ~0; 
            overlayCamera.enabled = false;
        }
    }
}