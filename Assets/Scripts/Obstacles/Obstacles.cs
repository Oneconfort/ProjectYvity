using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacles : MonoBehaviour
{
    //Eu criei essa clase abstrata caso a gente queira adicionar mais efeitos do tipo
    float lifeTime;
    CapsuleCollider colider;
    public virtual void Start()
    {
        colider = GetComponent<CapsuleCollider>();
        lifeTime = SetLifetime();
        Invoke("TurnOff", lifeTime);
        Destroy(gameObject, lifeTime + 3);
    }
    public virtual void TurnOff()
    {
        colider.enabled = false;
    }
    public abstract void OnTriggerEnter(Collider other);

    public abstract float SetLifetime();

}
