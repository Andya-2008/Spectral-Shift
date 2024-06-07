using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Services.Lobbies.Models;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Canvas StartCanvas;
    [SerializeField] Canvas EndCanvas;
    [SerializeField] TextMeshProUGUI playerCountText;
    bool hasSpawned = false;
    int roomCount = 0;
    [SerializeField] GameObject mainObjectsParent;
    [SerializeField] public GameObject playersParent;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCountServerRPC();
        StartCanvas.GetComponent<Canvas>().enabled = true;
        DontDestroyOnLoad(mainObjectsParent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnNewPlayer();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        GameObject newPlayer = Instantiate(Player, new Vector3(0,0,0), Quaternion.identity);
        newPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, false);
    }

    public void SpawnNewPlayer()
    {
        if(!hasSpawned)
        {
            hasSpawned = true;
            SpawnPlayerServerRPC();
            StartCanvas.enabled = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateCountServerRPC()
    {
        roomCount++;
        UpdateCountRPC(roomCount);
    }
    [Rpc(SendTo.Everyone)]
    public void UpdateCountRPC(int roomCount)
    {
        playerCountText.gameObject.SetActive(true);
        playerCountText.text = "Players: " + roomCount.ToString();
    }

    public void StartEndGame()
    {
        EndCanvas.GetComponent<Canvas>().enabled = true;
    }
}