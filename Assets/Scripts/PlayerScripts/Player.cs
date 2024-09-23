using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // [SerializeField] private LayerMask Default;

    public float speed, rotateSpeed;
    private float gravity = -9.81f;
    Rigidbody rb;
    public GameObject playerModel;
    Vector3 moveDirection, moveSpeed;
    public Transform pivot;
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
        if (InteracaoComItem.interacaoComItem.pegouCaixa == true && LanternaPlayer.lanternaPlayer.caixaDetectada == false) return;

        VerificarSeEstaNoChao();

        if (!insideLadder && moveDirection != Vector3.zero)
        {
            rb.useGravity = true;
            rb.isKinematic = false;

            if (!InteracaoComItem.interacaoComItem.pegouCaixa)
            {
                RotacionarJogador();
                MoverJogador();
            }
            else
            {
                MoverJogadorComObjeto();
            }
        }
        Gravidade();
        UsarEscada();

    }

    void VerificarSeEstaNoChao()
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
    }

    void RotacionarJogador()
    {
        if (moveDirection.z != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
        }
    }

    void MoverJogador()
    {
        Vector3 cameraForward = CameraController.cameraController.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 move = cameraForward * moveDirection.z + CameraController.cameraController.transform.right * moveDirection.x;

        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    void MoverJogadorComObjeto()
    {
        Vector3 moveComObj = transform.TransformDirection(moveDirection) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveComObj);
    }

    void Gravidade()
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
        if (ctxt.performed && isGrounded && InteracaoComItem.interacaoComItem.pegouCaixa == false)
        {
            rb.velocity = new Vector3(rb.velocity.x, 13, rb.velocity.z);
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
    public void OnRecharge(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed && GameController.controller.fosforo != 0 && LanternaPlayer.lanternaPlayer.bateriaAtual < 100)
        {
            GameController.controller.fosforo--;
            GameController.controller.uiController.UpdateFosforo(GameController.controller.fosforo);
            LanternaPlayer.lanternaPlayer.RecargaBateria(100);
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
                rb.isKinematic = true;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
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
            case"Cabana":
                GameController.controller.Vitoria();
                break;
            case "Fosforo":
                GameController.controller.fosforo++;
                GameController.controller.uiController.UpdateFosforo(GameController.controller.fosforo);
                Destroy(collider.gameObject);
                break;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Escada":
                insideLadder = !insideLadder;
                break;

        }
    }
    public void Morrer()
    {
        GameController.controller.PararJogo();
    }
}