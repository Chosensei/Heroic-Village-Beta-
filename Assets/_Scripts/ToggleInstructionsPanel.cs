using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInstructionsPanel : MonoBehaviour
{
    public GameObject instructionMenu; 
    public bool instructionMenuIsActive;
    void Start()
    {
        instructionMenu.SetActive(false);
    }

    // Instructions panel open / closes when pressed
    public void ButtonOnClick()
    {
        instructionMenuIsActive = !instructionMenuIsActive;
        instructionMenu.SetActive(instructionMenuIsActive);
    }
}
