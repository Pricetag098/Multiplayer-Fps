using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoller : MonoBehaviour
{
    Rigidbody[] rbs;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        foreach(Rigidbody rb in rbs)
		{
            rb.isKinematic = true;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Do")]
    public void RagDoll()
	{
        animator.enabled = false;
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }
    }
}
