using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    //Esse script instancia os efeitos de vento
    //Pra usar e só colocar o prefab na cena, e colocar os spanws(Empty) onde você quer instanciar o vento
    //Alem dos spawns não presisa mexer em mais nada, FUNCIONA!!!
    //Da para otimizar melhor para instanciar com corotina, vou fazer depois to com preguiça

    // Cloquei o cooldown pra ser aleatorio
    [SerializeField] private GameObject effect;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private float cooldown;
   
    void Start()
    {
        InvokeRepeating("Spawn", 1, cooldown);
    }


    void Spawn()
    {
        for (int i = 0; spawns.Length > i; i++)
        {
            Instantiate(effect, spawns[i].transform.position, spawns[i].transform.rotation);
        }
        float cooldown = Random.Range(15, 20);
    }


}
