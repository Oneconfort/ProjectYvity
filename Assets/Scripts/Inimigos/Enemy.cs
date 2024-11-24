using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int life = 1;
    int dano = -1;

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Luz")){
            Destroy(this.gameObject);
        }
    }
    public virtual void Movimentacao(){

    }
    public void Ataque(){
       // GameController.controller.TomarDano();
        Destroy(this.gameObject);
    }
    public int GetDamage()
    {
        return dano;
    }
}
