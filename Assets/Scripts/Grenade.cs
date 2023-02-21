using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float maxTime=5;
    public float time = 5;
    public LayerMask targets;
    public float rad = 5;
    public float force = 1000;
    public float checkAngle = 15;
    public int rayCount = 360 ;
    Light lightComp;
    Vector3 testdir;
    ParticleSystem particle;
    
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
        if(time < 0)
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
            body.AddExplosionForce(force,transform.position,rad);
        }
        Destroy(gameObject,.5f);
        //enabled = false;
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

}
