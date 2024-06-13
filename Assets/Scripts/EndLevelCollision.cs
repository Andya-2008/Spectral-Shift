using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EndLevelCollision : MonoBehaviour
{
    [SerializeField]
    public int numOfPlayersCompleted = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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

            if (numOfPlayersCompleted == 1)
            {
                return;
            }
        }

        if (numOfPlayersCompleted >= 2)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().StartEndGameRPC();
            GameObject.Find("GameManager").GetComponent<GameManager>().LevelOverRPC();
            Debug.Log("Both players reached the end!");
        }

    }
}
