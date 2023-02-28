using MoveStates;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class Gun : MonoBehaviour
{
    public float dmg;
    public ParticleSystem particle;
    public Transform leftHandIk, rightHandIk;
    public PlayerData owner;
    [SerializeField] float fireRate =1,scopeSpeedFov=1,scopeSpeedPP =1,adsMvSpeed = 1;
    [SerializeField]Health health;
    [SerializeField] LayerMask validLayers;
    public Volume pp;
    float cooldown = 0;
    public Vector3 basePos,adsPos;

    // Start is called before the first frame update
    private void Start()
    {
        basePos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown-=Time.deltaTime;
        if (Input.GetMouseButtonDown(0)&&cooldown<= 0&& health.isAlive)
        {
            cooldown = fireRate;
            particle.Play();
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit,float.PositiveInfinity,validLayers))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.GetComponent<HitBox>())
                {
                    hit.collider.gameObject.GetComponent<HitBox>().OnHit(dmg,owner);
                }
                owner.CmdSpawnBulletTrails(particle.transform.position, hit.point);
            }
            else
            {
                owner.CmdSpawnBulletTrails(particle.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 1000);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 60/4, Time.deltaTime * scopeSpeedFov);
            if (pp != null)
            {
                pp.weight = Mathf.MoveTowards(pp.weight, 1, Time.deltaTime * scopeSpeedPP);
                if (pp.weight > .8)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            
            MoveState.sensitivity = 0.5f;
            transform.localPosition = Vector3.Slerp(transform.localPosition, adsPos,Time.deltaTime * adsMvSpeed);
            
        }
        if (!Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 60, Time.deltaTime * scopeSpeedFov);
            if(pp != null)
            {
                pp.weight = Mathf.MoveTowards(pp.weight, 0, Time.deltaTime * scopeSpeedPP);
                if (pp.weight < .8)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
           
            MoveState.sensitivity = 1f;
            transform.localPosition = Vector3.Slerp(transform.localPosition, basePos, Time.deltaTime * adsMvSpeed);
            
            
        }
    }
}
