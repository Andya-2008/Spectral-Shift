using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Maze_ShardManager : NetworkBehaviour
{
    [SerializeField] GameObject Shard;
    [SerializeField] List<string> colors = new List<string>();
    [SerializeField] float timeBetweenEachShard;
    [SerializeField] List<Transform> ShardPosList = new List<Transform>();
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        colors.Add("Red");
        //colors.Add("Blue");
        colors.Add("Cyan");
        //colors.Add("Cyan");
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            if (Time.time - startTime >= timeBetweenEachShard)
            {
                startTime = Time.time;
                int randomColorInt = Random.Range(0, colors.Count);
                int randomSpawnInt = Random.Range(0, ShardPosList.Count);
                SpawnShardRPC(randomColorInt, randomSpawnInt);
            }
        }
    }
    [Rpc(SendTo.Everyone)]
    public void SpawnShardRPC(int colorNum, int spawnNum)
    {
        GameObject newShard = Instantiate(Shard, ShardPosList[spawnNum].position, Quaternion.identity);
        Color ShardColor = Color.clear;
        ColorUtility.TryParseHtmlString(colors[colorNum], out ShardColor);
        newShard.GetComponent<MeshRenderer>().material.color = ShardColor;
        newShard.layer = LayerMask.NameToLayer(colors[colorNum] + " Objects");
    }

    [Rpc(SendTo.Server)]
    public void DestroyAllShardsRPC()
    {
        foreach (GameObject shard in GameObject.FindGameObjectsWithTag("Shard"))
        {
            Destroy(shard);
        }
    }
}