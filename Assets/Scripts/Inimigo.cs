using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{

    public int danoInimigo = -1;

   /* private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
        }
    }*/

    public int GetDamage()
    {
        return danoInimigo;
    }
}
