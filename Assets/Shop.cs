using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public string shopSceneName; // the name of the shop interior scene
    public KeyCode keyToPress = KeyCode.F; // the key to press to enter/exit the shop
    public GameObject enterShopTooltip, exitShopTooltip; // the tooltip to display when the player can enter/exit the shop
    public Transform shopEnterPoint, shopExitPoint; // the exit point in the shop where the player will be transported back to the previous scene
    public GameObject player; // the player game object
    public bool magicShop;  // allow for opening menu
    public bool canEnter;
    public bool isInShop; 

    private void OnTriggerEnter(Collider other)
    {
        // If not in shop
        if (!TestMovement.isInShop)
        {
            if (other.CompareTag("Player"))
            {
                // Show enter shop tooltip
                if (enterShopTooltip != null)
                {
                    enterShopTooltip.SetActive(true);
                }
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                // Show exit shop tooltip
                if (exitShopTooltip != null)
                {
                    exitShopTooltip.SetActive(true);
                }
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (!TestMovement.isInShop)
        {
            if (other.CompareTag("Player") && Input.GetKeyDown(keyToPress))
                EnterShop(other.gameObject);
        }
        else
        {
            if (other.CompareTag("Player") && Input.GetKeyDown(keyToPress))
                ExitShop(other.gameObject);
        }
        //if (!isInShop && other.CompareTag("Player"))
        //{
        //    canEnter = true; 
        //}
        //else
        //{
        //    canEnter = false; 
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (!TestMovement.isInShop)
        {
            if (other.CompareTag("Player"))
            {
                // Show enter shop tooltip
                if (enterShopTooltip != null)
                {
                    enterShopTooltip.SetActive(false);
                }
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                // Show exit shop tooltip
                if (exitShopTooltip != null)
                {
                    exitShopTooltip.SetActive(false);
                }
            }
        }
    }
    private void EnterShop(GameObject player)
    {
        // Change camera to shop camera
        print("entered shop!");
        // Set the player position to the shop entrance
        player.transform.position = shopEnterPoint.position;

        // Set the player rotation to face the shop interior
        //player.transform.rotation = Quaternion.LookRotation(shopEnterPoint.position - transform.position);

        // Set the flag to indicate the player is now in the shop
        TestMovement.isInShop = true;
    }
    private void ExitShop(GameObject player)
    {
        // Exit Shop 
        // Change camera to town camera

        // Set the player position to the shop entrance
        player.transform.position = shopExitPoint.position;
        // Set the player rotation to face the exit
        //player.transform.rotation = Quaternion.LookRotation(transform.position - shopExitPoint.position);

        // Set the flag to indicate the player is no longer in the shop
        TestMovement.isInShop = false;
    }
    private void Update()
    {
        isInShop = TestMovement.isInShop;
        //if (canEnter && Input.GetKeyDown(keyToPress))
        //{
        //    EnterShop(player);
        //}
        //else if (!canEnter && Input.GetKeyDown(keyToPress))
        //{
        //    ExitShop(player);
        //}
        //if (isInShop)
        //{
        //    // Check if the player presses the key to exit the shop
        //    if (Input.GetKeyDown(keyToPress))
        //    {
        //        ExitShop(player);
        //    }
        //}
        //else
        //{
        //    // Check if the player is inside the shop trigger area and presses the key to enter the shop
        //    if (Input.GetKeyDown(keyToPress) && IsPlayerInShopArea())
        //    {
        //        EnterShop(player);
        //    }
        //}
    }
    private bool IsPlayerInShopArea()
    {
        // Check if the player is inside the shop trigger area
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}