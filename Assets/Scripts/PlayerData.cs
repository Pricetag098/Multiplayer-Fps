using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class PlayerData : NetworkBehaviour
{
    public GameObject clientBody;
    public GameObject serverBody;
    public GameObject clientUi;
    public GameObject cam;
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
        pm.cam.GetComponent<Camera>().enabled = isLocalPlayer;
        pm.cam.GetComponent<AudioListener>().enabled = isLocalPlayer;
        pm.cam.parent = null;
        pm.cam.gameObject.name = gameObject.name + "Cam";
        if (isLocalPlayer)
        {
            transform.position = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)].position;
        }
        //GetComponent<GrenadeThrow>().enabled = isLocalPlayer;
        clientBody.SetActive(isLocalPlayer);
        clientUi.SetActive(isLocalPlayer);
        serverBody.SetActive(!isLocalPlayer);
        gun.enabled = isLocalPlayer;
        if (isLocalPlayer)
        {
            gun.EnablePP();
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
		Destroy(cam);
        if (isServer)
        {
            Debug.Log("A");
            //NetManager.singleton.playerManager.players.Remove(this);
        }
	}
    

}
