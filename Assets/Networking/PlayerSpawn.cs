using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] GameObject myCam;
    [SerializeField] GameObject myAudioListener;

    [SerializeField] private Camera color1Cam;
    [SerializeField] private Camera color2Cam;
    [SerializeField] private Camera color3Cam;

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
        DontDestroyOnLoad(this.gameObject);
        if(!this.GetComponent<NetworkObject>().IsOwner)
        {
            myCam.SetActive(false);
            GetComponent<FirstPersonController>().enabled = false;
            GetComponent<SwitchActiveCamera>().enabled = false;
            Debug.Log("IsNotOwner");
            Destroy(myAudioListener.gameObject);
            this.gameObject.tag = "Player";
        }
        else
        {
            GetComponent<FirstPersonController>().ChangeActiveCamera(myCam.GetComponent<Camera>());
            this.gameObject.tag = "MyPlayer";
            //GameObject.Find("Main Camera").SetActive(false);
            Debug.Log("IsOwner");
            Camera.main.gameObject.SetActive(false);
        }

        if (IsHost)
        {
            //Player 1 stuff
            color1Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Red Objects");
            color2Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Green Objects");
            color3Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Blue Objects");
        }
        else if (IsClient)
        {
            //Player 2 stuff
            color1Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Cyan Objects");
            color2Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Magenta Objects");
            color3Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Yellow Objects");
        }
    }
}
