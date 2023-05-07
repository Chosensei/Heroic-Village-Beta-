using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBoard : MonoBehaviour
{
    public string[] dialogue;
    public void StartInteraction()
    {
        SignBoardManager.Instance.AddNewDialogue(dialogue);
    }
}
