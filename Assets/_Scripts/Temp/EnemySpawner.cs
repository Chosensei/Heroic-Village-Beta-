using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Drag & drop enemy prefab here")]
    public GameObject[] enemies;
    public Vector3 spawnValues;
    [Header("Spawning time variables")]
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;
    public bool stopSpawn;
    int randEnemy;

    [Header("Enemy Control")]
    public int enemiesSpawned = 0;
    public int maxEnemies;

    void Start()
    {
        StartCoroutine(Spawner());
    }

    void Update()
    {
        //Can use this if prefer to hv random spawn times, otherwise setting a fixed time is also good
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
        if (maxEnemies >= enemiesSpawned)
        {
            stopSpawn = true;
        }
        else
        {
            stopSpawn = false;
        }
            
    }
    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(startWait);

        while (!stopSpawn)
        {
            //Spawns random enemy, sofar this only spawns 2 gameobject, bump up value of 2nd param to add more enemy types
            randEnemy = Random.Range(0, enemies.Length - 1);

            //Random positions
            //Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));
            Vector3 spawnPosition = transform.position; 
            //Clone enemies object prefab into scene
            Instantiate(enemies[randEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), Quaternion.identity);
            //Spawn time Delay 
            yield return new WaitForSeconds(spawnWait);
        }
    }
}