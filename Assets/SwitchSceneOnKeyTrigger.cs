using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneOnKeyTrigger : MonoBehaviour
{
    public string sceneName;    // the name of the scene to switch to
    public GameObject tooltip;  // the mini text that will appear to inform player they can switch scene
    public KeyCode keyToPress = KeyCode.F; // the key to press to change scenes

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show Tooltip 
            if (tooltip != null)
            {
                tooltip.SetActive(true);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(keyToPress))
        {
            if (sceneName == null) { return; }

            if (SceneManager.GetSceneByName(sceneName) != null)
            {
                // Load the selected scene
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                // Throw an exception error if the scene cannot be found
                throw new System.Exception("Scene not found!");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide Tooltip
            tooltip.SetActive(false);
        }
    }
}
