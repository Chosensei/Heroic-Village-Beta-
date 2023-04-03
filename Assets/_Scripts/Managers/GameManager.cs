using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public float playerMaxHealth = 100; 
    private float playerCurHealth = 0;  // current health of the player
    private Vector3 lastSavePoint = Vector3.zero;  // position where the player last rested
    private int currentDay = 0;  // current day in the game
    private int maxDays = 50;    // Max day in the game  

    //void Start()
    //{
    //    // Initialize game state
    //    Init();
    //    // Start the game loop
    //    GameLoop();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // When player is in town start the rest phase
    //    if (hasReturnedToVillage && !isRestPhase)
    //    {
    //        StartRestPhase();
    //    }

    //    // When player leaves town start the battle phase
    //    if (hasLeftVillage && isRestPhase)
    //    {
    //        StartBattlePhase();
    //    }

    //    // Reset to last saved point when lose condition 
    //    if (isGameOver)
    //    {
    //        ResetToLastSavePoint();
    //    }
    //    if (playerWin)
    //    {
    //        EndBattlePhase();
    //    }
    //    // maintaining the Game Loop
    //    GameLoop();
    //}
    //// Initialize 
    //void Init()
    //{
    //    currentDay = 1;
    //    currentWave = 1;
    //    enemiesRemaining = 0;
    //    playerCurHealth = playerMaxHealth;
    //    lastSavePoint = transform.position;
    //    StartRestPhase();
    //}

    //// Start a new day 
    //public void StartNewDay()
    //{
    //    //FadeToBlack(); 
    //    playerCurHealth = playerMaxHealth;
    //    playerWin = false;
    //    enemiesRemaining = maxWaves * enemiesPerWave;
    //    // Day phase: player can go outside town to battle
    //    Debug.Log($"Day {currentDay} started. Time to go outside and battle!");

    //}
    //void GameLoop()
    //{

    //    // Start a new wave of enemies
    //    currentWave++;
    //    enemiesRemaining = (maxWaves - currentWave + 1) * enemiesPerWave;

    //    // Update enemies count when enemies killed
    //    enemiesRemaining = currentEnemies - enemiesDefeated;



    //    #region Check win/lose condition

    //    if (playerCurHealth <= 0)
    //    {
    //        Debug.Log("You died");
    //        PlayerDead();
    //        return;
    //    }

    //    // Check for wall hp too 
    //    if (currentWave > maxWaves)
    //    {
    //        Debug.Log($"You survived day {currentDay}!");
    //        playerWin = true;
    //    }
    //    #endregion
    //}
    //// Start rest phase
    //void StartRestPhase()
    //{
    //    isRestPhase = true;
    //    isBattlePhase = false;
    //    // TODO: Add any necessary logic for starting rest phase
    //    // Save game
    //    // Move time forward to next day
    //    currentDay++;
    //    if (currentDay > maxDays)
    //    {
    //        Debug.Log("Game completed. Congratulations!");
    //        // End the game
    //        //SceneManager.LoadScene("EndgameScene");
    //        return;
    //    }
    //}

    //// Start battle phase
    //void StartBattlePhase()
    //{
    //    isRestPhase = false;
    //    isBattlePhase = true;
    //    currentWave = 1;
    //    enemiesDefeated = 0; 
    //    StartNextWave();
    //    // TODO: Add any necessary logic for starting battle phase
    //}
    //// End battle phase
    //void EndBattlePhase()
    //{
    //    isBattlePhase = false;
    //    isRestPhase = true;
    //    // TODO: Add any necessary logic for ending battle phase
    //}
    //// Start next wave
    //void StartNextWave()
    //{
    //    currentEnemies = 0;
    //    StartCoroutine(SpawnWave());
    //    // TODO: Spawn enemies for next wave
    //}

    //// Handle player death
    //public void PlayerDead()
    //{
    //    isGameOver = true;
    //}
    //// Handle enemy defeat
    //public void EnemyDefeated()
    //{
    //    enemiesRemaining--;
    //}


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
    //IEnumerator SpawnWave()
    //{
    //    Debug.Log($"Starting wave {currentWave}");
    //    for (int i = 0; i < enemiesPerWave; i++)
    //    {
    //        SpawnEnemy();
    //        currentEnemies++;
    //        yield return new WaitForSeconds(waveInterval);
    //    }
    //}

    //void SpawnEnemy()
    //{
    //    // TODO: spawn an enemy object in the game world and decrement enemiesRemaining
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
