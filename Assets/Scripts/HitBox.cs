using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float multi = 1;
    
    Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<Health>();
    }
    public void OnHit(float dmg,PlayerData source)
    {
        health.TakeDmg(dmg * multi,source);
    }
}
