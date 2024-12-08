using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Player : MonoBehaviour
{
    // [SerializeField] private LayerMask Default;
    public Transform spherePoint;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float speed, speedMove, maxSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float jumpSpeed;
    private float speedBuffer;
    private float gravity;
    private float initialJumpSpeed;

    [SerializeField] public bool caverna = true;
   // private bool isPaused = false;
    private Vector3 dir = Vector3.zero;
    private Vector3 moveDir = Vector3.zero;

    private Rigidbody rb;

    private Animator animator;

    [SerializeField] private GameObject jumpParticle;
    [SerializeField] private GameObject moveParticle;
    public Transform pivot;
    public GameObject flor;

    bool insideLadder = false;
    public CampFire lastSavePointReached;
    public LayerMask mask;

    [Header("Config Audio")]
    public AudioClip audioClip;
    public AudioClip deadScream;
    public AudioSource audioSourcePassos;
    public AudioSource audioSource;
    public float delay = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        speedBuffer = speed;
        SetJumpVar();

        GameController.controller.lifePlayer = SaveGame.data.playerLives;
        lastSavePointReached = GameController.controller.campFires[SaveGame.data.lastSavePointReached];
        if (GameController.controller.lifePlayer < GameController.controller.lifeMax)
        {
            Respawn(lastSavePointReached);
        }

        GameController.controller.UpdateHearts();

        audioSourcePassos = GetComponent<AudioSource>();


        if (audioClip != null)
        {
            audioSourcePassos.clip = audioClip;
        }
    }

    private void Update()
    {
        if (dir.magnitude >= 1 && IsGrounded())
        {
            if (!audioSourcePassos.isPlaying)
            {
                audioSourcePassos.Play();
            }
            audioSourcePassos.loop = true;
        }
        else
        {
            audioSourcePassos.Pause();
            audioSourcePassos.loop = false;
        }
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
        rb.AddForce(new Vector3(newVelocity.x, 0, newVelocity.z), ForceMode.Acceleration);



        Vector3 dirRot = new Vector3(dir.x, 0, dir.z);
        Quaternion rot;
        float t = Time.fixedDeltaTime;

        if (dirRot != Vector3.zero)
        {
            rot = Quaternion.LookRotation(dirRot);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, t * rotateSpeed);
        }

    }

    void MoverJogadorComObjeto()
    {
        if (InteracaoComItem.interacaoComItem.pegouCaixa)
        {
            Vector3 moveComObj = transform.TransformDirection(dir) * speedMove;
            moveComObj.y = rb.velocity.y;
            rb.velocity = moveComObj;
        }
    }

    void MoverJogadorEm2D()
    {
        if (caverna == false)
        {
            Vector3 move = new Vector3(-dir.x, 0, -dir.z) * speedMove;
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
        if (!IsGrounded())
        {
            rb.velocity += new Vector3(0, gravity * 10 * Time.fixedDeltaTime, 0);
        }
    }

    void UsarEscada()
    {
        if (insideLadder && dir.z > 0)
        {
            GameController.controller.Player.transform.position += Vector3.up / 10;
        }
        else if (insideLadder && dir.z < 0)
        {
            GameController.controller.Player.transform.position += Vector3.down / 10;

            if (IsGrounded())
            {
                insideLadder = false;
            }
        }
        if (dir.magnitude <= 0 && insideLadder)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }

    public void OnMove(InputAction.CallbackContext ctxt)
    {
        Vector2 NewMoveDir = ctxt.ReadValue<Vector2>();
        dir.x = NewMoveDir.x;
        dir.z = NewMoveDir.y;
        animator.SetFloat("Speed", dir.magnitude);

        if (!moveParticle.activeSelf && IsGrounded())
        {
            moveParticle.SetActive(true);
        }

    }

    public void OnJump(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed && IsGrounded() && InteracaoComItem.interacaoComItem?.pegouCaixa == false)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpSpeed, rb.velocity.z);
            moveParticle.SetActive(false);

            animator.SetTrigger("Jump");
           // audioSourcePassos.Play();
            if (jumpParticle)
            {
                GameObject myParticle = Instantiate(jumpParticle, spherePoint.position, spherePoint.rotation);
                Destroy(myParticle, 1);
            }
            else
            {
                Debug.Assert(false, "'jumpParticle' reference is null!");
            }

            
          
        }
    }
    public void OnInteraction(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed)
        {
            InteracaoComItem.interacaoComItem?.InteracaoCenario();
        }
    }
    public void OnFlashlight(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed)
        {
            LanternaPlayer.lanternaPlayer?.Lanterna();
        }
    }
    public void OnRecharge(InputAction.CallbackContext ctxt)
    {
        if (ctxt.performed && GameController.controller?.fosforo != 0 && LanternaPlayer.lanternaPlayer?.bateriaAtual < 100)
        {
            AudioController.audioController.PlaySoundEffectAtIndex(1);
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



    void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Escada":
                animator.SetLayerWeight(3, 1f);

                rb.useGravity = false;
                rb.isKinematic = true;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                insideLadder = !insideLadder;
                break;
            case "Inimigo":
                GameController.controller.TomaDano(collider.gameObject.GetComponent<Enemy>().GetDamage());
                animator.SetTrigger("Dano");
                speed = 0f;
                Invoke("Velocidade", 1.2f);
                Destroy(collider.gameObject);
                break;
            case "ParedeFim":
              /*  audioSource.clip = deadScream;
                if (audioSource != null && audioClip != null)
                {
                    audioSource.Play();
                }*/
                GameController.controller.TomaDano(collider.gameObject.GetComponent<Inimigo>().GetDamage());
                CameraController.cameraController.cave = false;
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

                GerenciadorDeConquistas.instance.Camping();
                break;
            case "Fosforo":
                GameController.controller.fosforo++;
                GameController.controller.uiController.UpdateFosforo(GameController.controller.fosforo);
                AudioController.audioController.PlaySoundEffectAtIndex(16);
                Destroy(collider.gameObject);
                break;
            case "Caverna":
                if (caverna == false)
                {
                    transform.position = new Vector3(222.5f, 1.1f, 395.8f);
                    caverna = true;
                    AudioController.audioController.ChangeMusic(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
                else
                {
                    AudioController.audioController.ChangeMusic("Caverna");
                    transform.position = new Vector3(62.17f, 0.79f, 702.57f);
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(0, -180, 0);
                    caverna = false;
                }
                break;

            case "Caverna2":
                if (caverna == false)
                {
                    CameraController.cameraController.cave = false;
                    speed = 20;
                    transform.position = new Vector3(246.1f, -0.11f, 355.8f);
                    caverna = true;
                    AudioController.audioController.ChangeMusic(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
                else//entra
                {
                    CameraController.cameraController.transform.position = new Vector3(812.03f, 9.77f, 289.86f);


                    CameraController.cameraController.cave = true;
                    speed = 6;
                    transform.position = new Vector3(829.92f, -2.36f, 262.84f);
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(7f, -180, 0);
                    caverna = false;
                    AudioController.audioController.ChangeMusic("Caverna");
                }
                break;
            case "CavernaSaida2":
                if (caverna == false)
                {
                    CameraController.cameraController.cave = false;
                    transform.position = new Vector3(291.07f, 1.25f, 429.254f);
                    speed = 20;
                    caverna = true;
                    AudioController.audioController.ChangeMusic(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
                else
                {
                    CameraController.cameraController.transform.position = new Vector3(812.03f, 9.77f, 289.8f);
                    CameraController.cameraController.cave = true;
                    speed = 6;
                    transform.position = new Vector3(793.53f, 11.02f, 262.84f);
                    CameraController.cameraController.transform.rotation = Quaternion.Euler(7f, -180, 0);
                    caverna = false;
                    AudioController.audioController.ChangeMusic("Caverna");
                }
                break;
            case "Estalactite":
                GameController.controller.TomaDano(collider.gameObject.GetComponent<Inimigo>().GetDamage());
                Destroy(collider.gameObject);
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Icy"))
        {
            rb.drag = 0.25f;
            speed = speed * 0.25f;
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Icy"))
        {

            if (rb.velocity.magnitude <= 0)
            {
                rb.AddForce(0, 0, 0.5f * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Icy"))
        {
            rb.drag = 1.5f;
            speed = speedBuffer;
        }
    }
    public bool IsGrounded()
    {
        return Physics.CheckSphere(spherePoint.position, 0.1f, mask);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Escada"))
        {
            animator.SetLayerWeight(3, 0f);


            insideLadder = !insideLadder;
        }
    }
    public void Morrer()
    {
        animator.SetTrigger("Morrer");
        Invoke("SpawFlor", 1.5f);
        speed = 0;
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
        AudioController.audioController.ChangeMusic(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void SetJumpVar()
    {
        float timeApex = maxJumpTime / 2.0f;
        gravity = (-2.0f * maxJumpHeight) / Mathf.Pow(timeApex, 2.0f);
        initialJumpSpeed = (2.0f * maxJumpHeight) / timeApex;
    }

    void SpawFlor()
    {
        flor.SetActive(true);
    }

    void Velocidade()
    {
        speed = 20;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(spherePoint.position, 0.1f);
    }
}