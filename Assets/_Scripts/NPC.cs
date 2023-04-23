using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public new string name;
    public string[] dialogue;
    public bool weaponSmith = false, magicCaster = false, guildLady = false; 
    public void StartInteraction()
    {
        DialogueManager.Instance.AddNewDialogue(dialogue, name);
    }
    public void OpenShopMenu()
    {
        if (weaponSmith)
        {
            UIManager.Instance.WeaponShopMenu.SetActive(true);
        }
        if (magicCaster)
        {
            UIManager.Instance.MagicShopMenu.SetActive(true);
        }
    }
}
