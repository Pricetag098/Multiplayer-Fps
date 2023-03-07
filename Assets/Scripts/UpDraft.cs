using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDraft : MonoBehaviour
{
    public float force,control;
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
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if(rb != null)
        {
            if(!rb.isKinematic)
            rigidbodies.Add(rb);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            rigidbodies.Remove(rb);
        }
    }
    private void FixedUpdate()
    {
        ///List<Rigidbody> list = new List<Rigidbody>(rigidbodies);
        
        for(int i = rigidbodies.Count -1; i > -1; i--)
        {
            Rigidbody rb = rigidbodies[i];
            if(rb != null)
            {
                rb.AddForce(transform.up * force * Time.fixedDeltaTime);
                //list.Add(rb);
            }
            else
            {
                rigidbodies.RemoveAt(i);
            }
            
            

            //Vector3 contDir = rb.position - transform.position;
            //contDir.y = 0;
            //rb.AddForce(-contDir.normalized * Mathf.Pow(Vector3.Distance(rb.transform.position,transform.position),2) * control * Time.fixedDeltaTime);
        }
        //rigidbodies = list;
    }

}
