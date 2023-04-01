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

    Button continueButton;
    Text dialogueText, nameText;
    int dialogueIndex; 

    void Awake()
    {
        continueButton = dialoguePanel.transform.Find("Continue").GetComponent<Button>();
        dialogueText = dialoguePanel.transform.Find("Text").GetComponent<Text>();
        nameText = dialoguePanel.transform.Find("NpcName").GetChild(0).GetComponent<Text>();

        continueButton.onClick.AddListener(delegate { ContinueDialogue(); });
        dialoguePanel.SetActive(false);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    
    public void AddNewDialogue(string[] lines, string npcName)
    {
        dialogueIndex = 0; 
        dialogueLines = new List<string>(lines.Length);
        dialogueLines.AddRange(lines);
        this.npcName = npcName; 
        //Debug.Log(dialogueLines.Count);
        CreateDialogue();
    }
    public void CreateDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = npcName;
        dialoguePanel.SetActive(true);
    }
    public void ContinueDialogue()
    {
        // if not yet at the last dialogue line, continue to the next dialogue 
        if (dialogueIndex < dialogueLines.Count-1)
        {
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];
        }
        else // if its the last dialogue line then close it after button
        {
            dialoguePanel.SetActive(false);
        }
    }
}
