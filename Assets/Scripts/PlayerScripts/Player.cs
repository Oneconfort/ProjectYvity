using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask Default;

    public float speed, jumpForce, rotateSpeed;
    private float gravity = -9.81f;
    Rigidbody rb;
    public GameObject playerModel;
    Vector3 moveDirection, moveSpeed;

    public bool isGrounded = true;
    bool insideLadder = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
    }

    void Update()
    {
        if (GameController.controller.uiController.visivelpause == true) return;

        Mover();
    }

    void Mover()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (insideLadder == false)
        {
            rb.useGravity = true;
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed);

                Vector3 move = transform.forward * moveDirection.magnitude;
                rb.MovePosition(rb.position + move * speed * Time.deltaTime);
            }
        }
        Pular();
        UsarEscada();
    }

    void Pular()
    {
        if (isGrounded)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += new Vector3(0, -0.1f * Time.deltaTime, 0);
            }
        }
        else
        {
            bool falling = rb.velocity.y < 0;
            if (falling)
            {
                rb.velocity += new Vector3(0, gravity * 3 * Time.deltaTime, 0);
            }
            else
            {
                rb.velocity += new Vector3(0, gravity * Time.deltaTime, 0);
            }
        }
    }

    void UsarEscada()
    {
        if (insideLadder && moveDirection.z > 0)
        {
            GameController.controller.Player.transform.position += Vector3.up / 10;
            isGrounded = false;
        }
        else if (insideLadder && moveDirection.z < 0)
        {
            GameController.controller.Player.transform.position += Vector3.down / 10;

            if (isGrounded == true)
            {
                insideLadder = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext ctxt)
    {
        Vector2 NewMoveDir = ctxt.ReadValue<Vector2>();
        moveDirection.x = NewMoveDir.x;
        moveDirection.z = NewMoveDir.y;
    }
    public void OnJump(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 10, rb.velocity.z);
        }
    }
    public void OnInteraction(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed)
        {
            InteracaoComItem.interacaoComItem.InteracaoCenario();
        }
    }
    public void OnFlashlight(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed)
        {
            LanternaPlayer.lanternaPlayer.Lanterna();
        }
    }
    public void OnDropItem(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed)
        {
            InteracaoComItem.interacaoComItem.DerrubarItem();
        }
    }

    public void TomaDano(int damage)
    {
        GameController.controller.lifePlayer += damage;
        GameController.controller.uiController.UpdateVida(GameController.controller.lifePlayer);
        if (GameController.controller.lifePlayer <= 0)
        {
            Morrer();
        }
        if (GameController.controller.lifePlayer >= GameController.controller.lifeMax)
        {
            GameController.controller.lifePlayer = GameController.controller.lifeMax;
        }
    }



    void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Escada":
                rb.useGravity = false;
                insideLadder = !insideLadder;
                break;
            case "Inimigo":
                TomaDano(collider.gameObject.GetComponent<Inimigo>().GetDamage());
                break;
            case "Inimigo1":
                TomaDano(collider.gameObject.GetComponent<Inimigo>().GetDamage());
                break;
            case "ParedeFim":
                gameObject.SetActive(false);
                TomaDano(collider.gameObject.GetComponent<Inimigo>().GetDamage());
                break;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Escada")
        {
            insideLadder = !insideLadder;
        }
    }

    public void Morrer()
    {
        GameController.controller.PararJogo();
    }
}

