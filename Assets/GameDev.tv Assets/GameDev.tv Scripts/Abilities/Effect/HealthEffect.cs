using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health")]
    public class HealthEffect : EffectStrategy
    {
        // This value can be +ve / -ve depending on if you want to use it for damage or heal
        [Range(-1000f, 1000f)]
        [SerializeField] float healthChange; 
        public override void StartEffect(AbilityData data, Action finished)
        { 
            foreach (var target in data.GetTargets())
            {
                var damagable = target.TryGetComponent(out IDamageable targetDamagable);
                if (damagable)
                {
                    if (healthChange < 0)
                    {
                        targetDamagable.TakeDamage(data.GetUser(), -healthChange);
                    }
                }
            }
            finished();
        }

    }
}


