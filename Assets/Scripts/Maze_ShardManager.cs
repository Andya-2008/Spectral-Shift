using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Maze_ShardManager : MonoBehaviour
{
    [SerializeField] GameObject Shard;
    [SerializeField] List<string> colors = new List<string>();
    [SerializeField] float timeBetweenEachShard;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        colors.Add("Red");
        colors.Add("Blue");
        colors.Add("Yellow");
        colors.Add("Cyan");
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time - startTime >= timeBetweenEachShard)
        {
            /*
            if ()
            {

            }
            */
        }
    }
}
