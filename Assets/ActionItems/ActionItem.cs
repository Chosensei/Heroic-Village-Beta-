using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action Items can be classified as Items that triggers an effect when interacted with. 
/// Can be stuff like lever switch, portals, breaking objects in scene etc. 
/// </summary>
public class ActionItem : Interactable
{
    public override void Interact()
    {
        Debug.Log("interacting with base actionItem");

    }
}
