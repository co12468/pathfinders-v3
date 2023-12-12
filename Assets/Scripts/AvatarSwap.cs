using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSwap : MonoBehaviour
{
    // Start is called before the first frame update
    public MyNetworkPlayer aPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aPlayer == null) { 
            aPlayer = GameObject.Find("PlayerPrefab(Clone)").GetComponent<MyNetworkPlayer>();
        }
    }

    public void swapAvatar(string url)
    {
        aPlayer.setAvatar(url);
    }
}
