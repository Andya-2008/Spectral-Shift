using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject Player;
    public List<GameObject> PlayerList = new List<GameObject>();

    [SerializeField] Canvas StartCanvas;
    [SerializeField] Canvas EndCanvas;
    [SerializeField] TextMeshProUGUI playerCountText;
    bool hasSpawned = false;
    int roomCount = 0;
    [SerializeField] GameObject mainObjectsParent;
    [SerializeField] public GameObject playersParent;

    //Make spawn point in each scene
    public Transform StartingPos1;
    public Transform StartingPos2;

    public static bool isLevel1Completed = true;
    public static bool isLevel2Completed = false;
    public static bool isLevel3Completed = false;
    public static bool isLevel4Completed = false;
    public static bool isLevel5Completed = false;
    public static bool isLevel6Completed = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCountServerRPC();
        StartCanvas.GetComponent<Canvas>().enabled = true;
        DontDestroyOnLoad(mainObjectsParent.gameObject);
        SetNewSpawn();
        if (NetworkManager.Singleton.IsHost)
        {
            SpawnEveryoneRPC();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartEndGameRPC();
        }
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

    //Call here when the game is done
    [Rpc(SendTo.Everyone)]
    public void StartEndGameRPC()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EndCanvas.GetComponent<Canvas>().enabled = true;

        // Checks if the level just completed is level 1
        if (SceneManager.GetActiveScene().name.Equals("Level1"))
        {
            isLevel1Completed = true;
        }

        else if (SceneManager.GetActiveScene().name.Equals("Level2"))
        {
            isLevel2Completed = true;
        }

        else if (SceneManager.GetActiveScene().name.Equals("Level3"))
        {
            isLevel3Completed = true;
        }

        else if (SceneManager.GetActiveScene().name.Equals("Level4"))
        {
            isLevel4Completed = true;
        }

        else if (SceneManager.GetActiveScene().name.Equals("Level5"))
        {
            isLevel5Completed = true;
        }

        else if (SceneManager.GetActiveScene().name.Equals("Level6"))
        {
            isLevel6Completed = true;
        }
    }

    [Rpc(SendTo.Everyone)]
    public void LevelOverRPC()
    {
        Debug.Log("the function has indeed ran");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        EndCanvas.GetComponent<Canvas>().enabled = false;
        Debug.Log("the function has indeed reached the end of its lifetime");
        SetNewSpawn();
        SpawnEveryoneRPC();
    }
    public void SetNewSpawn()
    {
        StartingPos1 = GameObject.Find("StartingPos1").transform;
        StartingPos2 = GameObject.Find("StartingPos2").transform;
    }

    [Rpc(SendTo.Everyone)]
    public void SpawnEveryoneRPC()
    {
        Debug.Log("1");
        foreach(GameObject player in PlayerList)
        {
            player.GetComponent<FirstPersonController>().isMoving = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if(player.GetComponent<PlayerSpawn>().player1)
            {
                player.transform.position = StartingPos1.position;
                Debug.Log(player.transform.position + " : " + StartingPos1.position);
            }
            else if(player.GetComponent<PlayerSpawn>().player2)
            {
                player.transform.position = StartingPos2.position;
            }
            player.transform.rotation = Quaternion.identity;
            player.GetComponent<PlayerSpawn>().startTime = Time.time;
            player.GetComponent<PlayerSpawn>().cantMove = true;
        }
    }
}