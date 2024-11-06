
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
//using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    // [SerializeField] private LayerMask Default;

    [SerializeField] private Transform spherePoint;

    [SerializeField] private float speed, maxSpeed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] public bool caverna = true, isGrounded = true;

    private float gravity = -9.81f;
    private float initialJumpSpeed;

   
    private Vector3 dir = Vector3.zero;
    private Vector3 moveDir = Vector3.zero;

    private Rigidbody rb;

    private Animator animator;

    public Transform pivot;

    bool insideLadder = false;
    public CampFire lastSavePointReached;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        GameController.controller.lifePlayer = SaveGame.data.playerLives;
        lastSavePointReached = GameController.controller.campFires[SaveGame.data.lastSavePointReached];
        if (GameController.controller.lifePlayer < GameController.controller.lifeMax)
        {
            Respawn(lastSavePointReached);
        }

        GameController.controller.uiController.UpdateVida(GameController.controller.lifePlayer);
    }


    void FixedUpdate()
    {
        if (GameController.controller.uiController.visivelpause == true) return;

        Mover();
        CheckCheatInput();
    }

    void Mover()
    {
        if (InteracaoComItem.interacaoComItem.pegouCaixa == true && LanternaPlayer.lanternaPlayer.caixaDetectada == false) return;

        IsGrounded();

        if (!insideLadder && dir != Vector3.zero)
        {
            rb.useGravity = true;
            rb.isKinematic = false;

            if (!InteracaoComItem.interacaoComItem.pegouCaixa)
            {
                RotacionarJogador();
                MoverJogador();
            }
            if (!InteracaoComItem.interacaoComItem.pegouCaixa && caverna == true)
            {
                RotacionarJogador();
                MoverJogador();
            }
            MoverJogadorComObjeto();

            MoverJogadorEm2D();
        }
        Gravidade();
        UsarEscada();
    }

    void CheckCheatInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Cheats.GoToPreviousCampFire();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Cheats.GoToNextCampFire();
        }
    }

    void IsGrounded()
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
        if (dir.z != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        }
    }

    void MoverJogador()
    {   //camera
        Vector3 cameraForward = CameraController.cameraController.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 move = (cameraForward * dir.z + CameraController.cameraController.transform.right * dir.x);
        move.Normalize();

        //AddForce
        Vector3 force = move * speed;
        Vector3 newVelocity = new Vector3(force.x, rb.velocity.y, force.z) - rb.velocity;

        rb.AddForce(new Vector3(newVelocity.x, 0, newVelocity.z), ForceMode.VelocityChange);
        
    }



    void MoverJogadorComObjeto()
    {
        if (InteracaoComItem.interacaoComItem.pegouCaixa)
        {
            Vector3 moveComObj = transform.TransformDirection(dir) * speed;
            moveComObj.y = rb.velocity.y;
            rb.velocity = moveComObj;
        }
    }

    void MoverJogadorEm2D()
    {
        if (caverna == false)
        {
            Vector3 move = new Vector3(-dir.x, 0, -dir.z) * speed;
            move.y = rb.velocity.y;

            rb.velocity = move;

            Vector3 dirRot = new Vector3(-dir.x, 0, -dir.z);
            Quaternion rot;

            if (dirRot != Vector3.zero)
            {
                rot = Quaternion.LookRotation(dirRot);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1);
            }
        }
    }
    void Gravidade()
    {
       if (!isGrounded)
        {
            rb.velocity += new Vector3(0, gravity * 2 * Time.deltaTime, 0);
        }
    }

    void UsarEscada()
    {
        if (insideLadder && dir.z > 0)
        {
            GameController.controller.Player.transform.position += Vector3.up / 10;
            isGrounded = false;
        }
        else if (insideLadder && dir.z < 0)
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
        dir.x = NewMoveDir.x;
        dir.z = NewMoveDir.y;
        animator.SetFloat("Speed", dir.magnitude);
    }
    public void OnJump(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed && isGrounded && InteracaoComItem.interacaoComItem.pegouCaixa == false)
        {
            rb.velocity = new Vector3(rb.velocity.x, 24.5F, rb.velocity.z);
            animator.SetTrigger("Jump");
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
            case "ParedeFim":
                TomaDano(collider.gameObject.GetComponent<Inimigo>().GetDamage());
                CameraController.cameraController.cam.orthographic = false;
                caverna = true;
                if (GameController.controller.lifePlayer > 0) // Respawn
                {
                    Respawn(lastSavePointReached);
                }
                else // Death
                {
                    gameObject.SetActive(false);
                }
                break;
            case "Cabana":
                GameController.controller.Vitoria();
                break;
            case "Fosforo":
                GameController.controller.fosforo++;
                GameController.controller.uiController.UpdateFosforo(GameController.controller.fosforo);
                Destroy(collider.gameObject);
                break;
            case "Caverna":
                if (caverna == false)
                {
                    transform.position = new Vector3(222.5f, 1.1f, 395.8f);
                    caverna = true;
                }
                else
                {
                    transform.position = new Vector3(62.17f, 0.79f, 702.57f);
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(0, -180, 0);
                    caverna = false;
                }
                break;

            case "Caverna2":
                if (caverna == false)
                {
                    CameraController.cameraController.cave = false;
                    speed = 11;
                    transform.position = new Vector3(246.1f, -0.11f, 355.8f);
                    caverna = true;
                }
                else//entra
                {
                    CameraController.cameraController.transform.position = new Vector3(812.039f, 11.930f, 284.75f);


                    CameraController.cameraController.cave = true;
                    speed = 6;
                    transform.position = new Vector3(829.92f, -2.36f, 262.84f);
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(11.763f, -180, 0);
                    caverna = false;
                }
                break;
            case "CavernaSaida2":
                if (caverna == false)
                {
                    CameraController.cameraController.cave = false;
                    transform.position = new Vector3(291.07f, 1.25f, 429.254f);
                    speed = 11;
                    caverna = true;
                }
                else
                {
                    CameraController.cameraController.transform.position = new Vector3(812.039f, 11.930f, 284.75f);
                    CameraController.cameraController.cave = true;
                    speed = 6;
                    transform.position = new Vector3(793.53f, 11.02f, 262.84f);
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(11.763f, -180, 0);
                    caverna = false;
                }
                break;
            case "Estalactite":
                TomaDano(collider.gameObject.GetComponent<Inimigo>().GetDamage());
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

    public void Respawn(CampFire savePoint)
    {
        if (!savePoint)
        {
            Debug.Assert(false, "Save Point is null.");
            return;
        }

        transform.position = savePoint.spawnPoint.position;
        transform.rotation = savePoint.spawnPoint.rotation;
    }
}