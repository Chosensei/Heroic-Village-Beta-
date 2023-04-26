using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using RPG.Core;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Abilities", menuName = "Abilities/New Ability", order = 0)]
    // IF DECIDED TO USE THIS. MAKE SURE TO KEEP THIS CLASS INHERITED FROM ACTION ITEM FOR EASE OF SWAPPING IN INVENTORY
    // ALSO THE COOLDOWN CAN STAY, THEN YOU MAY TURN THIS INTO A ABILITY ACTIVATION MASTER SWITCH CLASS
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilteringStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float cooldownTime = 0;
        [SerializeField] float manaCost = 0;
        [SerializeField] float damage = 0;

        public float CooldownTime
        {
            get { return cooldownTime; }
            set { cooldownTime = value; }
        }

        public float ManaCost
        {
            get { return manaCost; }
            set { manaCost = value; }
        }

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public override void Use(GameObject user)
        {
            Mana mana = user.GetComponent<Mana>();
            if (mana.GetMana() < manaCost) { return; }  // When not enough mana, simply return

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>(); 
            if (cooldownStore.GetTimeRemaining(this) > 0) { return; }   // When we got a cooldown already in progress, simply return 

            AbilityData data = new AbilityData(user);

            // Check for action priorities, handle things like cancellation of existing action when new action is received
            //ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            //actionScheduler.StartAction(data);
            targetingStrategy.StartTargeting(data,
                () => {
                    TargetAcquired(data);
                });
        }

        private void TargetAcquired(AbilityData data)
        {
            if (data.IsCancelled()) return;

            Mana mana = data.GetUser().GetComponent<Mana>();
            // Check if we have enough mana before targeting
            if (!mana.UseMana(manaCost)) return;

            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);

            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
            //FOR DEBUG ONLY
            //foreach (var target in targets)
            //{
            //    Debug.Log("Target acquired : " + target);
            //}
        }
        private void EffectFinished()
        {

        }
    }
}


