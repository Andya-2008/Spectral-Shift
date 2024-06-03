using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        Instantiate(Player, new Vector3(0,0,0), Quaternion.identity);
        Player.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkObjectId);
    }
}
