using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public new string name;
    public string[] dialogue;
    public override void Interact()
    {
        DialogueManager.Instance.AddNewDialogue(dialogue, name);
        //Debug.Log("interacting with NPC");
    }
}
