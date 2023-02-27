using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GunSwing : MonoBehaviour
{
    public float resetStr;
    public Vector3 fxStr;
    Quaternion initalRot;
    // Start is called before the first frame update
    void Start()
    {
        initalRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = Vector3.zero;
        rot.y = Input.GetAxis("Mouse X") * fxStr.y;
        rot.z = Input.GetAxis("Mouse X") * fxStr.z;
        rot.x = -Input.GetAxis("Mouse Y")* fxStr.x;
        transform.localRotation = transform.localRotation * Quaternion.Euler(rot);


        transform.localRotation = Quaternion.Slerp(transform.localRotation, initalRot, Time.deltaTime * resetStr);

    }
}
