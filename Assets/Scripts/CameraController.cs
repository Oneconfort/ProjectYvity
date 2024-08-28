using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraController : MonoBehaviour
{

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private float initialYPosition;


    void LateUpdate()
    {
        if (GameController.controller.Player.isGrounded == true)
        {
            initialYPosition = GameController.controller.Player.transform.position.y;

        }

        transform.position = GameController.controller.Player.transform.position + offset;
    }
}
