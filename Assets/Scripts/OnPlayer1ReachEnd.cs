using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnPlayer1ReachEnd : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Rpc(SendTo.Everyone)]
    public void Player1ReachedEndRpc()
    {
        GameObject.Find("EndingDoorPlayer1").SetActive(true);
    }
}
