using UnityEngine;

public class SignBoardTrigger : MonoBehaviour
{
    public GameObject interactionTextSign;  // the mini text that will appear to inform player they can interact
    public KeyCode keyToPress = KeyCode.F; // the key to press to change scenes
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show Tooltip 
            if (interactionTextSign != null)
            {
                interactionTextSign.SetActive(true);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(keyToPress))
        {
            GetComponent<SignBoard>().StartInteraction();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (interactionTextSign != null)
        {
            interactionTextSign.SetActive(false);
        }
    }
}
