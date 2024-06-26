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
    public List<GameObject> PlayerList = new List<GameObject>();

    [SerializeField] GameObject mainObjectsParent;
    [SerializeField] public GameObject playersParent;
    [SerializeField] Canvas EndCanvas;

    //Make spawn point in each scene
    public Transform StartingPos1;
    public Transform StartingPos2;

    [SerializeField] GameObject shardManager; 

    public static bool isLevel1Completed = false;
    public static bool isLevel2Completed = false;
    public static bool isLevel3Completed = false;
    public static bool isLevel4Completed = false;
    public static bool isLevel5Completed = false;
    public static bool isLevel6Completed = false;

    [SerializeField]
    public int numOfPlayersCompleted = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Setting all of the serializables
        mainObjectsParent = GameObject.Find("MainObjects");
        playersParent = GameObject.Find("Players");
        GameObject.Find("EndingDoor").GetComponent<EndLevelCollision>().gameManager = GetComponent<GameManager>();
        EndCanvas = GameObject.Find("LevelOverCanvas").GetComponent<Canvas>();
        //
        DontDestroyOnLoad(this);

        
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

        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach(GameObject player in PlayerList)
        {
            player.GetComponent<FirstPersonController>().enabled = true;
        }
        SetNewSpawn();
        if (NetworkManager.Singleton.IsHost)
        {
            SpawnEveryoneRPC();
        }

        if (IsHost && isLevel1Completed && !isLevel2Completed)
        {
            GameObject newShardManager = Instantiate(shardManager, new Vector3(0, 0, 0), Quaternion.identity);
            newShardManager.GetComponent<NetworkObject>().Spawn();
        }
    }

    //Call here when the game is done
    [Rpc(SendTo.Everyone)]
    public void StartEndGameRPC()
    {
        foreach(GameObject player in PlayerList)
        {
            player.GetComponent<FirstPersonController>().enabled = false;
        }
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
        Debug.Log("Run LevelOverRPC");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        EndCanvas.GetComponent<Canvas>().enabled = false;

        foreach (GameObject player in PlayerList)
        {
            player.GetComponent<OnReachEnd>().ResetPlayerRPC();
        }
    }

    public void SetNewSpawn()
    {
        if (GameObject.Find("StartingPos1").transform != null)
        {
            StartingPos1 = GameObject.Find("StartingPos1").transform;
        }

        else
        {
            Debug.LogWarning("Player 1's starting position could not be found!");
        }

        if (GameObject.Find("StartingPos2").transform != null)
        {
            StartingPos2 = GameObject.Find("StartingPos2").transform;
        }

        else
        {
            Debug.LogWarning("Player 2's starting position could not be found!");
        }
    }

    [Rpc(SendTo.Everyone)]
    public void SpawnEveryoneRPC()
    {
        foreach(GameObject player in PlayerList)
        {
            player.GetComponent<FirstPersonController>().isMoving = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if(player.GetComponent<PlayerSpawn>().player1)
            {
                player.transform.position = StartingPos1.position;
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