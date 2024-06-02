using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActiveCamera : MonoBehaviour
{
    private Camera redViewCamera;
    private Camera blueViewCamera;

    public GameObject redViewCameraObject;
    public GameObject blueViewCameraObject;

    void Start()
    {
        redViewCamera = redViewCameraObject.GetComponent<Camera>();
        blueViewCamera = blueViewCameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (redViewCameraObject.activeSelf == true)
            {
                redViewCameraObject.SetActive(false);
                blueViewCameraObject.SetActive(true);
                GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeActiveCamera(blueViewCamera);
                Debug.Log("Switched to blue camera");
            }

            else if (blueViewCameraObject.activeSelf == true)
            {
                blueViewCameraObject.SetActive(false);
                redViewCameraObject.SetActive(true);
                GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeActiveCamera(redViewCamera);
                Debug.Log("Switched to red camera");
            }
            
        }
    }
}
