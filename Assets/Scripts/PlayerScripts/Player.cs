using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // [SerializeField] private LayerMask Default;

    [SerializeField] private Transform spherePoint;
    [SerializeField] private Transform point2;

    [SerializeField] private float speed, maxSpeed;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float hangTime;
    [SerializeField] private float  rotateSpeed;

    [SerializeField] private bool isGrounded = true;
    [SerializeField] public bool caverna = true;
    
    private float gravity = -9.81f;
    private float initialJumpSpeed;
    private float hangCounter;


    private Vector3 dir = Vector3.zero;
    private Vector3 moveDir = Vector3.zero;

    private Rigidbody rb;
    
    public GameObject playerModel;
  
    public Transform pivot;
    
    bool insideLadder = false;
    public CampFire lastSavePointReached;

    public LayerMask mask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetJumpVar();

        //GameController.controller.lifePlayer = SaveGame.data.playerLives;
        //lastSavePointReached = GameController.controller.campFires[SaveGame.data.lastSavePointReached];
        if (GameController.controller.lifePlayer < GameController.controller.lifeMax)
        {
            Respawn(lastSavePointReached);
        }

        GameController.controller.uiController.UpdateVida(GameController.controller.lifePlayer);
    }


    void Update()
    {
        if (GameController.controller.uiController.visivelpause == true) return;

        //Mover();
        CheckCheatInput();

    }

    private void FixedUpdate()
    {
        MoverJogador();

    }
    void Mover()
    {
        if (InteracaoComItem.interacaoComItem.pegouCaixa == true && LanternaPlayer.lanternaPlayer.caixaDetectada == false) return;

        IsGrunded();

        if (!insideLadder && dir != Vector3.zero)
        {

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

        //Gravidade();
        UsarEscada();

    }

    void CheckCheatInput()
    {
        if (!Debug.isDebugBuild) // Returns 'true' in the Editor as well
        {
            return; 
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Cheats.GoToPreviousCampFire();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Cheats.GoToNextCampFire();
        }
    }


    void RotacionarJogador()
    {
        Vector3 dirRot = new Vector3(dir.x, 0, dir.z);
        Quaternion rot = Quaternion.LookRotation(dirRot);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1);
    }

    void MoverJogador()
    {
        if (rb.velocity.magnitude < maxSpeed){
            rb.AddForce(dir, ForceMode.Acceleration);
        }
        if (IsGrunded())
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
            dir.y += gravity * Time.deltaTime;
        }
        Debug.Log(dir.z);
    }
    void Jump()
    {
        if ( hangCounter > 0)
        {
            dir.y = initialJumpSpeed;
        }
        else
        {
            dir.y += -0.1f * Time.deltaTime;
        }


    }

    void MoverJogadorComObjeto()
    {
        if (InteracaoComItem.interacaoComItem.pegouCaixa)
        {
            Vector3 moveComObj = transform.TransformDirection(moveDir) * speed * Time.deltaTime;
            rb.MovePosition(rb.position + moveComObj);
        }
    }

    void MoverJogadorEm2D()
    {
        if (caverna == false)
        {
            Vector3 move = new Vector3(-moveDir.x, 0, 0) * speed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
            if (move.x != 0)
            {
                if (move.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }
        }

    }
    /*void Gravidade()
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

    }*/

    void UsarEscada()
    {
        if (insideLadder && moveDir.z > 0)
        {
            GameController.controller.Player.transform.position += Vector3.up / 10;
            isGrounded = false;
        }
        else if (insideLadder && moveDir.z < 0)
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
        moveDir.x = NewMoveDir.x * -1;
        moveDir.z = NewMoveDir.y * -1;
        dir = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed * -1);

    }
    public void OnJump(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed && isGrounded && InteracaoComItem.interacaoComItem.pegouCaixa == false)
        {
            Jump();
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
            case"Cabana":
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
                    CameraController.cameraController.cam.orthographic = false;
                    caverna = true;
                }
                else
                {
                    transform.position = new Vector3(62.17f, 0.79f, 702.57f);
                    CameraController.cameraController.cam.orthographic = true;
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(0, -180, 0);
                    caverna = false;
                }
                break;
            
            case "Caverna2":
                if (caverna == false)
                {
                   CameraController.cameraController.cave = false;
                    speed = 10;
                    transform.position = new Vector3(246.1f, -0.11f, 355.8f);
                    CameraController.cameraController.cam.orthographic = false;
                    caverna = true;
                }
                else//entra
                {
                    CameraController.cameraController.transform.position = new Vector3(823.47f, 7.488f, 275.77f);

                    CameraController.cameraController.cave = true;
                    speed = 5;
                    transform.position = new Vector3(855.7f, -10.189f, 262.7f);
                    CameraController.cameraController.cam.orthographic = true;
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(0, -180, 0);
                    caverna = false;
                }
                break;
            case "CavernaSaida2":
                if (caverna == false)
                {
                    CameraController.cameraController.cave = false;
                    transform.position = new Vector3(291.07f, 1.25f, 429.254f);
                    speed = 10;
                    CameraController.cameraController.cam.orthographic = false;
                    caverna = true;
                }
                else
                {
                    CameraController.cameraController.transform.position = new Vector3(823.47f, 7.488f, 275.77f);
                    CameraController.cameraController.cave = true;
                    speed = 5;
                    transform.position = new Vector3(791.8f, 14.2f, 262.71f);
                  
                    CameraController.cameraController.cam.orthographic = true;
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(0, -180, 0);
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
    public bool IsGrunded()
    {
        return Physics.CheckSphere(spherePoint.position, 0.5f, mask);
    }
    void SetJumpVar()
    {
        float timeApex = maxJumpTime / 2.0f;
        gravity = (-2.0f * maxJumpHeight) / Mathf.Pow(timeApex, 2.0f);
        initialJumpSpeed = (2.0f * maxJumpHeight) / timeApex;
    }
}