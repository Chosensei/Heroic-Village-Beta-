using System;
using GameDevTV.Utils;
using GameDevTV.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IDamageable, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoints;

        bool wasDeadLastFrame = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private void Update()
        {
            //FOR DEBUGGING ONLY
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    TakeDamage(this.gameObject, 10);
            //}
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return healthPoints.value <= 0;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if (IsDead())
            {
                // Play death anim
                onDie.Invoke();
                // Find another way later to give exp to player
                //AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
            //UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            UpdateState();
        }
        public float RestoreFullHealth()
        {
            return healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        //private void Die()
        //{
        //    if (wasDeadLastFrame ) return;

        //    wasDeadLastFrame  = true;
        //    GetComponent<Animator>().SetTrigger("die");
        //    GetComponent<ActionScheduler>().CancelCurrentAction();
        //}
        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if (!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die"); // you need to add a die anim first
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead();
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;
            UpdateState(); 
        }
    }
}

