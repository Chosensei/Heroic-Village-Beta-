using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Attributes; 
public class GameManager : Singleton<GameManager>
{
    public int MoneyInBank = 1000;   // Player's money in the game
    public int maxWaves = 5; // Maximum number of waves in battle phase
    public int currentWave = 0; // Current wave the player is fighting
    public int enemiesPerWave = 10; // Number of enemies per wave
    public float waveInterval = 2f;  // time between waves
    public int currentEnemies = 0; // Number of enemies spawned in current wave
    public int enemiesRemaining = 0;  // number of enemies left to defeat in the current wave
    public int enemiesDefeated = 0; // Total number of enemies defeated in battle phase

    public bool isBattlePhase = false; // Whether the game is currently in battle phase
    public bool playerWin = false;  // Whether player has won the current day
    public bool isGameOver = false; // Whether the player has lost the game
    public bool isRestPhase = true; // Whether the game is currently in rest phase
    public bool hasLeftVillage = false; // Whether player has left outside the village (To be applied to portal) 
    public bool hasReturnedToVillage = false;   // Whether player has returned to village (To be applied to portal) 
    public bool hasBeatenCurrentWave = false;   // Whether player has beaten all enemies in the current wave  
    public SkyboxController skyboxController;
    // Spawner variables
    public GameObject playervillageSpawnPoint; // the point where player will respawn at when they return from battle 
    public GameObject[] enemyPrefabs; // an array of different enemy prefabs to spawn
    public Transform[] spawnPoints; // an array of different spawn points to use
    //private List<GameObject> spawnedEnemies = new List<GameObject>(); // This is the pool of enemy objects that we will create
    private List<GameObject> enemyPool = new List<GameObject>(); // a pool of enemy gameobjects to reuse
    public int poolSize = 50; // This is the number of enemy objects that we will create in the pool
    public int baseEnemiesPerDay = 10; // the number of enemies to spawn on the first day
    public int enemiesPerDayIncrement; // the number of additional enemies to spawn each day

    public float playerMaxHealth = 100; 
    private float playerCurHealth = 0;  // current health of the player
    private Vector3 lastSavePoint = Vector3.zero;  // position where the player last rested
    private int currentDay = 0;  // current day in the game
    private int maxDays = 50;    // Max day in the game  
    private GameObject player; 
 
