using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RespawnScreen : MonoBehaviour
{
    public Image spawn;
    public TextMeshProUGUI text;
    float time, maxTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawn.fillAmount = time / maxTime;
        time-=Time.deltaTime;
        if(time < 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void OnDie(float respawnTime, string killerName)
    {
        text.text = killerName;
        maxTime = respawnTime;
        time = maxTime;
    }
}
