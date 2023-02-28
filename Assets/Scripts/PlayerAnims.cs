using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerAnims : NetworkBehaviour
{
    [SyncVar]
    Vector3 vel;
    [SyncVar]
    public bool crouch;
    public Gun gun;
    public Animator animator;
    Rigidbody rb;
    PlayerMove mv;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mv = GetComponent<PlayerMove>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            vel = rb.velocity;
            crouch = mv.UseCrouchAnim();
        }
        animator.SetFloat("VelFwd", Vector3.Dot(transform.forward, vel));
        animator.SetFloat("VelSide", Vector3.Dot(transform.right, vel));
        animator.SetBool("Crouched", crouch);
    }
}
