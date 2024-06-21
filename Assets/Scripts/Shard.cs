using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShardMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if(collision.gameObject.tag == "MyPlayer" || collision.gameObject.tag == "Player")
            {
                GameObject.Find("GameManager").GetComponent<DeathManager>().OnDeath();
                GameObject.Find("ShardManager").GetComponent<Maze_ShardManager>().DestroyAllShardsRPC();
            }
        }
    }

    void ShardMove()
    {
        this.transform.Translate(moveSpeed * transform.right*100*Time.deltaTime);
    }
}
