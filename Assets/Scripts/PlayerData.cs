using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class PlayerData : NetworkBehaviour
{
    public GameObject clientBody;
    public GameObject serverBody;
    public PlayerUi clientUi;
    public GameObject clientCam;
    public GameObject serverCam;
    public Gun gun;
    public TextMeshProUGUI playerName;
    public string playerNameStr;
    [SyncVar] public int kills = 0;
    [SyncVar] public int deaths = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = !isLocalPlayer;
        PlayerMove pm = GetComponent<PlayerMove>();
        pm.enabled = isLocalPlayer;
        //pm.cam = isLocalPlayer? Camera.main.transform : null;
        //pm.cam.GetComponent<Camera>().enabled = isLocalPlayer;
        //pm.cam.GetComponent<AudioListener>().enabled = isLocalPlayer;
        clientCam.SetActive(isLocalPlayer);
        serverCam.SetActive(!isLocalPlayer);
        pm.cam.parent = null;
        pm.cam.gameObject.name = gameObject.name + " Cam";
        if (isLocalPlayer)
        {
            transform.position = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)].position;
        }
        //GetComponent<GrenadeThrow>().enabled = isLocalPlayer;
        clientBody.SetActive(isLocalPlayer);
        NetManager netManager = (NetManager)NetworkManager.singleton;
        clientUi = netManager.playerUi;
        //clientUi.gameObject.SetActive(isLocalPlayer);
        
        serverBody.SetActive(!isLocalPlayer);
        gun.enabled = isLocalPlayer;
        if (isLocalPlayer)
        {
            gun.pp = clientUi.scopePp;
            clientUi.healthBar.health = GetComponent<Health>();
        }
        CmdGetName();
        if (isServer)
        {
            //NetManager.singleton.playerManager.players.Add(this);
            
        }
    }
    [Command(requiresAuthority = false)]
    void CmdGetName()
	{
        RpcGetName(playerNameStr);
	}
    [ClientRpc]
    void RpcGetName(string nme)
	{
        playerNameStr = nme;
        playerName.text = playerNameStr;
	}
	private void OnDestroy()
	{
        if(GetComponent<PlayerMove>().cam)
		Destroy(GetComponent<PlayerMove>().cam.gameObject);
        if (isServer)
        {
            Debug.Log("A");
            //NetManager.singleton.playerManager.players.Remove(this);
        }
	}
    

}
