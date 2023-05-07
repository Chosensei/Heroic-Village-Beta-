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
    private int maxDays = 30;    // Max day in the game  
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
    public int totalEnemiesKilled = 0;  // Track the number of enemies killed in the game
    public int totalBounty;
    private int totalRent; 
    //Added function to calculate income from village houses
    public void CalculateVillageHouseIncome()
    {
        //GameObject[] villageHouses = GameObject.FindGameObjectsWithTag("VillageHouse");
        //villageHouses = GameObject.FindGameObjectsWithTag("VillageHouse");
        GameObject[] villageHouses = GameObject.FindGameObjectsWithTag("VillageHouse");
        if (villageHouses == null) return;
        else
        {
            foreach (GameObject villageHouse in villageHouses)
            {
                var vh = villageHouse.GetComponent<TowerLevelSwitch>();
                //MoneyInBank += vh.baseIncome;
                //UIManager.Instance.moneyText.text = MoneyInBank.ToString();

                // Store all rental income 
                totalRent += vh.baseIncome;
            }
        }

    }
    public void CalculateEnemyBounties()
    {

    }

    /* BUTTON REFERENCE METHODS */
    public void StartBattle()
    {
        Debug.Log("Battle Started!");
        // Set battleStarted to true
        battleStarted = true;
        // Show Battle Menu
        UIManager.Instance.BattleMenu.SetActive(true);
        // Show minimap
        UIManager.Instance.Minimap.SetActive(true);
        // Hide bank UI
        UIManager.Instance.BankUI.SetActive(false);
        // Play battle sfx
        SoundManager.Instance.PlaySfx("FirstWave");

        // set current wave to 1
        currentWave = 1;
        // update UI to reflect current wave 
        UIManager.Instance.CurrentWaveValue.text = currentWave.ToString();
        // set numbers defeated to zero
        enemiesDefeated = 0;
        // enemies remaining to enemies per wave minus enemies defeated 
        enemiesRemaining = enemiesPerWave - enemiesDefeated;
        UIManager.Instance.EnemyRemainingValue.text = enemiesRemaining.ToString();
        // reset previous day total enemy kills, rent and bounties
        totalEnemiesKilled = 0;
        totalBounty = 0;
        totalRent = 0; 
        // WORKS! - TEST TO TRY SPAWNING STRAIGHTAWAY 
        StartCoroutine(SpawnWave());
    }
    public void ReturnToTown()
    {
        Time.timeScale = 1f; // resume the game
        // Transport player back to town 
        player.transform.position = playerTownSpawnPoint.position;
        hasLeftTown = false; 
        Debug.Log("Resting Started!");
        // Deactivate Win menu
        UIManager.Instance.WinDayMenu.SetActive(false);
        // Deactivate minimap
        UIManager.Instance.Minimap.SetActive(false);
        SoundManager.Instance.sfxSource.Stop();
        SoundManager.Instance.PlayMusic("Village_Theme");
        skyboxController.ToggleSkybox();    // Start night time
    }
    public void EnterBuildMode()
    {
        UIManager.Instance.BuildButton.SetActive(false);
        UIManager.Instance.ExitBuildButton.SetActive(true); 
        isBuilding = true;  // Switch camera back to Buildcam

    }
    public void ExitBuildMode()
    {
        UIManager.Instance.ExitBuildButton.SetActive(false);
        UIManager.Instance.BuildButton.SetActive(true);
        isBuilding = false;         // Change camera back to Towncam
    }
    /* AFTER SAVING */
    public void EndCurrentDay()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // After saving the game, start next day 
            InitializeNextDay();
        }
        
        // Restore player HP to full 
        player.GetComponent<Health>().RestoreFullHealth(); 
    }
    void Start()
    {
        InitializeDayOne();
        player = GameObject.FindWithTag("Player");  

    }

    void Update()
    {
        // Check for cheat key to fast forward to the last day
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            InitializeNextDay(cheatMode: true);
        }
        // Debug purposes
        if (Input.GetKeyDown(KeyCode.K))
        {
            EndBattle(); 
        }
        // Check to enable / disable StartBattle & BuildButton
        if (hasLeftTown)
        {
            if (!battleStarted)
            {
                UIManager.Instance.StartBattleButton.SetActive(true);
            }
            else
            {
                UIManager.Instance.StartBattleButton.SetActive(false);
            }
            // Enable Build Button
            UIManager.Instance.BuildButton.SetActive(false);
        }
        else { UIManager.Instance.BuildButton.SetActive(true); }

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
        }
        // Start Next Wave
        StartNextWave();
        // Check game status
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
        // Set starting money 
        UIManager.Instance.moneyText.text = MoneyInBank.ToString(); 
        UIManager.Instance.CurrentDayValue.text = currentDay.ToString();
        UIManager.Instance.CurrentWaveValue.text = "-";
        UIManager.Instance.EnemyRemainingValue.text = "-";
        // Hide buttons
        UIManager.Instance.StartBattleButton.SetActive(false);
        UIManager.Instance.BattleMenu.SetActive(false);
        UIManager.Instance.Minimap.SetActive(false);
        //SoundManager.Instance.PlayMusic("Village_Theme");
    }
    public void InitializeNextDay(bool cheatMode = false)
    {
        if (cheatMode)
        {
            currentDay = maxDays; // Move forward to the last day
        }
        else
        {
            currentDay++;   // Increase current day by 1
        }

        // Update UI to reflect Current Day
        UIManager.Instance.CurrentDayValue.text = currentDay.ToString();

        // Set max waves and enemies per wave based on the current day count
        if (currentDay > 3 && currentDay <= 7) { maxWaves = 6; enemiesPerWave = 4; }
        if (currentDay > 7 && currentDay <= 11) { maxWaves = 7; enemiesPerWave = 5; }
        if (currentDay > 11 && currentDay <= 16) { maxWaves = 8; enemiesPerWave = 6; }
        if (currentDay > 16 && currentDay <= 21) { maxWaves = 9; enemiesPerWave = 7; }
        if (currentDay > 21) { maxWaves = 10; enemiesPerWave = Random.Range(7,10); }

        skyboxController.ToggleSkybox();    // Start a new daytime
        SoundManager.Instance.PlayMusic("Village_Theme");
    }

    public void EndBattle()
    {
        // Set battleStarted to false
        battleStarted = false;

        // Stop spawning enemies
        StopCoroutine(SpawnWave());

        // Disable Battle UIs
        UIManager.Instance.BattleMenu.SetActive(false);
        UIManager.Instance.Minimap.SetActive(false);
        // Show bank UI
        UIManager.Instance.BankUI.SetActive(true);
        UIManager.Instance.CurrentWaveValue.text = "-";
        UIManager.Instance.EnemyRemainingValue.text = "-";

        // Activate Win menu
        UIManager.Instance.WinDayMenu.SetActive(true);

        // Add all income from village houses into bank 
        CalculateVillageHouseIncome();

        // Village houses income + Enemies kill rewards
        int totalRentBountyIncome = totalBounty + totalRent;
        // Add total sum into bank 
        MoneyInBank += totalRentBountyIncome;
        // Update Money UI 
        UIManager.Instance.moneyText.text = MoneyInBank.ToString();

        // Display money earned and kill score in the win menu
        UIManager.Instance.TotalMoneyEarnedValue.text = totalRentBountyIncome.ToString();
        UIManager.Instance.TotalKillsValue.text = totalEnemiesKilled.ToString();

        // Play victory music
        SoundManager.Instance.musicSource.Stop(); 
        SoundManager.Instance.PlaySfx("WinDay");

        // Set first day to false
        if (firstDay) { firstDay = false; }

        Time.timeScale = 0f; // pause the game 
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
        // If current day exceeds the max day then Win Game 
        if (currentDay > maxDays)
        {
            Debug.Log("You win!");
            SoundManager.Instance.PlayMusic("Ending_Theme");
            // Show ending 
            MenuUIController.Instance.LoadEndingScene(); 
        }
        // If the last wall is destroyed then Game Over
        if (lastWall.IsDead())
        {
            UIManager.Instance.LoseDayMenu.SetActive(true);
        }
    }

}
