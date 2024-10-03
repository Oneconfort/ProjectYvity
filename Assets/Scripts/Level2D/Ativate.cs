using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ativate : MonoBehaviour
{
    public GameObject objectToDrop;
    private GameObject currentObject; 

    private void Start()
    {
        SpawnObject(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentObject != null)
        {
            Rigidbody rb = currentObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; 
                Invoke("SpawnObject", 7f); 
                Destroy(currentObject, 7f); 
            }
        }
    }

    private void SpawnObject()
    {
        currentObject = Instantiate(objectToDrop, transform.position, Quaternion.identity);


    }
}