using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponUpgrade : MonoBehaviour
{
    [SerializeField] GameObject weaponUpgrade = null;
    [SerializeField] Vector3 offsetPos = new Vector3(0f,0f,0f);
    float projectileSpeed = 7f;
    EnemySpawner enemySpawner;
    int counter = -1;
    int index = -1;
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();   
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game Over" || SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        else
        {
            SpawnWeaponUpgrade();
        }
        
    }

    public void SpawnWeaponUpgrade()
    {
        if(enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        if (enemySpawner.GetIsUpgrade() && enemySpawner.GetIsWaveOver() && (enemySpawner.GetWaveNumber() != counter))
        {
            GameObject weapon = Instantiate(weaponUpgrade, transform.position + offsetPos, Quaternion.identity) as GameObject;
            weapon.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            counter = enemySpawner.GetWaveNumber();
            index++;
        }
    }

    public int GetIndex()
    {
        return index;
    }
}
