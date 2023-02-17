using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkedPlayer : NetworkBehaviour
{
    public GameObject clientBody;
    public GameObject serverBody;
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = !isLocalPlayer;
        PlayerMove pm = GetComponent<PlayerMove>();
        pm.enabled = isLocalPlayer;
        //pm.cam = isLocalPlayer? Camera.main.transform : null;
        pm.cam.GetComponent<Camera>().enabled = isLocalPlayer;
        pm.cam.parent = null;
        pm.cam.gameObject.name = gameObject.name + "Cam";
        GetComponent<GrenadeThrow>().enabled = isLocalPlayer;
        clientBody.SetActive(isLocalPlayer);
        serverBody.SetActive(!isLocalPlayer);
        
    }
	private void OnDestroy()
	{
		Destroy(cam);
	}


}
