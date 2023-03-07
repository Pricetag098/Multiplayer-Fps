using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GrenadeThrow : NetworkBehaviour
{
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] float spawnOffset;
    [SerializeField] float throwVel;
    [SerializeField] float maxTime;
    Rigidbody rb;
    float time;
    bool holding;
    public Transform cam;
    NetworkManager server;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		if (isLocalPlayer)
		{
            //cam = Camera.main.transform;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (isLocalPlayer)
		{
            if (Input.GetKeyDown(KeyCode.G) && !holding)
            {
                holding = true;
                time = maxTime;

            }
            if (holding)
            {
                time -= Time.deltaTime;
                if (Input.GetKeyUp(KeyCode.G) || time < 0)
                {
                    //server.
                    //Debug.Log(cam);
                    SpawnGrenade(time,cam.position,cam.forward,rb.velocity);
                    holding = false;
                }
            }
        }
       
    }
    [Command]
    void SpawnGrenade(float t,Vector3 origin, Vector3 dir,Vector3 vel)
	{
        //Debug.Log(origin);
        
        GameObject grenadeGo = Instantiate(grenadePrefab);
        grenadeGo.transform.position = origin + dir * spawnOffset;
        grenadeGo.GetComponent<Rigidbody>().velocity = dir * throwVel + vel * .25f;
        Grenade g = grenadeGo.GetComponent<Grenade>();
        g.maxTime = maxTime;
        g.time = t;
        g.playerData = GetComponent<PlayerData>();
        NetworkServer.Spawn(grenadeGo,gameObject);


    }
}
