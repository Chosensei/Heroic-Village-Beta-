using System;
using UnityEngine;
using RPG.Attributes;
using RPG.Combat;
using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.PlayerLoop;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Flying Object Effect", menuName = "Abilities/Effects/Spawn Flying Obj", order = 0)]
    public class SpawnFlyingObjectEffect : EffectStrategy
    {
        [SerializeField] FlyingObjectScript flyingObject; //The prefab that will get spawned
        [SerializeField] Vector3 spawnOffset; // Height at which to spawn the prefab in
        [SerializeField] int amount; //The amount of prefabs that will spawn
        [SerializeField] float destroyDelay; //The delay to destroy this gameobject and it children
        public static float spawnInterval; //How fast will the prefabs be spawned
        [SerializeField] float spawnRadius; //The aoe radius
        [SerializeField] float spawnForce; //Force added to the prefab upon spawning

        [SerializeField] float damage;
        [SerializeField] bool isRightHand = true;
        [SerializeField] bool useTargetPoint = true;

        //float spawnIntervalTimer; //The hidden timer that is used for our interval loop
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished)); 
            
        }
        private IEnumerator Effect(AbilityData data, Action finished)
        {

            if (FlyingObjectScript.canSpawnObj == false) yield break;
            else
            {
                Debug.Log("Spawn Flying Object Effect started!");

                //Lower the amount by 1
                amount -= 1;

                //We calculate the spawn position for our prefab
                var spawnPos = data.GetUser().transform.position + new Vector3(Random.insideUnitCircle.x * spawnRadius, 0, Random.insideUnitCircle.y * spawnRadius) + spawnOffset;

                //Create out prefab at our spawn position
                var obj = Instantiate(flyingObject, spawnPos, Quaternion.identity);

                // Set target of flying object
                //flyingObject.SetTarget(data.GetTargetedPoint(), data.GetUser(), damage);

                //Set the parent of our prefab to be this gameobject to avoid crowding our hierarchy with lots of gameobjects
                obj.transform.SetParent(data.GetUser().transform);

                //Calculate the direction of our spawn force using the spawnOffset
                var forceDir = data.GetUser().transform.position - (data.GetUser().transform.position + spawnOffset);

                //Add a force to the rigidbody upon spawning it
                obj.GetComponent<Rigidbody>().AddForce(forceDir * spawnForce, ForceMode.VelocityChange);

                //Destroy the prefab using a delay
                yield return new WaitForSeconds(3);
                Destroy(obj);

            }
            finished();
            Debug.Log("Spawn Flying Object Effect ended!");
        }
    }
}

