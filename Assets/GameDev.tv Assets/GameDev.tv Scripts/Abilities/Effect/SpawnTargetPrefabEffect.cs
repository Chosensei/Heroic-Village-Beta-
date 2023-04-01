using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "Abilities/Effects/Spawn Target Prefab", order = 0)]
    // SPAWNS WHATEVER PARTICLE FX YOU WANT AT THE POINT YOU CLICK
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [Tooltip("Prefab to Spawn.")]
        [SerializeField] Transform prefabToSpawn;
        [Tooltip("Should the prefab be parented to the user?")]
        [SerializeField] bool followUser = false;
        [SerializeField] bool destroyAfterSpawn = false;
        [Tooltip("How long before destroying Prefab? (-1 for never).")]
        [SerializeField] float destroyDelay = -1;   // Don't destroy by default unless specified 
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            Transform prefabInstance;
            // Spawn prefab on the user if enabled in editor, otherwise spawn at the selected target with no parent.
            if (followUser) prefabInstance = Instantiate(prefabToSpawn, data.GetUser().transform);
            else
            {
                prefabInstance = Instantiate(prefabToSpawn);
                prefabInstance.position = data.GetTargetedPoint();
            }

            if (destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);

                if (destroyAfterSpawn) { Destroy(prefabInstance.gameObject); }
            }
            finished();
        }
    }
}
