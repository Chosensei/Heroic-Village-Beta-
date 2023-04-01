using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Demo Targeting", menuName = "Abilities/Targeting/Demo")]
public class DemoTargeting : TargetingStrategy
{
    public override void StartTargeting(AbilityData data, Action finished)
    {
        finished(); 
    }
}
