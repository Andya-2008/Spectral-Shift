using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.GetComponent<OnReachEnd>().HasPlayerReachedEnd());
        if ((other.gameObject.tag == "MyPlayer" || other.gameObject.tag == "Player") && !other.gameObject.GetComponent<OnReachEnd>().HasPlayerReachedEnd())
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<DeathManager>().OnDeath();
        }
    }
}
