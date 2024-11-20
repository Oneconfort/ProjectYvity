using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : Obstacles
{
    //O vento adiciona uma força no player e joga ele longe

    [SerializeField] float force;
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rigidbody = other?.GetComponent<Rigidbody>();

            rigidbody.AddForce(0, 0, force, ForceMode.Impulse);
        }
    }
}
