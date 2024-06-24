using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerSpawn : NetworkBehaviour
{
    [SerializeField] GameObject gameManager;
    bool hasSpawned = false;
    [SerializeField] GameObject Player;
    [SerializeField] Canvas StartCanvas;
    [SerializeField] TextMeshProUGUI playerCountText;
    int roomCount = 0;

    // Start is called before the first frame update
    void Start()
    {

        StartCanvas = GameObject.Find("StartCanvas").GetComponent<Canvas>();
        playerCountText = GameObject.Find("playerCountText").GetComponent<TextMeshProUGUI>();

        UpdateCountServerRPC();


        StartCanvas.GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRPC(ServerRpcParams serverRpcParams = default)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (!GameObject.FindGameObjectWithTag("GameManager"))
            {
                Debug.Log("Has run SpawnPlayerServerRPC on server");
                GameObject newGameManager = Instantiate(gameManager, new Vector3(0, 0, 0), Quaternion.identity);
                newGameManager.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, false);
            }
            GameObject newPlayer = Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
            newPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, false);
        }
        else
        {
            Debug.Log("Has run SpawnPlayerServerRPC on client");
        }
    }

    public void SpawnNewPlayer()
    {
        Debug.Log("1");
        if (!hasSpawned)
        {
            Debug.Log("2");
            hasSpawned = true;
            SpawnPlayerServerRPC();
            StartCanvas.enabled = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateCountServerRPC()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("I am the server: UpdateCountServerRPC");
            roomCount++;
            UpdateCountRPC(roomCount);
        }
        else
        {
            Debug.Log("I am the client: UpdateCountServerRPC not running");
        }
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateCountRPC(int roomCount)
    {
        Debug.Log("UpdatingCount");
        playerCountText.gameObject.SetActive(true);
        playerCountText.text = "Players: " + roomCount.ToString();
    }
}
