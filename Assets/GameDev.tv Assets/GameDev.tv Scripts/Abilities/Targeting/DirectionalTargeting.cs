using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Directional Targeting", menuName = "Abilities/Targeting/Directional", order = 0)]
    // MUST SET [USE TARGET POINT] in SpawnProjectileEffect variable to TRUE!
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1;    // To prevent the projectile from hitting the ground
        [SerializeField] Transform targetingPrefab;
        Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController pc = data.GetUser().GetComponent<PlayerController>();
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                data.SetTargetedPoint(raycastHit.point + ray.direction * groundOffset / ray.direction.y);
            }
            finished();
        }
        //targetingPrefabInstance.position = new Vector3(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z);
        //Quaternion transRot = Quaternion.LookRotation(targetingPrefabInstance.position - pc.transform.position);
        //targetingPrefabInstance.rotation = Quaternion.Lerp(transRot, targetingPrefab.transform.rotation, 0f);
        //targetingPrefabInstance.gameObject.SetActive(false);
    }
}