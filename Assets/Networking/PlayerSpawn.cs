using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] GameObject myCam;
    [SerializeField] GameObject myAudioListener;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if(!this.GetComponent<NetworkObject>().IsOwner)
        {
            myCam.SetActive(false);
            GetComponent<FirstPersonController>().enabled = false;
            GetComponent<SwitchActiveCamera>().enabled = false;
            Debug.Log("IsNotOwner");
            Destroy(myAudioListener.gameObject);
        }
        else
        {
            //GameObject.Find("Main Camera").SetActive(false);
            Debug.Log("IsOwner");
        }
    }
}
