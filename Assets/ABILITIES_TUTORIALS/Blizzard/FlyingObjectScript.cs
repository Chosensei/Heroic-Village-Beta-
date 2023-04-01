using UnityEngine;
using System.Linq;
using RPG.Attributes;
using RPG.Abilities.Effects;

public class FlyingObjectScript : MonoBehaviour
{
    public static bool canSpawnObj = true; 
    public string[] tagsToCheck; //The tags to check, example; Enemy, Ground, etc.
    public float impactRadius; //Aoe radius that will damage enemies
    public float destroyDelay; //Delay for the gameobject to be deleted from the game
    private GameObject impactFX; //Our particle system
    float spawnIntervalTimer; //The hidden timer that is used for our interval loop
    private Health target = null;
    private Vector3 targetPoint;
    private GameObject instigator = null;
    private float damage = 0;


    private void Start()
    {
        impactFX = transform.Find("ImpactFX").gameObject; //Assign our impact particle gameobject
    }
    void Update()
    {
        //if (spawnIntervalTimer <= 0) //We check if the timer is smaller or equals to zero
        //{
        //    Debug.Log("interval = 0");
        //    //Reset out timer
        //    spawnIntervalTimer = SpawnFlyingObjectEffect.spawnInterval;
        //    canSpawnObj = true; 
        //}
        //else 
        //{
        //    Debug.Log("interval < 0");
        //    //If the spawn interval timer is bigger than 0 we lower it by Time.deltaTime
        //    spawnIntervalTimer -= Time.deltaTime;
        //    canSpawnObj = false; 
        //} 

    }
    public void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
    {
        this.target = target;
        this.targetPoint = targetPoint;
        this.damage = damage;
        this.instigator = instigator;

        Destroy(gameObject, destroyDelay);
    }
    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        SetTarget(instigator, damage, target);
    }

    public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
    {
        SetTarget(instigator, damage, null, targetPoint);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Check our tags before we continue
        if (tagsToCheck.Contains(other.gameObject.tag))
        {
            //We get all colliders that overlap our sphere cast
            Collider[] objectsInRange = Physics.OverlapSphere(transform.position, impactRadius);

            //For each one of those colliders we detected we do the following
            foreach (Collider col in objectsInRange)
            {
                //We get the enemies within range that contain a rigidbody
                Rigidbody enemy = col.GetComponent<Rigidbody>();

                //We check if enemy has been found
                if (enemy != null)
                {
                    //Destroy(enemy.gameObject);
                    //You can call your damaging script here
                }
            }

            //We enable the particle system gameobject
            impactFX.SetActive(true);

            //We remove its parent so that if we destroy the blizzard spawner, this particle system will continue to play
            impactFX.transform.SetParent(null);

            //We destroy the particle system gameobkect using the destroy delay variable
            Destroy(impactFX, destroyDelay);

            //We destroy this gameobject
            Destroy(gameObject);
        }
    }
}
