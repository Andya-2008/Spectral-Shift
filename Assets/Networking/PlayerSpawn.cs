using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] GameObject myCam;
    [SerializeField] GameObject myAudioListener;

    [SerializeField] private Camera color1Cam;
    [SerializeField] private Camera color2Cam;
    [SerializeField] private Camera color3Cam;

    public bool player1;
    public bool player2;
    public float startTime;
    [SerializeField] float timeSinceDeath = 2f;
    public bool cantMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cantMove)
        {
            if(Time.time - startTime >= timeSinceDeath)
            {
                cantMove = false;
                ResetControlRPC();
            }
        }
    }
    [Rpc(SendTo.Everyone)]
    public void ResetControlRPC()
    {

        GetComponent<FirstPersonController>().isMoving = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
    

    public override void OnNetworkSpawn()
    {
        DontDestroyOnLoad(this.gameObject);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList.Add(this.gameObject);
        if(!this.GetComponent<NetworkObject>().IsOwner)
        {
            myCam.SetActive(false);
            GetComponent<FirstPersonController>().enabled = false;
            GetComponent<SwitchActiveCamera>().enabled = false;
            Destroy(myAudioListener.gameObject);
            this.gameObject.tag = "Player";
        }
        else
        {
            GetComponent<FirstPersonController>().ChangeActiveCamera(myCam.GetComponent<Camera>());
            this.gameObject.tag = "MyPlayer";
            //GameObject.Find("Main Camera").SetActive(false);
            Camera.main.gameObject.SetActive(false);
        }

        if (IsHost)
        {
            //Player 1 stuff
            player1 = true;
            GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>().text = "Player 1";
            color1Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Red Objects");
            color2Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Green Objects");
            color3Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Blue Objects");
        }
        else if (IsClient)
        {
            //Player 2 stuff
            player2 = true;
            GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>().text = "Player 1";
            color1Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Cyan Objects");
            color2Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Magenta Objects");
            color3Cam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Yellow Objects");
        }
    }
}