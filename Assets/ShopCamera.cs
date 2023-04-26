using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement; 

public class ShopCamera : MonoBehaviour
{
    // Get the currently active camera
    public CinemachineVirtualCameraBase startCam;
    public CinemachineVirtualCameraBase currentCamera;
    void Start()
    {
        // assign the camera manager current camera to the new one in this scene
        //CameraManager.Instance.currentCamera = startCam; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
