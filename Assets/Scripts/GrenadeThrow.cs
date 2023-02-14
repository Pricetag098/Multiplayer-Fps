using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] float spawnOffset;
    [SerializeField] float throwVel;
    [SerializeField] float maxTime;
    Rigidbody rb;
    float time;
    bool holding;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !holding)
        {
            holding = true;
            time = maxTime;
        }
        if (holding)
        {
            time -= Time.deltaTime;
            if(Input.GetKeyUp(KeyCode.G)|| time < 0)
            {
                GameObject grenadeGo = Instantiate(grenadePrefab);
                grenadeGo.transform.position = Camera.main.transform.position + Camera.main.transform.forward * spawnOffset;
                grenadeGo.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwVel + rb.velocity * .25f;
                Grenade g = grenadeGo.GetComponent<Grenade>();
                g.maxTime = maxTime;
                g.time = time;
                holding = false;
            }
        }
    }
}
