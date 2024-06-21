using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class OnReachEnd : NetworkBehaviour
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
    public void ReachedEndRpc()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        GetComponent<FirstPersonController>().isMoving = false;
        GetComponent<FirstPersonController>().myCapsule.SetActive(false);
        
        Debug.Log("The number of players completed increased by 1");
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreasePlayersCompletedServerRpc()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().numOfPlayersCompleted++;
    }

}
