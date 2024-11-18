using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SetInteractiveShaderEffects : MonoBehaviour
{
    [SerializeField]
    RenderTexture rt;
    [SerializeField]
    Transform target;
    public ParticleSystem particlesystem;

    float offset = 3.46f;

    void Awake()
    {
        Shader.SetGlobalTexture("_GlobalEffectRT", rt);
        Shader.SetGlobalFloat("_OrthographicCamSize", GetComponent<Camera>().orthographicSize);
    }
   
    
    private void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offset, target.transform.position.z);
        Shader.SetGlobalVector("_Position", transform.position);


        if (particlesystem != null)
        {
            var mainModule = particlesystem.main;

            if (GameController.controller.Player.isGrounded)
            {
                mainModule.maxParticles = 1000;
            }
            else
            {
                mainModule.maxParticles = 0;
            }
        }
    }


}