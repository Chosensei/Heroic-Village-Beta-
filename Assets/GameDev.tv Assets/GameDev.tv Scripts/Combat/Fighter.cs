using UnityEngine;
using RPG.Movement;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;
using GameDevTV.Inventories;
using InventoryExample.Core;
using System.Collections;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        
        public static bool isAttacking = false;
        float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        Equipment equipment;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        [SerializeField] GameObject playerWeapon;
        private void Awake()
        {
            playerWeapon.GetComponent<PlayerWeapon>().weaponCollider.enabled = false;
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            // check for attack input & only allow swing sword when not in town
            if (Input.GetMouseButtonDown(0) && !isAttacking && GMDebug.Instance.hasLeftTown)   
            {
                StartCoroutine(WeaponAttack());
            }

        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }
        // Grab hand component for our magic cast
        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand)
            {
                return rightHandTransform;
            }
            else
            {
                return leftHandTransform;
            }
        }
        private IEnumerator WeaponAttack()
        {
            isAttacking = true;
            // set attack animation here
            TriggerAttack();
            SoundManager.Instance.PlaySfx("CharacterAttack");
            // wait for attack duration
            yield return new WaitForSeconds(2f);

            // end attack animation here
            StopAttack();
            isAttacking = false;
        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            //if (timeSinceLastAttack > timeBetweenAttacks)
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        // This will trigger the Hit() event.
            //        TriggerAttack();
            //        timeSinceLastAttack = 0;
            //    }
            //}
        }

        public void TriggerAttack()
        {
            isAttacking = true;
            // enable weapon collider
            playerWeapon.GetComponent<PlayerWeapon>().weaponCollider.enabled = true;

            GetComponent<Animator>().ResetTrigger("stopWeaponAttack");
            GetComponent<Animator>().SetTrigger("weaponAttack");
        }
        public void StopAttack()
        {
            isAttacking = false;
            // disable weapon collider
            playerWeapon.GetComponent<PlayerWeapon>().weaponCollider.enabled = false;
            
            GetComponent<Animator>().ResetTrigger("weaponAttack");
            GetComponent<Animator>().SetTrigger("stopWeaponAttack");
        }

        // Animation Event
        //TODO re-add after implement stat class
        void Hit()
        {
            if (target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            BaseStats targetBaseStats = target.GetComponent<BaseStats>();
            if (targetBaseStats != null)
            {
                float defence = targetBaseStats.GetStat(Stat.Defence);
                damage /= 1 + defence / damage;
            }

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform))
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            Debug.Log("Cancel Fighter");
            target = null;
            GetComponent<PlayerMovement>().Cancel();
        }


    }
}