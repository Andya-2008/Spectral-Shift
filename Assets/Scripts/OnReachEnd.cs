using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class OnReachEnd : NetworkBehaviour
{
    bool hasReachedEnd;

    // Start is called before the first frame update
    void Start()
    {
        this.hasReachedEnd = false; 
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
        this.hasReachedEnd = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreasePlayersCompletedServerRpc()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().numOfPlayersCompleted++;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetPlayersCompletedServerRpc()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().numOfPlayersCompleted = 0;
    }

    [Rpc(SendTo.Everyone)]
    public void ResetPlayerRPC()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<FirstPersonController>().isMoving = true;
        GetComponent<FirstPersonController>().myCapsule.SetActive(true);
        this.hasReachedEnd = false;
    }

    public bool HasPlayerReachedEnd()
    {
        return this.hasReachedEnd;
    }

}
