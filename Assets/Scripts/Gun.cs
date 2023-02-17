using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float dmg;
    public ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            particle.Play();
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.GetComponent<HitBox>())
                {
                    hit.collider.gameObject.GetComponent<HitBox>().OnHit(dmg);
                }
            }
        }
    }
}
