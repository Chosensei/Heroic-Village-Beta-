using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerJobBoard : ActionItem
{
    public string[] dialogue;
    private new string name = "Job Board";
    public override void Interact()
    {
        DialogueManager.Instance.AddNewDialogue(dialogue, name);
        Debug.Log("interacting with Job board");

    }
}
