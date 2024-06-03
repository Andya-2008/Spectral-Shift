using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject Player;
    bool hasSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && !hasSpawned)
        {
            hasSpawned = true;
            SpawnPlayerServerRPC();
        }
    }
    [ServerRpc]
    public void SpawnPlayerServerRPC()
    {
        Instantiate(Player, new Vector3(0,0,0), Quaternion.identity);
        Player.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkObjectId);
        Camera.main.gameObject.SetActive(false);
    }
}
