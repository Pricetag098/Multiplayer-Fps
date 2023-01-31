using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDraft : MonoBehaviour
{
    public float force;
    List<Rigidbody> rigidbodies = new List<Rigidbody>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rigidbodies.Add(rb);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rigidbodies.Remove(rb);
        }
    }
    private void FixedUpdate()
    {
        foreach(Rigidbody rb in rigidbodies)
        {
            rb.AddForce(transform.up * force * Time.fixedDeltaTime);
        }
    }

}
