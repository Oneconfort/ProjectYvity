using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteracaoComItem : MonoBehaviour
{
    public static InteracaoComItem interacaoComItem;
    float dropForce = 3.0f;
    public GameObject holdPosition;
    GameObject heldItem;
    private Rigidbody heldItemRigidbody;
    public GameObject bauInteracao;

    //tirolesa
    public Transform startPoint;
    public Transform endPoint;
    private bool isZipping = false;


    void Awake()
    {
        if (interacaoComItem == null)
        {
            interacaoComItem = this;
        }
    }

    public void InteracaoCenario()
    {
        string[] tags = { "ObjetoI", "Bau", "Tirolesa" }; 
        List<GameObject> todosOsObjetos = new List<GameObject>();

        foreach (string tag in tags)
        {
            GameObject[] objetosComTag = GameObject.FindGameObjectsWithTag(tag);
            todosOsObjetos.AddRange(objetosComTag);
        }

        foreach (GameObject alvo in todosOsObjetos)
        {
            Vector3 dir = alvo.transform.position - transform.position;
            float dot = Vector3.Dot(dir.normalized, transform.forward);

            if (dot > 0.5f)
            {
                if (dir.magnitude < 3)
                {
                    switch (alvo.tag)
                    {
                        case "ObjetoI":
                            interacaoComItem.PickUpItem(alvo);
                            break;
                        case "Bau":
                            bauInteracao = alvo;
                            if (bauInteracao != null)
                            {
                                bauInteracao.SendMessage("SpawnItemBau", SendMessageOptions.DontRequireReceiver);
                                bauInteracao = null;
                            }
                            break;
                        case "Tirolesa":
                            if (!isZipping)
                            {
                                transform.position = gameObject.transform.position;
                                StartCoroutine(ZiplineMovement());
                            }
                            isZipping = false;
                            break;
                    }
                }
            }
        }
    }


    public void PickUpItem(GameObject item)
    {
        heldItem = item;
        heldItemRigidbody = heldItem.GetComponent<Rigidbody>();

        heldItemRigidbody.isKinematic = true;
        heldItem.transform.SetParent(holdPosition.transform);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItemRigidbody.isKinematic = false;
            heldItemRigidbody.AddForce(holdPosition.transform.forward * dropForce, ForceMode.Impulse);
            heldItem = null;
        }

    }

    IEnumerator ZiplineMovement()
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

