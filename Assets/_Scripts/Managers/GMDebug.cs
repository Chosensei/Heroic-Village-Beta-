using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GMDebug : Singleton<GMDebug>
{
    // Clean 
    public int MoneyInBank = 1000000;   // Player's money in the game
    private int currentDay = 0;  // current day in the game
    private int maxDays = 10;    // Max day in the game  
    private GameObject player;
    public SkyboxController skyboxController; 
    public Transform playerTownSpawnPoint;  // player spawn position in town
    public bool battleStarted = false;
    public bool hasLeftTown = false;    // Used to enable battle button
    public bool isBuilding = false;  // Used to change cam perspective 
    public bool firstDay = false;
    public bool startSpawn = false; 
    public int currentWave = 0; // Current wave the player is fighting
    public int currentWaveCount; // Keep count of the current wave for display purposes
    public int maxWaves; // Maximum number of waves in battle phase, increments after X days
    public int enemiesPerWave; // Number of enemies per wave, increments after X days
    public int currentEnemies = 0; // Number of enemies spawned in current wave
    public int enemiesRemaining = 0;  // total number of enemies remaining left to defeat
    public int enemiesDefeated = 0; // Keep count of the total number of enemies defeated, It gets incremented whenever an enemy is killed.
    public float spawnIntervalLower = 1.5f; 
    public float spawnIntervalUpper = 6.5f;  // time between waves

    // Dirty 
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPoints;
    public CageWall lastWall; 
    
    //Added function to calculate income from village houses
    public void CalculateVillageHouseIncome()
    {
        GameObject[] villageHouses = GameObject.FindGameObjectsWithTag("VillageHouse");
        villageHouses = GameObject.FindGameObjectsWithTag("VillageHouse");

        if (villageHouses == null) return;
        else
        {
            foreach (GameObject villageHouse in villageHouses)
            {
                var vh = villageHouse.GetComponent<TowerLevelSwitch>();
                MoneyInBank += vh.baseIncome;
                UIManager.Instance.moneyText.text = MoneyInBank.ToString();
            }
        }

    }

    /* BUTTON REFERENCE METHODS */
    public void StartBattle()
    {
        Debug.Log("Battle Started!");
        battleStarted = true;
        // Enable Battle info
        UIManager.Instance.BattleMenu.SetActive(true);
        // Enable minimap
        UIManager.Instance.Minimap.SetActive(true);
        // set current wave to 1
        currentWave = 1;
        // update UI to reflect current wave 
        UIManager.Instance.CurrentWaveValue.text = currentWave.ToString();
        // set numbers defeated to zero
        enemiesDefeated = 0;
        // enemies remaining to enemies per wave minus enemies defeated 
        enemiesRemaining = enemiesPerWave - enemiesDefeated;
        UIManager.Instance.EnemyRemainingValue.text = enemiesRemaining.ToString();
        // WORKS! - TEST TO TRY SPAWNING STRAIGHTAWAY 
        StartCoroutine(SpawnWave());
    }
    public void ReturnToTown()
    {
        // Transport player back to town 
        player.transform.position = playerTownSpawnPoint.position;
        hasLeftTown = false; 
        Debug.Log("Resting Started!");
        // Deactivate Win menu
        UIManager.Instance.WinDayMenu.SetActive(false);
        // Deactivate minimap
        UIManager.Instance.Minimap.SetActive(false);
        skyboxController.ToggleSkybox();    // Start night time
    }
    public void EnterBuildMode()
    {
        isBuilding = true;
    }
    public void ExitBuildMode()
    {
        isBuilding = false;
        // Change camera back to towncam
    }
    /* AFTER SAVING */
    public void StartRest()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // After saving the game, start next day 
            InitializeNextDay();
        }
    }
    void Start()
    {
        InitializeDayOne();
        player = GameObject.FindWithTag("Player"); 
        //lastWall = GetComponent<CageWall>(); 
        UIManager.Instance.StartBattleButton.SetActive(false);
        UIManager.Instance.BattleMenu.SetActive(false);
    }

    void Update()
    {
        if (hasLeftTown && !battleStarted)
        {
            UIManager.Instance.StartBattleButton.SetActive(true);
            UIManager.Instance.BuildButton.SetActive(false);
        }
        else
        {
            UIManager.Instance.StartBattleButton.SetActive(false);
            UIManager.Instance.BuildButton.SetActive(true); 
        }

        //if (!isInTown && battleStarted)
        if (battleStarted)
        {
            startSpawn = true; 
            // enemies remaining to enemies per wave minus enemies defeated 
            enemiesRemaining = enemiesPerWave - enemiesDefeated;
            UIManager.Instance.EnemyRemainingValue.text = enemiesRemaining.ToString();
            // display current wave when has battle started 
            currentWaveCount = currentWave;
            UIManager.Instance.CurrentWaveValue.text = currentWaveCount.ToString();

            // check for if final wave is cleared & all enemies in world are dead
            if (currentWave > maxWaves)
            {
                EndBattle();
            }
        }
        else 
        {
            startSpawn = false;
            //StartRest();
        }
        StartNextWave();

        CheckWinLoseCondition(); 
    }
    void InitializeDayOne()
    {
        firstDay = true; 
        battleStarted = false; 
        currentDay = 1;
        currentWave = 1; 
        maxWaves = 5;
        enemiesPerWave = 3;
        UIManager.Instance.moneyText.text = MoneyInBank.ToString(); // Set starting money 
        UIManager.Instance.CurrentDayValue.text = currentDay.ToString();
        UIManager.Instance.CurrentWaveValue.text = "-";
        UIManager.Instance.EnemyRemainingValue.text = "-";
    }
    public void InitializeNextDay()
    {
        // Increase current day by 1
        currentDay++;
        // Update UI to reflect Current Day
        UIManager.Instance.CurrentDayValue.text = currentDay.ToString();

        // Set max waves and enemies per wave based on the current day count
        if (currentDay > 3 && currentDay <= 7) { maxWaves = 6; enemiesPerWave = 4; }
        if (currentDay > 7 && currentDay <= 11) { maxWaves = 7; enemiesPerWave = 5; }
        if (currentDay > 11 && currentDay <= 16) { maxWaves = 8; enemiesPerWave = 6; }

        skyboxController.ToggleSkybox();    // Start a new daytime
    }

    public void EndBattle()
    {
        battleStarted = false;
        // How do we know the battle is over? 
        Debug.Log("Battle Ended!");
        // Disable Battle info
        UIManager.Instance.BattleMenu.SetActive(false);
        UIManager.Instance.CurrentWaveValue.text = "-";
        UIManager.Instance.EnemyRemainingValue.text = "-";

        // Activate Win menu
        UIManager.Instance.WinDayMenu.SetActive(true);

        // Display money earned and kill score in the win menu

        // Add all income from village houses into bank 
        CalculateVillageHouseIncome();

        // Set first day to false
        if (firstDay) { firstDay = false; }
    }

    public void StartNextWave()
    {
        if (startSpawn)
        {
            // check if the player has defeated all the enemies in the current wave
            if (enemiesDefeated == enemiesPerWave)
            {
                Debug.Log($"Wave {currentWave} started!");
                // Increment current wave
                currentWave++;
                // reset numbers of enemies defeated to zero
                enemiesDefeated = 0;
                // Reset enemies remaining 
                enemiesRemaining = enemiesPerWave - enemiesDefeated;
                
                // Spawn enemies for next wave
                StartCoroutine(SpawnWave());

                // update UI to reflect current wave and enemies remaining
                UIManager.Instance.CurrentWaveValue.text = currentWave.ToString();
                UIManager.Instance.EnemyRemainingValue.text = enemiesRemaining.ToString();
            }
        }
        
    }
    IEnumerator SpawnWave()
    {
        Debug.Log($"Starting wave {currentWave}");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(Random.Range(spawnIntervalLower, spawnIntervalUpper));
        }
    }

    private void SpawnEnemy()
    {
        // create a list to hold the allowed enemies and their probabilities
        List<(GameObject enemyPrefab, float probability)> allowedEnemies = new List<(GameObject, float)>();

        // add allowed enemies based on the current day number and their probabilities
        if (currentDay >= 5) // allow all enemies starting from day 5
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                allowedEnemies.Add((enemyPrefab, 1f)); // assign equal probability to each enemy
            }
        }
        else if (currentDay >= 3) // allow only certain enemies starting from day 3
        {
            allowedEnemies.Add((enemyPrefabs[0], 0.1f)); // first enemy prefab with 10% probability
            allowedEnemies.Add((enemyPrefabs[2], 0.9f)); // third enemy prefab with 30% probability
        }
        else // allow no enemies before day 3
        {
            allowedEnemies.Add((enemyPrefabs[0], 0.7f)); // first enemy prefab with 70% probability
            allowedEnemies.Add((enemyPrefabs[1], 0.3f)); // second enemy prefab with 30% probability
        }

        // calculate the total probability of all allowed enemies
        float totalProbability = allowedEnemies.Sum(e => e.probability);

        // get a random enemy prefab from the list of allowed enemies, weighted by their probabilities
        float randomValue = Random.Range(0f, totalProbability);
        GameObject randomEnemyPrefab = null;
        foreach ((GameObject enemyPrefab, float probability) in allowedEnemies)
        {
            randomValue -= probability;
            if (randomValue <= 0)
            {
                randomEnemyPrefab = enemyPrefab;
                break;
            }
        }

        // get a random spawn point from the array
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);

        // set the random offset within a range
        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        Vector3 spawnPosition = spawnPoints[randomSpawnPointIndex].transform.position + randomOffset;

        // spawn the random enemy from a random spawn point with the random offset
        Instantiate(randomEnemyPrefab, spawnPosition, Quaternion.identity);
    }
   
    // Call this on enemies that are dead
    public void MinusEnemiesCount()
    {
        enemiesDefeated++;
        enemiesRemaining = enemiesPerWave - enemiesDefeated;
        UIManager.Instance.EnemyRemainingValue.text = enemiesRemaining.ToString();
    }

    public void CheckWinLoseCondition()
    {
        // If player died respawn back to last save point
        if (player.GetComponent<Health>().IsDead())
        {
            // TO DO 
        }
        // If current day exceeds the max day then Win Game 
        if (currentDay > maxDays)
        {
            Debug.Log("You win!");
        }
        // If the last wall is destroyed then Game Over
        if (lastWall.IsDead())
        {
            UIManager.Instance.LoseDayMenu.SetActive(true);
        }
    }

    [System.Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public int cost;
    }
    
    // Object pooling way
    //private void SpawnEnemy()
    //{
    //    List<GameObject> allowedEnemies = new List<GameObject>();

    //    // add allowed enemies based on the current day number
    //    if (currentDay >= 5) // allow all enemies starting from day 5
    //    {
    //        allowedEnemies.AddRange(enemyPrefabs);
    //    }
    //    else if (currentDay >= 3) // allow only certain enemies starting from day 3
    //    {
    //        allowedEnemies.Add(enemyPrefabs[0]);
    //        allowedEnemies.Add(enemyPrefabs[1]);
    //        allowedEnemies.Add(enemyPrefabs[2]); 
    //        allowedEnemies.Add(enemyPrefabs[3]); 
    //    }
    //    else // allow no enemies before day 3
    //    {
    //        allowedEnemies.Add(enemyPrefabs[0]);
    //        allowedEnemies.Add(enemyPrefabs[1]); 
    //    }


    //    // get a random enemy prefab and a spawn point from the array
    //    int randomMonsterIndex = Random.Range(0, allowedEnemies.Count);
    //    int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);

    //    // set the random offset within a range
    //    Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
    //    Vector3 spawnPosition = spawnPoints[randomSpawnPointIndex].transform.position + randomOffset;


    //    // spawn a random enemy from a random spawn point
    //    Instantiate(allowedEnemies[randomMonsterIndex], spawnPosition, Quaternion.identity);
    //}

    //private void GenerateEnemies()
    //{
    //    // Create a temporary list of enemies to generate
    //    // in a loop to grab random enemy
    //    // check if can afford it, if so add it to our list & deduct the cost
    //    List<GameObject> generatedEnemies = new List<GameObject>();

    //    if (enemiesRemaining > 0)
    //    {
    //        int randEnemyId = Random.Range(0, enemiesPrefabs.Count);
    //        int randEnemyCost = enemiesPrefabs[randEnemyId].cost;

    //        if (enemiesRemaining - randEnemyCost >= 0)
    //        {
    //            generatedEnemies.Add(enemiesPrefabs[randEnemyId].enemyPrefab);
    //            enemiesRemaining -= randEnemyCost;
    //        }
    //        else if (enemiesRemaining <= 0)
    //        {

    //        }
    //        enemiesToSpawn.Clear();
    //    }
    //}
}
