using UnityEngine;

public class BlizzardSpawner : MonoBehaviour
{
    public GameObject flyingObject; //The prefab that will get spawned
    public int amount; //The amount of prefabs that will spawn
    public float destroyDelay; //The delay to destroy this gameobject and it children
    public float spawnInterval; //How fast will the prefabs be spawned
    public float spawnRadius; //The aoe radius
    public float spawnForce; //Force added to the prefab upon spawning
    public Vector3 spawnOffset; //Height at which to spawn the prefab in

    private bool isDead = false; //This stops our spawn interval loop
    private float spawnIntervalTimer; //The hidden timer that is used for our interval loop

    void Update()
    {
        if (!isDead) //If we are not dead
        {
            if (spawnIntervalTimer <= 0) //We check if the timer is smaller or equals to zero
            {
                //Reset out timer
                spawnIntervalTimer = spawnInterval;

                //Lower the amount by 1
                amount -= 1;

                //We calculate the spawn position for our prefab
                var spawnPos = transform.position + new Vector3(Random.insideUnitCircle.x * spawnRadius, 0, Random.insideUnitCircle.y * spawnRadius) + spawnOffset;

                //Create out prefab at our spawn position
                var obj = Instantiate(flyingObject, spawnPos, Quaternion.identity);

                //Set the parent of our prefab to be this gameobject to avoid crowding our hoierarchy with ltos of gameobjects
                obj.transform.SetParent(transform);

                //Calculate the direction of our spawn force using the spawnOffset
                var forceDir = transform.position - (transform.position + spawnOffset);

                //Add a force to the rigidbody upon spawning it
                obj.GetComponent<Rigidbody>().AddForce(forceDir * spawnForce, ForceMode.VelocityChange);

                //Destroy the prefab using a delay
                Destroy(obj, destroyDelay);

                if (amount <= 0) //If our amount is smaller or equals to zero
                {
                    isDead = true; //Stop our loops by setting this boolean to false
                    Destroy(gameObject, destroyDelay); //Destroy this gameobject using the destroy delay variable
                }
            }
            else { spawnIntervalTimer -= Time.deltaTime; } //If the spawn interval timer is bigger than 0 we lower it by Time.deltaTime
        }
    }
}
