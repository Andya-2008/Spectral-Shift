using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class EndLevelCollision : MonoBehaviour
{

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" || collision.collider.tag == "MyPlayer")
        {
            collision.collider.gameObject.GetComponent<OnReachEnd>().ReachedEndRpc();

            if (SceneManager.GetActiveScene().name == "Level1")
            {
                if (collision.collider.gameObject.GetComponent<PlayerSpawn>().player1)
                {
                    collision.collider.gameObject.GetComponent<OnPlayer1ReachEnd>().Player1ReachedEndRpc();
                }
            }


            if (gameManager.numOfPlayersCompleted == 1)
            {
                return;
            }
        }

        if (gameManager.numOfPlayersCompleted >= 2)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().StartEndGameRPC();
            Debug.Log("Both players reached the end!");
        }

    }
}
