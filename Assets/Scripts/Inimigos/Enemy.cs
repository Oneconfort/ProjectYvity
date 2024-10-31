using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int dano;
    public virtual void Movimentacao(){

    }

    public void Ataque(){
        GameController.controller.TomarDano();
        Destroy(this.gameObject);
    }
}
