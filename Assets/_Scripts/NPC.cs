using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public new string name;
    public string[] dialogue;
    public bool weaponSmith = false, magicCaster = false, guildLady = false;
    [HideInInspector]
    public Animator anim; 
    private void Awake()
    {
        anim = GetComponent<Animator>(); 
    }
    public void StartInteraction()
    {
        anim.SetTrigger("Welcome");
        anim.SetTrigger("Idle");
        DialogueManager.Instance.AddNewDialogue(dialogue, name, this);
    }

}
