using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SignBoardManager : MonoBehaviour
{
    [HideInInspector]
    public static SignBoardManager Instance { get; set; }
    [HideInInspector]
    public List<string> dialogueLines = new List<string>();

    public GameObject dialoguePanel;
    GameObject closeButton;
    Button continueButton;
    Text dialogueText;
    int dialogueIndex;

    void Awake()
    {
        continueButton = dialoguePanel.transform.Find("Continue").GetComponent<Button>();
        dialogueText = dialoguePanel.transform.Find("Text").GetComponent<Text>();
        closeButton = dialoguePanel.transform.Find("Close").gameObject;
        closeButton.SetActive(false);
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


    public void AddNewDialogue(string[] lines)
    {
        dialogueIndex = 0;
        dialogueLines = new List<string>(lines.Length);
        dialogueLines.AddRange(lines);
        CreateDialogue();
    }
    public void CreateDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        dialoguePanel.SetActive(true);
    }
    public void ContinueDialogue()
    {
        // if not yet at the last dialogue line, continue to the next dialogue 
        if (dialogueIndex < dialogueLines.Count - 1)
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