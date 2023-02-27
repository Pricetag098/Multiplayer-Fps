using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillFeedText : MonoBehaviour
{
    public float decayoffset = 5;
    public float decayRate;
    float lifetime;
    CanvasGroup group;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime * decayRate;
        if(lifetime < 0)
            gameObject.SetActive(false);
        group.alpha = lifetime;
    }

    public void Spawn(PlayerData killer, PlayerData dead)
    {
        lifetime = decayoffset;
        text = GetComponent<TextMeshProUGUI>();
        text.text = killer.playerNameStr + " > " + dead.playerNameStr;
    }
}
