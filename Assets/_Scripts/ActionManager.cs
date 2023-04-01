using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private ActionMethod currentAction;

    public void StartAction(ActionMethod action)
    {
        if (currentAction == action) return;
        if (currentAction != null)
        {
            currentAction.Cancel();
        }
        currentAction = action;
    }

    public void CancelCurrentAction()
    {
        StartAction(null);
    }
}
