using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeFalsa : MonoBehaviour
{
    
   
    void Update()
    {
        if (LanternaPlayer.lanternaPlayer.paredeDetectada == true)
        {
           Destroy(gameObject, 0);
        }
    }
}
