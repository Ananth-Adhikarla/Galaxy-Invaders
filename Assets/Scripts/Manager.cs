using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    //[SerializeField] GameObject gameSessionPrefab = null;
    [SerializeField] GameObject playerShipPrefab = null;
    [SerializeField] Vector3 playerShipOffset = new Vector3(0,0,0);
    GameObject playerShipInstance;
    Player player;

    //GameObject gameSessionInstance;
    GameSession gameSession;

    EnemySpawner enemySpawner;

    Vector3 playerPosOnLevelLoad = new Vector3(0, -9, 0);

    int currentHealth = 0;
    int score = 0;

    int currentMin = 0;
    int currentMax = 0;
    bool isRestart = false;
    //bool isPlayerDead = false;
    //bool isLastLevel = false;

    private void Awake()
    {
        int numberGameSessions = FindObjectsOfType<Manager>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();

        playerShipInstance = Instantiate(playerShipPrefab, transform.position + playerShipOffset, transform.rotation) as GameObject;
        player = playerShipInstance.GetComponent<Player>();

        enemySpawner = Transform.FindObjectOfType<EnemySpawner>();
        currentHealth = player.GetHealth();
    }

    void Update()
    {
        if (enemySpawner == null) { enemySpawner = Transform.FindObjectOfType<EnemySpawner>(); }

        if(SceneManager.GetActiveScene().name == "Game Over" || SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        CheckForLastWave();
        if (player.PlayerIsDead() == true)
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndGameRun();
        }
    }

    private void RestartGame()
    {
        //player.GetIsGameRestart() == true && player != null
        //FindObjectsOfType<Player>().Length == 0

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            score = 0;
            currentMin = 100;
            currentMax = 110;
        }
        if(gameSession == null)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
        FindObjectOfType<Level>().RestartGame(SceneManager.GetActiveScene().buildIndex);
        player.SetCurrentHealth(currentHealth);
        gameSession.ResetScore(score);

        player.SetWeaponDamageMinimum(currentMin);
        player.SetWeaponDamageMaximum(currentMax);
    }

    public void EndGameRun()
    {
        if(gameSession == null)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
        score = gameSession.GetScore();
        gameSession.SetIsGameQuit(true);
        enemySpawner.StopAllCoroutines();
        var enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Die();
        }
        FindObjectOfType<Level>().LoadGameOver();
        gameSession.ResetScore(score);
        gameSession.SetIsGameQuit(false);
    }

    private void CheckForLastWave()
    {
        if(enemySpawner == null)
        {
            enemySpawner = Transform.FindObjectOfType<EnemySpawner>();
        }
        if (gameSession == null)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
        if (enemySpawner.GetFinalWave() == true && enemySpawner.GetAllEnemiesSpawned() == true )
        {
            var enemiesLeft = FindObjectsOfType<Enemy>().Length;
            if (enemiesLeft == 0)
            {
                currentMin = player.GetWeaponDamageMinimum();
                currentMax = player.GetWeaponDamageMaximum();
                LoadNextScene();
                score = gameSession.GetScore();
                player.SetCurrentHealth(currentHealth);    
            }   
        }
    }

    private void LoadNextScene()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene + 1 < SceneManager.sceneCountInBuildSettings - 1)
        {
            FindObjectOfType<Level>().LoadNextLevel(currentScene + 1);
            
        }
        else if (currentScene + 1 == SceneManager.sceneCountInBuildSettings - 1)
        {
            FindObjectOfType<Level>().LoadGameOver();
        }
        else
        {
            return;
        }
    }

    public GameObject GetPlayerShipInstance()
    {
        return playerShipInstance;
    }

    public void RecreateObjects()
    {
        Start();
    }

    public bool GetIsRestart()
    {
        return isRestart;
    }
}
