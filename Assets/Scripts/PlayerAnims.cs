using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerAnims : NetworkBehaviour
{
    [SyncVar]
    Vector3 vel;
    public Gun gun;
    public Animator animator;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            vel = rb.velocity;
        }
        animator.SetFloat("VelFwd", Vector3.Dot(transform.forward, vel));
        animator.SetFloat("VelSide", Vector3.Dot(transform.right, vel));
    }
}
