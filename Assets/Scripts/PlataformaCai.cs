using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlataformaCai : MonoBehaviour
{
    public float delay = 1.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("DerrubarPlataforma", delay);
        }
    }

    void DerrubarPlataforma()
    {
        rb.isKinematic = false; 
    }
}