using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class EndLevelCollision : MonoBehaviour
{

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("GameManager") != null)
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name + " collided with the end");

        if (collision.collider.tag == "Player" || collision.collider.tag == "MyPlayer")
        {
            Debug.Log("A player reached the end");
            collision.collider.gameObject.GetComponent<OnReachEnd>().ReachedEndRpc();
            collision.collider.gameObject.GetComponent<OnReachEnd>().IncreasePlayersCompletedServerRpc();

            if (gameManager.numOfPlayersCompleted == 1)
            {
                Debug.Log("The first player reached the end!");
                return;
            }
        }

        if (gameManager.numOfPlayersCompleted >= 2 && NetworkManager.Singleton.IsHost)
        {
            gameManager.GetComponent<GameManager>().StartEndGameRPC();
        }

    }
}
