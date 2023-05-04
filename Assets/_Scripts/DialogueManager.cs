using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [HideInInspector]
    public static DialogueManager Instance { get; set; }
    [HideInInspector]
    public List<string> dialogueLines = new List<string>();
    [HideInInspector]
    public string npcName;
    public GameObject dialoguePanel;
    public GameObject shopOptionPanel;
    public GameObject weaponShopMenu;
    public GameObject magicShopMenu;
    Button continueButton;
    Button closeButton;
    Button shopButton; 
    Text dialogueText, nameText;
    int dialogueIndex;
    NPC npc;

    void Awake()
    {
        continueButton = dialoguePanel.transform.Find("Continue").GetComponent<Button>();
        dialogueText = dialoguePanel.transform.Find("Text").GetComponent<Text>();
        nameText = dialoguePanel.transform.Find("NpcName").GetChild(0).GetComponent<Text>();
        closeButton = dialoguePanel.transform.Find("Close").GetComponent<Button>();
        shopButton = shopOptionPanel.transform.Find("OpenShopButton").GetComponent<Button>();

        continueButton.onClick.AddListener(delegate { ContinueDialogue(); });
        closeButton.onClick.AddListener(delegate { EndDialogue(); });
        shopButton.onClick.AddListener(delegate { OpenShopMenu(); });

        dialoguePanel.SetActive(false);
        shopOptionPanel.SetActive(false);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // FOR SIGNBOARDS
    public void AddNewDialogue(string[] lines, string npcName)
    {
        dialogueIndex = 0;
        dialogueLines = new List<string>(lines.Length);
        dialogueLines.AddRange(lines);
        this.npcName = npcName;
        CreateDialogue();
    }
    // FOR NPC
    public void AddNewDialogue(string[] lines, string npcName, NPC npc)
    {
        dialogueIndex = 0; 
        dialogueLines = new List<string>(lines.Length);
        dialogueLines.AddRange(lines);
        this.npcName = npcName;
        this.npc = npc;

        CreateDialogue();
    }
    public void CreateDialogue()
    {
        continueButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = npcName;
        dialoguePanel.SetActive(true);
    }
    // Need to change the logic for sign boards that don't require shop button to be displayed!
    public void ContinueDialogue()
    {
        // if not yet at the last dialogue line, continue to the next dialogue 
        if (dialogueIndex < dialogueLines.Count-1)
        {
            npc.anim.SetTrigger("Idle");
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];

            // if this is the last line, set the shopOptionPanel to active
            if (dialogueIndex == dialogueLines.Count - 1)
            {
                npc.anim.SetTrigger("Welcome");
                npc.anim.SetTrigger("Idle");

                shopOptionPanel.SetActive(true);
                continueButton.gameObject.SetActive(false);
                closeButton.gameObject.SetActive(true);
            }
        }
    }
    public void EndDialogue()
    {
        npc.anim.SetTrigger("GoodBye");
        npc.anim.SetTrigger("Idle");

        shopOptionPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        weaponShopMenu.SetActive(false);
        magicShopMenu.SetActive(false);
    }
    public void OpenShopMenu()
    {
        if (npc.weaponSmith)
        {
            npc.anim.SetTrigger("OpenShop");
            npc.anim.SetTrigger("Idle");
            weaponShopMenu.SetActive(true);
        }
        else if (npc.magicCaster)
        {
            magicShopMenu.SetActive(true);
        }
        else if (npc.guildLady)
        {
            // display a message saying the guild lady does not have a shop
            Debug.Log("I'm sorry, I don't have a shop.");
        }

        // Clear the dialogue box 
        dialogueText.text = "";
        // hide the shop button 
        //shopButton.gameObject.SetActive(false);
    }
    public void ShowThankYouMessage()
    {
        string[] thankYouMessages = new string[] {
        "Thank you for your business!",
        "Your patronage is much appreciated!",
        "You're the best customer ever!"
        };
        int index = Random.Range(0, thankYouMessages.Length);
        dialogueText.text = thankYouMessages[index];
    }
}
