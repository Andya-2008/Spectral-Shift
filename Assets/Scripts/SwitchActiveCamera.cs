using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActiveCamera : MonoBehaviour
{
    private Camera color1Cam;
    private Camera color2Cam;
    private Camera color3Cam;

    public GameObject color1CamObject;
    public GameObject color2CamObject;
    public GameObject color3CamObject;

    void Start()
    {
        color1Cam = color1CamObject.GetComponent<Camera>();
        color2Cam = color2CamObject.GetComponent<Camera>();
        color3Cam = color3CamObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (color1CamObject.activeSelf == true)
            {
                color1CamObject.SetActive(false);
                color3CamObject.SetActive(false);
                color2CamObject.SetActive(true);
                GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeActiveCamera(color2Cam);
            }

            else if (color2CamObject.activeSelf == true)
            {
                color1CamObject.SetActive(false);
                color2CamObject.SetActive(false);
                color3CamObject.SetActive(true);
                GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeActiveCamera(color3Cam);
            }

            else
            {
                color1CamObject.SetActive(true);
                color3CamObject.SetActive(false);
                color2CamObject.SetActive(false);
                GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeActiveCamera(color1Cam);
            }
            
        }
    }
}
