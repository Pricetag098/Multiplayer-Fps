using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Grenade : NetworkBehaviour
{
    
    public float maxTime=5;
    public float damage = 100;
    public float time = 5;
    public LayerMask targets;
    public float rad = 5;
    public float force = 1000;
    public float checkAngle = 15;
    public int rayCount = 360 ;
    Light lightComp;
    Vector3 testdir;
    ParticleSystem particle;
    public PlayerData playerData;
    
    // Start is called before the first frame update
    void Start()
    {
        lightComp = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
        lightComp.intensity = Mathf.Sin(Time.time * 50 * (1-(time/maxTime)));
        time -= Time.deltaTime;
        if(time < 0 && isServer)
        {
            Explode();
            GetComponentInChildren<ParticleSystem>().Play();
            lightComp.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,rad,targets);
        List<Rigidbody> bodies = new List<Rigidbody>();
        foreach (Collider collider in colliders)
        {
            testdir = collider.transform.position - transform.position;
            for (int i = 0; i < rayCount; i++)
            {
                
                Vector3 rand = new Vector3((Random.value - .5f) * 2 * checkAngle, (Random.value - .5f) * 2 * checkAngle, (Random.value - .5f) * 2 * checkAngle);
                Vector3 rayDir = Quaternion.Euler(rand) * testdir;
                RaycastHit hit;
                
                if(Physics.Raycast(transform.position,rayDir, out hit, rad))
                {
                    Rigidbody targetBody = hit.collider.GetComponent<Rigidbody>();
                    if(targetBody == null)
					{
                        HitBox hb = hit.collider.gameObject.transform.GetComponent<HitBox>();
                        if(hb != null)
						{
                            targetBody = hb.health.GetComponent<Rigidbody>();
						}
					}
                    if(targetBody != null)
                    {
                        if (!bodies.Contains(targetBody))
                        {
                            bodies.Add(targetBody);
                        }
                    }
                }
                
            }
        }
        foreach(Rigidbody body in bodies)
        {
            Health h = body.GetComponent<Health>();
            if(h != null)
			{
                h.TakeDmg(damage * 1 / Mathf.Pow(Vector3.Distance(body.transform.position, transform.position),2),playerData);
			}
            NetworkIdentity id = body.GetComponent<NetworkIdentity>();
            if(id != null)
			{
                RpcKnockBack(id.connectionToClient);
			}
			else
			{
                body.AddExplosionForce(force, transform.position, rad);
            }
            //
        }
        StartCoroutine("Dest");
        //enabled = false;
    }

    [TargetRpc]
    void RpcKnockBack(NetworkConnection target)
	{
        target.identity.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, rad);

    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rad);
        Gizmos.DrawRay(transform.position, testdir);
        for(int i = 0; i < rayCount; i++)
        {
            Gizmos.color = Color.yellow;
            Vector3 rand = new Vector3((Random.value-.5f)*2*checkAngle, (Random.value - .5f) * 2 * checkAngle,(Random.value - .5f) * 2 * checkAngle);
            Vector3 rayDir = Quaternion.Euler(rand) * testdir;
            Gizmos.DrawRay(transform.position, rayDir);
        }
    }
    public IEnumerator Dest()
	{
        yield return new WaitForSeconds(.5f);
        NetworkServer.Destroy(gameObject);
    }

}
