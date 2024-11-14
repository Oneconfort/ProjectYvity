using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class InteracaoComItem : MonoBehaviour
{
    public static InteracaoComItem interacaoComItem;
    float dropForce = 1.0f;
    private BoxCollider boxCollider;
    public GameObject holdPosition;
    GameObject heldItem;
    private Rigidbody heldItemRigidbody;
    public GameObject inimigo;
    public bool pegouLanterna = false, pegouItem = false, pegouCaixa = false;
    Animator animator;
    //tirolesa
    public Transform startPoint;
    public Transform endPoint;
    bool isZipping = false;


    void Awake()
    {
        if (interacaoComItem == null)
        {
            interacaoComItem = this;
        }
    }

    void Start()
    {
        if (SaveGame.data.hasFlashlight)
        {
            ProcessarInteracao(LanternaPlayer.lanternaPlayer.gameObject);
        }
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void InteracaoCenario()
    {
        if (GameController.controller.uiController.visivelpause == true || pegouCaixa == true) return;

        string[] tags = { "ObjetoI", "Tirolesa", "Lanterna", "Fogueira", "Caixa", "Alavanca", "Mecanismo" };
        List<GameObject> todosOsObjetos = new List<GameObject>();

        foreach (string tag in tags)
        {
            GameObject[] objetosComTag = GameObject.FindGameObjectsWithTag(tag);
            todosOsObjetos.AddRange(objetosComTag);
        }

        foreach (GameObject alvo in todosOsObjetos)
        {
            if (EstaNaFrenteEProximo(alvo.transform.position))
            {
                ProcessarInteracao(alvo);
            }
        }
    }

    void ProcessarInteracao(GameObject alvo)
    {
        switch (alvo.tag)
        {
            case "ObjetoI":
                PegarItem(alvo);
                break;
            case "Caixa":
                if (LanternaPlayer.lanternaPlayer.ligar)
                {
                    LanternaPlayer.lanternaPlayer.ReposicionarLanterna(alvo);
                }
                if (LanternaPlayer.lanternaPlayer.caixaDetectada == true)
                {
                    PegarItem(alvo);
                    pegouCaixa = true;
                    boxCollider.enabled = true;
                }
                break;
            case "Lanterna":
                pegouLanterna = true;
                PegarItem(alvo);
                animator.SetLayerWeight(1, 1f);
                LanternaPlayer.lanternaPlayer.LigarLanterna();
                break;
            case "Tirolesa":
                if (!isZipping)
                {
                    transform.position = gameObject.transform.position;
                    StartCoroutine(ZiplineMovement());
                }
                isZipping = false;
                break;
            case "Fogueira":
                LanternaPlayer.lanternaPlayer.RecargaBateria(100);
                GameController.controller.Player.lastSavePointReached = alvo.GetComponent<CampFire>();
                break;
            case "Alavanca":
                var buttonScript = alvo.GetComponent<ButtonScript>();
                if (buttonScript != null)
                {
                    buttonScript.AlternarBotao();
                }
                break;
            case "Mecanismo":
                if (LanternaPlayer.lanternaPlayer.naPosicao == false)
                {
                    LanternaPlayer.lanternaPlayer.ReposicionarLanterna(alvo);
                }
                else
                {
                    PegarItem(alvo);
                    LanternaPlayer.lanternaPlayer.naPosicao = false;
                }
                break;
        }
    }

    bool EstaNaFrenteEProximo(Vector3 posicaoAlvo)
    {
        Vector3 dir = posicaoAlvo - transform.position;
        float dot = Vector3.Dot(dir.normalized, transform.forward);
        return dot > 0.5f && dir.magnitude < 3.5f;
    }

    public void PegarItem(GameObject item)
    {
        if (heldItem == null)
        {
            heldItem = item;
            pegouItem = true;
            heldItemRigidbody = heldItem.GetComponent<Rigidbody>();
            heldItemRigidbody.isKinematic = true;
           
                heldItem.transform.SetParent(holdPosition.transform);
            
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }
    }

    public void DerrubarItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            if (pegouCaixa == false)
            {
                heldItemRigidbody.isKinematic = false;
            }
            heldItemRigidbody.AddForce(holdPosition.transform.forward * dropForce, ForceMode.Impulse);
            pegouItem = false;
            pegouCaixa = false;
            boxCollider.enabled = false;

            heldItem = null;

            if (pegouLanterna == true)
            {
                LanternaPlayer.lanternaPlayer.LigarLuminaria();
                animator.SetLayerWeight(1, 0f);
                pegouLanterna = false;
            }

            if (LanternaPlayer.lanternaPlayer.naPosicao == true)
            {
                PegarItem(LanternaPlayer.lanternaPlayer.lanterna);
                pegouLanterna = true;
                animator.SetLayerWeight(1, 1f);
                LanternaPlayer.lanternaPlayer.LigarLanterna();
                LanternaPlayer.lanternaPlayer.naPosicao = false;
            }
        }
    }

    IEnumerator ZiplineMovement()
    {
        if (!heldItem)
        {
            int velocity = 15;
            isZipping = true;
            float progress = 0f;
            while (progress < 1f)
            {
                progress += Time.deltaTime * velocity / Vector3.Distance(startPoint.position, endPoint.position);
                transform.position = Vector3.Lerp(startPoint.position, endPoint.position, progress);
                GameController.controller.Player.transform.rotation = startPoint.rotation;

                yield return null;
            }
        }
    }
}

