using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Multi Changeable Targeting", menuName = "Abilities/Targeting/MultiChangeable", order = 0)]
    public class MultiChangeableTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            throw new NotImplementedException();
        }

    }
}

