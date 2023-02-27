using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Health : NetworkBehaviour
{
    [SyncVar]public float health = 100, maxHealth = 100;
    [SerializeField] float respawnTimer = 5;
    [SerializeField]RagDoller ragDoller;
    public bool isAlive { get
        {
            return health > 0;
        } }

    PlayerData data;
    private void Start()
    {
        data = GetComponent<PlayerData>();
    }
    [Command(requiresAuthority = false)]
    public void TakeDmg(float dmg,PlayerData source)
    {
        
        health -= dmg;
        if(health < 0)
        {
            if(data != null)
            {
                source.kills++;
                data.deaths++;
            }
            
            RpcDie();
            if (GetComponent<PlayerData>())
            {
                RpcDeathScreen(GetComponent<NetworkIdentity>().connectionToClient, source);

                RpcUpdateKillfeed(source, GetComponent<PlayerData>());
                if (GetComponent<PlayerMove>() != null)
                    StartCoroutine("RespawnIe", GetComponent<NetworkIdentity>().connectionToClient);
            }
                
        }
        
    }
    [ClientRpc]
    public void RpcUpdateKillfeed(PlayerData killer, PlayerData dead)
    {
        NetManager netManager = (NetManager)NetworkManager.singleton;
        netManager.playerUi.killFeed.OnKill(killer, dead);
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)].position;
    }
    [TargetRpc]
    void RpcDeathScreen(NetworkConnection target,PlayerData data)
    {
        NetManager netManager = (NetManager)NetworkManager.singleton;
        netManager.playerUi.respawnScreen.gameObject.SetActive(true);
        netManager.playerUi.respawnScreen.OnDie(respawnTimer,data.playerNameStr);
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
