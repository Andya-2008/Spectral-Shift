using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if(collision.gameObject.tag == "MyPlayer" || collision.gameObject.tag == "Player")
            {
                //OnDeath
            }
        }
    }
}
