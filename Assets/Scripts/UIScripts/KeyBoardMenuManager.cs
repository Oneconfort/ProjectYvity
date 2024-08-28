using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardMenuManager : MonoBehaviour
{
    public InputActionReference moveRef, jumpRef, interactionRef, dropItemRef;
   

    private void OnEnable()
    {
        moveRef.action.Disable();
        jumpRef.action.Disable();
        dropItemRef.action.Disable();
        interactionRef.action.Disable();
    }
    private void OnDisable()
    {
        moveRef.action.Enable();
        jumpRef.action.Enable();
        dropItemRef.action.Enable();
        interactionRef.action.Enable();
    }
}
