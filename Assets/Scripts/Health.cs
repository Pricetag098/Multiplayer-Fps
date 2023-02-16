using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Health : NetworkBehaviour
{
    [SyncVar]public float health = 100, maxHealth = 100;
    [SerializeField]RagDoller ragDoller;
    
    [Command(requiresAuthority = false)]
    public void TakeDmg(float dmg)
    {
        
        health -= dmg;
        if(health < 0)
        {
            RpcDie();
        }
    }
    [ClientRpc]
    public void RpcDie()
	{
        if (ragDoller != null)
		{
            ragDoller.RagDoll();
		}
    }
}
