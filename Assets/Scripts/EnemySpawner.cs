using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs = null;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = true;
    
    int waveNumber = 0;
    float backgroundScrollSpeed;
    bool isUpgrade = false;
    bool isWaveOver = false;

    bool finalWave = false;
    bool enemiesSpawned = false;
    bool spawnAsteroids = false;
    Coroutine spawner;

    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
        
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex ++)
        {
            finalWave = waveConfigs[waveIndex].GetIsFinalWave();
            waveNumber = waveIndex;
            name = waveConfigs[waveIndex].name;
            backgroundScrollSpeed = waveConfigs[waveIndex].GetBackgroundScrollSpeed();
            isUpgrade = waveConfigs[waveIndex].GetIsDropWeaponUpgrade();
            var currentWave = waveConfigs[waveIndex];

            isWaveOver = true;
            yield return new WaitForSeconds(currentWave.GetWaveStartCounter());
            isWaveOver = false;
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            
            if(finalWave == true)
            {
                enemiesSpawned = true;
            }
            yield return new WaitForSeconds(currentWave.GetTimeBetweenWaves());
            
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for(int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies() ; enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetBackgroundScrollSpeed()
    {
        return backgroundScrollSpeed;
    }

    public bool GetIsUpgrade()
    {
        return isUpgrade;
    }

    public bool GetIsWaveOver()
    {
        return isWaveOver;
    }

    public bool GetFinalWave()
    {
        return finalWave;
    }

    public bool GetAllEnemiesSpawned()
    {
        return enemiesSpawned;
    }

}
