using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaShoot : Obstacles
{

    [SerializeField] float damage;

    public override void OnTriggerEnter(Collider other)
    {
        //Tomar dano = damage
        Debug.Log($"Tomei {damage} 6 de dano ");
    }

    public override float SetLifetime()
    {
        return 6;
    }
}
