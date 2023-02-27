using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class KillFeed : MonoBehaviour
{
   
    [SerializeField] int poolSize = 10;
    [SerializeField] GameObject textGo;
    //GameObject[] textObjs;
    VerticalLayoutGroup layout;
    private void Start()
    {
        
            for (int i = 0; i < poolSize; i++)
            {
                GameObject newGo = Instantiate(textGo, transform);
                newGo.SetActive(false);
            }
        
        
        
    }
    
    //[ClientRpc]
    public void OnKill(PlayerData killer,PlayerData dead)
    {
        GameObject obj = transform.GetChild(transform.childCount-1).gameObject;
        obj.GetComponent<KillFeedText>().Spawn(killer, dead);
        obj.SetActive(true);
        obj.transform.SetAsFirstSibling();
        
    }
}
