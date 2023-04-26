using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineVirtualCameraBase[] cameras;
    public CinemachineVirtualCameraBase startCamera;
    //[HideInInspector]
    public CinemachineVirtualCameraBase currentCamera;

    // stores the index of the camera to switch to in the next scene
    private static int targetCameraIndex = -1;

    private void Awake()
    {
        // Make sure this object persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {


        // Set the camera priorities
        for (int i = 0; i < cameras.Length; i++)
        {
            // Set a higher priority value to current active camera 
            if (cameras[i] == currentCamera)
            {
                cameras[i].Priority = 20;
            }
            else
            {
                cameras[i].Priority = 10;
            }
        }

        // Reset the target camera index
        targetCameraIndex = -1;

        // Subscribe to the sceneLoaded event so we can update the camera priorities
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the "ClearShotCamera" game object in the scene
        GameObject clearShotCameraObject = GameObject.Find("ClearShotCamera");

        // If found, set it as the current camera and adjust its priority
        if (clearShotCameraObject != null)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i].gameObject == clearShotCameraObject)
                {
                    currentCamera = cameras[i];
                    currentCamera.Priority = 20;
                }
                else
                {
                    cameras[i].Priority = 10;
                }
            }
        }
        else
        {
            // If not found, set the start camera as the current camera
            currentCamera = startCamera;
            currentCamera.Priority = 20;

            // Set the camera priorities
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i] != currentCamera)
                {
                    cameras[i].Priority = 10;
                }
            }
        }

        // Reset the target camera index
        targetCameraIndex = -1;
    }

    public void SwitchCamera(int index)
    {
        // Switch to the camera at the specified index
        if (index >= 0 && index < cameras.Length)
        {
            currentCamera = cameras[index];
            currentCamera.Priority = 20;

            for (int i = 0; i < cameras.Length; i++)
            {
                if (i != index)
                {
                    cameras[i].Priority = 10;
                }
            }
        }
        else
        {
            Debug.LogWarning("Camera index out of range!");
        }
    }

    // Can be called from a button or other UI element to switch to a different scene and set the target camera index
    public void LoadScene(string sceneName)
    {
        // this way is working not with CameraManager instance
        SceneManager.LoadScene(sceneName);
        Debug.Log("Load scene");
        if (SceneManager.GetActiveScene().name == "Weapon_Shop_Interior")
        {
            // Find the relevant camera gameobject and set it as the current camera
            var newCamObj = FindObjectOfType<ShopCamera>();
            currentCamera = newCamObj.startCam; 
        }
    }
}



