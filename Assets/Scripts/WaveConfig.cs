using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Header("Prefabs")]
    [SerializeField] GameObject enemyPrefab = null;
    [SerializeField] GameObject pathPrefab = null;

    [Header("Timer")]
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] float waveStartCounter = 2f;
    [SerializeField] float spawnRandomFactor = 0.3f; // not used

    [Header("Enemies Info")]
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    [Header("Background info")]
    [SerializeField] float backgroundScrollSpeed = 0f;

    [Header("Final Wave")]
    [SerializeField] bool isFinalWave = false;

    [Header("Spawn Weapon upgrade")]
    [SerializeField] bool isDropWeaponUpgrade = false;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }

    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach(Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    public float GetTimeBetweenSpawns () { return timeBetweenSpawns ; }

    public float GetWaveStartCounter() { return waveStartCounter; }

    public float GetSpawnRandomFactor() { return spawnRandomFactor; }

    public int GetNumberOfEnemies() { return numberOfEnemies; }

    public float GetMoveSpeed() { return moveSpeed ; }

    public float GetTimeBetweenWaves() { return timeBetweenWaves; }

    public float GetBackgroundScrollSpeed() { return backgroundScrollSpeed; }

    public bool GetIsDropWeaponUpgrade() { return isDropWeaponUpgrade; }

    public bool GetIsFinalWave() { return isFinalWave; }

}
