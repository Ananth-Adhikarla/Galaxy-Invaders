using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 10;

    [Header("Shooting")]
    float shotCounter = 0;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile = null;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Sound Effects")]
    [SerializeField] GameObject hitVFX = null;
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip shootSound = null;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    [Header("Item Drop")]
    [SerializeField] GameObject[] itemDrops = null;
    [SerializeField] float dropChance = 0.15f;
    [SerializeField] [Range(0.0f,100.0f)] float minChance = 0.0f;
    [SerializeField] [Range(0.0f,100.0f)] float maxChance = 1.0f;
    private float pickupSpeed = 6f;
    bool isHealthPickupSpawnned = false;

    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    void Update()
    {
        CountDownAndShoot(); 
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        GameObject hit = Instantiate(hitVFX, transform.position, transform.rotation) as GameObject;
        damageDealer.Hit();
        Destroy(hit, 1f);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        scoreValue = (int) ( scoreValue * 0.279 + 5);
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation) as GameObject;
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        ChanceToDrop();
    }

    public void ChanceToDrop()
    {
        var shouldDrop = UnityEngine.Random.Range(minChance,maxChance);
        if (shouldDrop < dropChance)
        {
            GameObject item = itemDrops[UnityEngine.Random.Range(0, itemDrops.Length)];
            GameObject drop = Instantiate(item, transform.position, transform.rotation) as GameObject;
            drop.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -pickupSpeed);
        }
        isHealthPickupSpawnned = true;
    }

    public bool GetHealthSpawnned()
    {
        return isHealthPickupSpawnned;
    }
}
