using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera townCamera;
    public CinemachineVirtualCamera battleCamera;
    public CinemachineVirtualCamera buildCamera;
    public float panSpeed = 5f;

    private CinemachineVirtualCamera currentCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial camera to town camera
        currentCamera = townCamera;
        currentCamera.Priority = 10;

        // Disable other cameras initially
        battleCamera.Priority = 0;
        buildCamera.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch to battle camera if player is fighting
        if (GMDebug.Instance.hasLeftTown && currentCamera != battleCamera)
        {
            SwitchCamera(battleCamera);
        }

        // Switch to town camera if player is in town
        if (!GMDebug.Instance.hasLeftTown && currentCamera != townCamera)
        {
            SwitchCamera(townCamera);
        }

        // Switch to build camera if player is building and left-clicks on a build button
        if (GMDebug.Instance.isBuilding && currentCamera != buildCamera)
        {
            SwitchCamera(buildCamera);
        }

        // Handle panning in build camera mode (UNTESTED)
        if (currentCamera == buildCamera)
        {
            float panDirection = 0f;
            if (Input.mousePosition.x < 10f)
            {
                panDirection = -1f;
            }
            else if (Input.mousePosition.x > Screen.width - 10f)
            {
                panDirection = 1f;
            }
            currentCamera.transform.position += new Vector3(panDirection * panSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    // Function to switch to a new camera
    void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        // Disable current camera
        currentCamera.Priority = 0;

        // Enable new camera
        newCamera.Priority = 10;
        currentCamera = newCamera;

    }

}
