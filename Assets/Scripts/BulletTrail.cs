using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    PooledObj obj;
    public float time;
    //Gradient g;
    //LineRenderer lr;
   public  float age;

    // Start is called before the first frame update
    void Start()
    {
        obj = GetComponent<PooledObj>();
        //lr = GetComponent<LineRenderer>();
        //g = lr.colorGradient;
        
    }
    public void Spawn()
    {
        age = time;
    }
    // Update is called once per frame
    void Update()
    {
        
        if(age < 0)
        {
            obj.Despawn();
        }
        age -= Time.deltaTime;

    }
}
