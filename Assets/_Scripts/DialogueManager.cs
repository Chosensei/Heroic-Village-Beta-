using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject shopOptionPanel2;
    public GameObject weaponShopMenu;
    public GameObject magicShopMenu;
    public GameObject repairWallMenu;
    public GameObject upgradeWallMenu;
    public TMP_Text shopButtonTxt;
    public TMP_Text shopButtonTxt2;

    // TO DO :extend for wall repairs and upgrades
    Button continueButton;
    Button closeButton;
    Button shopButton;
    Button shopButton2; 
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
        shopButton2 = shopOptionPanel2.transform.Find("OpenShopButton2").GetComponent<Button>();
        continueButton.onClick.AddListener(delegate { ContinueDialogue(); });
        closeButton.onClick.AddListener(delegate { EndDialogue(); });
        shopButton.onClick.AddListener(delegate { OpenShopMenu(); });
        shopButton2.onClick.AddListener(() => { 
            upgradeWallMenu.SetActive(true);
            repairWallMenu.SetActive(false);
        });

        dialoguePanel.SetActive(false);
        shopOptionPanel.SetActive(false);
        shopOptionPanel2.SetActive(false);
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

                //shopOptionPanel.SetActive(true);
                SetShopUI(); 

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
        shopOptionPanel2.SetActive(false);
        dialoguePanel.SetActive(false);
        weaponShopMenu.SetActive(false);
        magicShopMenu.SetActive(false);
        repairWallMenu.SetActive(false);
        upgradeWallMenu.SetActive(false);
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
            npc.anim.SetTrigger("OpenShop");
            npc.anim.SetTrigger("Idle");
            magicShopMenu.SetActive(true);
        }
        else if (npc.guildLady)
        {
            npc.anim.SetTrigger("OpenShop");
            npc.anim.SetTrigger("Idle");
            repairWallMenu.SetActive(true);
            upgradeWallMenu.SetActive(false); 
        }

        // Clear the dialogue box 
        dialogueText.text = "";
        // hide the shop button 
        //shopButton.gameObject.SetActive(false);
    }
    public void SetShopUI()
    {
        if (npc.weaponSmith)
        {
            shopOptionPanel.SetActive(true);
            shopButtonTxt.text = "WEAPON UPGRADE";
        }
        else if (npc.magicCaster)
        {
            shopOptionPanel.SetActive(true);
            shopButtonTxt.text = "MAGIC SHOP";
        }
        else if (npc.guildLady)
        {
            shopOptionPanel.SetActive(true);
            shopOptionPanel2.SetActive(true);
            shopButtonTxt.text = "WALL REPAIRS";
            shopButtonTxt2.text = "WALL UPGRADES";
        }
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
