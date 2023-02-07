using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100, maxHealth = 100;

    

    public void TakeDmg(float dmg)
    {
        health -= dmg;
        if(health < 0)
        {
            GetComponent<RagDoller>().RagDoll();
        }
    }
}
