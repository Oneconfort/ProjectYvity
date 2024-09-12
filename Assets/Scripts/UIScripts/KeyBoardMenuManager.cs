using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardMenuManager : MonoBehaviour
{
    public InputActionReference moveRef, jumpRef, interactionRef, dropItemRef, flashlightRef;
   

    private void OnEnable()
    {
        moveRef.action.Disable();
        jumpRef.action.Disable();
        flashlightRef.action.Disable();
        dropItemRef.action.Disable();
        interactionRef.action.Disable();
    }
    private void OnDisable()
    {
        moveRef.action.Enable();
        jumpRef.action.Enable();
        dropItemRef.action.Enable();
        interactionRef.action.Enable();
        flashlightRef.action.Enable();
    }
}
