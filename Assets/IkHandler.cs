using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkHandler : MonoBehaviour
{
    public PlayerAnims animSettings;
    Animator anim;
    Gun gun;
    // Start is called before the first frame update
    void Start()
    {
        gun = animSettings.gun;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandIk.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, gun.rightHandIk.position);
    }
}
