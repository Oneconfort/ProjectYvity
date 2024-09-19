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
    public GameObject holdPosition;
    GameObject heldItem;
    private Rigidbody heldItemRigidbody;
    public GameObject inimigo;
    public bool pegouLanterna = false, pegouItem = false, pegouCaixa = false;

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

    public void InteracaoCenario()
    {
        string[] tags = { "ObjetoI", "Tirolesa", "Lanterna", "Fogueira", "Caixa", "Alavanca" };
        List<GameObject> todosOsObjetos = new List<GameObject>();

        foreach (string tag in tags)
        {
            GameObject[] objetosComTag = GameObject.FindGameObjectsWithTag(tag);
            todosOsObjetos.AddRange(objetosComTag);
        }

        foreach (GameObject alvo in todosOsObjetos)
        {
            if (EstaNaFrenteEProximo(alvo.transform.position, alvo))
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
                LanternaPlayer.lanternaPlayer.ReposicionarLanterna(alvo);
                if (LanternaPlayer.lanternaPlayer.caixaDetectada == true)
                {
                    PegarItem(alvo);
                    pegouCaixa = true;
                }
                break;
            case "Lanterna":
                PegarItem(alvo);
                pegouLanterna = true;
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
                break;
            case "Alavanca":
               
                break;
        }
    }

    bool EstaNaFrenteEProximo(Vector3 posicaoAlvo, GameObject alvo)
    {
        Vector3 dir = posicaoAlvo - transform.position;
        float dot = Vector3.Dot(dir.normalized, transform.forward);
        return dot > 0.5f && dir.magnitude < 6.5f;
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

            heldItem = null;

            if (pegouLanterna == true)
            {
                LanternaPlayer.lanternaPlayer.LigarLuminaria();
                pegouLanterna = false;
            }

            if (pegouCaixa)
            {
                pegouCaixa = false;

                if (LanternaPlayer.lanternaPlayer.naCaixa == true)
                {
                    PegarItem(LanternaPlayer.lanternaPlayer.lanterna);
                    pegouLanterna = true;
                    LanternaPlayer.lanternaPlayer.LigarLanterna();
                    LanternaPlayer.lanternaPlayer.naCaixa = false;
                }
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

