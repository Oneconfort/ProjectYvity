using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacles : MonoBehaviour
{    
    //Eu criei essa clase abstrata caso a gente queira adicionar mais efeitos do tipo
    public float lifeTime;

    BoxCollider colider;
    private void Start()
    {
        colider = GetComponent<BoxCollider>();

        Invoke("TurnOff", lifeTime);
        Destroy(gameObject, lifeTime + 3);
    }
    public virtual void OnTriggerEnter(Collider other)
    {
      //Efeito do obstaculo
    }
    
    public virtual void SetLifeTime(float _lifeTime)
    {
        lifeTime = _lifeTime;
    }

    public virtual void TurnOff()
    {
        colider.enabled = false;
    }
}