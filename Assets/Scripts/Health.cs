using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Health : NetworkBehaviour
{
    [SyncVar]public float health = 100, maxHealth = 100;
    [SerializeField] float respawnTimer = 5;
    [SerializeField]RagDoller ragDoller;
    
    [Command(requiresAuthority = false)]
    public void TakeDmg(float dmg)
    {
        
        health -= dmg;
        if(health < 0)
        {
            RpcDie();
            if(GetComponent<PlayerMove>()!=null)
            StartCoroutine("RespawnIe",GetComponent<NetworkIdentity>().connectionToClient);
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
    [ClientRpc]
    public void RpcRespawn()
    {
        if (ragDoller != null)
        {
            ragDoller.UnRagdoll();
        }
    }
    [TargetRpc]
    void RpcMoveToSpawn(NetworkConnection target)
    {
        transform.position = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)].position;
    }

    [TargetRpc]
    void RpcSetMv(NetworkConnection target,bool state)
	{
        GetComponent<PlayerMove>().enabled = state;
	}
    
    IEnumerator RespawnIe(NetworkConnection target)
    {
        RpcSetMv(target,false);
        yield return new WaitForSeconds(respawnTimer);
        RpcSetMv(target,true);
        health = maxHealth;
        RpcRespawn();
        RpcMoveToSpawn(target);
    }
}
