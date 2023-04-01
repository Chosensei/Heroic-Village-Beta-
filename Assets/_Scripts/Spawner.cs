using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool enemyHasDied => this.monster.GetComponent<EnemyTarget>().isAlive == false;
    [SerializeField] private GameObject monster;
    //[SerializeField] List<GameObject> monsterSpawned = new List<GameObject>();
    [SerializeField] private float timeBtwnSpawns = 5f;
    [SerializeField][Range(0, 100)] private float spawnDelay = 10f;
    [SerializeField][Range(0, 100)] private int maxEnemies = 3;
    public int enemyCount { get; set; }
    private bool spawning; 
    private bool stopSpawn = false;

    void Start()
    {
        Spawn();
    }
    void Update()
    {
        if (stopSpawn)
            return;

        if (Time.time >= timeBtwnSpawns)
        {
            Spawn();
            timeBtwnSpawns = Time.time + spawnDelay;
        }  
        if (this.monster.GetComponent<EnemyTarget>().isAlive == false)
        {
            enemyCount--;
        }
    }
    void Spawn()
    {
        Instantiate(monster, transform.position, Quaternion.identity);
        //monsterSpawned.Add(monster); 
        enemyCount++;
        if (enemyCount >= maxEnemies)
        {
            stopSpawn = true; 
        }

    }
}