    void Start()
    {
        player = GameObject.FindWithTag("Player"); 
        // Initialize game state
        Init();

        // Start the game loop
        //GameLoop();

        // initialize the enemy pool with deactivated gameobjects
        for (int i = 0; i < poolSize; i++)
        {
            //GameObject enemy = Instantiate(enemyPrefabs[i]);
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    void Update()
    {

        // When player is in town start the rest phase
        if (hasReturnedToVillage && !isRestPhase)
        {
            StartRestPhase();
            Debug.Log("Resting Phase Started!");
        }

        // When player leaves town start the battle phase
        if (hasLeftVillage && isRestPhase)
        {
            StartBattlePhase();
            Debug.Log("Battle Phase Started!");
        }

        // Reset to last saved point when lose condition 
        if (isGameOver)
        {
            UIManager.Instance.LoseDayMenu.SetActive(true);
            // Pause game
            //Time.timeScale = 0;
            //ResetToLastSavePoint();
        }
        if (playerWin)
        {
            EndBattlePhase();
            // Pause game
            //Time.timeScale = 0;
            UIManager.Instance.WinDayMenu.SetActive(true);
        }
        // maintaining the Game Loop
        GameLoop();
    }
    //// Initialize 
    void Init()
    {
        currentDay = 1;
        currentWave = 1;
        enemiesRemaining = 0;
        // Modify this
        playerCurHealth = playerMaxHealth;
        lastSavePoint = transform.position;
        //StartRestPhase();
        StartNewDay();
        enemiesPerDayIncrement = 0;
        hasBeatenCurrentWave = false;
    }

    //// Start a new day 
    public void StartNewDay()
    {
        //FadeToBlack(); 
        // update Day value text 
        hasBeatenCurrentWave = false;
        playerCurHealth = playerMaxHealth;
        playerWin = false;
        //enemiesRemaining = maxWaves * enemiesPerWave;
        // Day phase: player can go outside town to battle
        Debug.Log($"Day {currentDay} started. Time to go outside and battle!");
        UIManager.Instance.CurrentDayValue.text = currentDay.ToString(); 
    }
    void GameLoop()
    {
        //enemiesRemaining = (maxWaves - currentWave + 1) * enemiesPerWave;

        // Update enemies count when enemies killed
        enemiesRemaining = currentEnemies - enemiesDefeated;
        UIManager.Instance.EnemyRemainingValue.text = enemiesRemaining.ToString();

        //check if a wave is cleared
        // Check if all enemies in wave are dead
        if (enemiesRemaining == 0 && currentWave < maxWaves)
        {
            // Start a new wave of enemies
            currentWave++;
            UIManager.Instance.CurrentWaveValue.text = currentWave.ToString();

            // Set flag to check if all enemies are defeated
            hasBeatenCurrentWave = true;
        }
        if (hasBeatenCurrentWave && enemiesRemaining > 0)
        {
            // Reset flag if not all enemies are defeated yet
            hasBeatenCurrentWave = false;
        }
        if (hasBeatenCurrentWave)
        {
            StartNextWave();
        }


        #region Check win/lose condition
        if (currentWave > maxWaves)
        {
            Debug.Log($"You survived day {currentDay}!");
            playerWin = true;
        }

        if (player.GetComponent<Health>().IsDead())
        {
            Debug.Log("You died");
            GameOver(); 
            return;
        }

        // Check for wall hp too 

        #endregion
    }
    //// Start rest phase
    void StartRestPhase()
    {
        isRestPhase = true;
        isBattlePhase = false;
        // TODO: Add any necessary logic for starting rest phase
        // Save game

        // Move time forward to next day
        currentDay++;

        if (currentDay > maxDays)
        {
            Debug.Log("Game completed. Congratulations!");
            // End the game
            //SceneManager.LoadScene("EndgameScene");
            return;
        }
        // Only testing until day 10 for now, to add more later
        else if (currentDay < 5)
        {
            enemiesPerDayIncrement += 2;
            maxWaves += 1;
        }
        else if (currentDay > 5 && currentDay < 10)
        {
            enemiesPerDayIncrement += 3;
            maxWaves += 2;
        }
    }

    // Start battle phase
    public void StartBattlePhase()
    {
        isRestPhase = false;
        isBattlePhase = true;
        currentWave = 1;
        UIManager.Instance.CurrentWaveValue.text = currentWave.ToString(); 
        enemiesDefeated = 0;
        StartNextWave();
        // TODO: Add any necessary logic for starting battle phase
    }
    // End battle phase
    void EndBattlePhase()
    {
        isBattlePhase = false;
        isRestPhase = true;
        // TODO: Add any necessary logic for ending battle phase

    }
    // Start next wave
    void StartNextWave()
    {
        int enemiesToSpawn = baseEnemiesPerDay + (currentDay - 1) * enemiesPerDayIncrement;
        enemiesPerWave = enemiesToSpawn; 
        currentEnemies = enemiesPerWave;
        // Spawn enemies for next wave
        StartCoroutine(SpawnWave());
        // Reset
        hasBeatenCurrentWave = false;
    }

    // Handle player death
    public void GameOver()
    {
        isGameOver = true;
    }
    // Handle enemy defeat
    public void EnemyDefeated()
    {
        enemiesDefeated++; 
        enemiesRemaining--;
    }

    IEnumerator SpawnWave()
    {
        Debug.Log($"Starting wave {currentWave}");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            currentEnemies++;
            yield return new WaitForSeconds(waveInterval);
        }
    }

    // TODO: spawn an enemy object in the game world and decrement enemiesRemaining

    private void SpawnEnemy()
    {
        // get a random enemy prefab from the array
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = null;

        // check if there's an available enemy in the pool to reuse
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy && enemyPool[i].tag == enemyPrefabs[randomIndex].tag)
            {
                enemy = enemyPool[i];
                break;
            }
        }
        // if no enemy is available in the pool, instantiate a new one
        if (enemy == null)
        {
            enemy = Instantiate(enemyPrefabs[randomIndex]);
            enemyPool.Add(enemy);
        }
        // get a random spawn point from the array
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // set the enemy's position and rotation to the spawn point's position and rotation
        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;

        // activate the enemy
        enemy.SetActive(true);
    }



    //// Reset game state to last save point
    //void ResetToLastSavePoint()
    //{
    //    // Reset game state to last saved point
    //    transform.position = lastSavePoint;
    //    playerCurHealth = playerMaxHealth;
    //    isGameOver = false;
    //    // Reset to rest phase
    //    StartRestPhase();
    //}

    //// Save game state
    //void SaveGame()
    //{
    //    // TODO: Save game state
    //    Debug.Log("Game saved.");
    //    PlayerPrefs.SetInt("currentDay", currentDay);
    //    PlayerPrefs.SetInt("currentWave", currentWave);
    //    PlayerPrefs.SetInt("playerWin", playerWin ? 1 : 0);
    //    PlayerPrefs.Save();
    //    StartNewDay();
    //}

    //// Load game state
    //void LoadGame()
    //{
    //    // TODO: Load game state
    //    currentDay = PlayerPrefs.GetInt("currentDay", 1);
    //    currentWave = PlayerPrefs.GetInt("currentWave", 0);
    //    playerWin = PlayerPrefs.GetInt("playerWin", 0) == 1;
    //    Debug.Log("Game loaded. Day " + currentDay + " Wave " + currentWave);
    //}

}
