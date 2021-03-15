using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
   
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    [SerializeField] int healthToAdd = 35;
    int maxHealth = 250;

    [Header("Player Audio")]
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip shootSound = null;
    [SerializeField][Range(0, 1)] float shootSoundVolume = 0.25f;

    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] float boostTimer = 5f;
    [SerializeField] GameObject laserPrefab = null;
    float projectileSpeedTemp;
    float projectileFiringPeriodTemp;
    float projectileSpeedBoost = 30f;
    float projectileFiringPeriodBoost = 0.05f;
    bool isBoosted = false;
    Coroutine projectileBooster;

    //level 1 - 3
    //level 2 - 3
    //level 3 - 3
    //level 4 - 1
    int[] upgradeMin = new int[] { 110, 125, 140, 170, 190, 220, 250, 280, 325, 375 };
    int[] upgradeMax = new int[] { 120, 140, 160, 185, 210, 240, 275, 310, 350, 420 };
    int currentMin = 100;
    int currentMax = 110;

    [Header("Shield")]
    [SerializeField] GameObject protectShieldPrefab = null;
    [SerializeField] float shieldTimer = 10f;
    bool isShielded = false;
    Coroutine damageShielder;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;

    float yMin;
    float yMax;

    WeaponUpgrade weaponUpgrade;

    int levelCount = -1;

    bool playerIsDead = false;

    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
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
        weaponUpgrade = FindObjectOfType<WeaponUpgrade>();
        SetUpMoveBoundaries();

        SetStartValues();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != levelCount)
        {
            StopAllCoroutines();
            StopBulletBooster();
            StopShield();
            levelCount = SceneManager.GetActiveScene().buildIndex;
        }
        Move();
        Fire();
    }

    private void SetStartValues()
    {
        laserPrefab.GetComponent<SpriteRenderer>().color = Color.white;
        projectileSpeedTemp = projectileSpeed;
        projectileFiringPeriodTemp = projectileFiringPeriod;
        laserPrefab.GetComponent<DamageDealer>().SetDamage(currentMin, currentMax);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);

        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);

    }

    private void CheckCollision(Collider2D other)
    {
        if (other.gameObject.tag == "Health Pickup")
        {
            health += healthToAdd;
            if (health >= maxHealth)
            {
                health = maxHealth;
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Weapon Upgrade")
        {
            UpgradeLaser();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Weapon Speed" )
        {
            if(isBoosted == false)
            {
                projectileBooster = StartCoroutine(IncreaseProjectileSpeed(other));
            }
            else if (isBoosted == true)
            {
                //print("stopping boost coroutine");
                StopCoroutine(projectileBooster);
                StartCoroutine(IncreaseProjectileSpeed(other));
            }
        }


        if (other.gameObject.tag == "Protect Shield" )
        {
            if(isShielded == false)
            {
                damageShielder = StartCoroutine(StartProtectShield(other));
            }
            else if (isShielded == true)
            {
               //print("stopping shield coroutine");
                StopCoroutine(damageShielder);
                StartCoroutine(StartProtectShield(other));
            }
        }

    }

    IEnumerator IncreaseProjectileSpeed(Collider2D other)
    {
        isBoosted = true;
        Destroy(other.gameObject);
        
        //Change Speed and firing rate to boosted rate
        projectileSpeed = projectileSpeedBoost;
        projectileFiringPeriod = projectileFiringPeriodBoost;

        // change color to give player visual feedback
        laserPrefab.GetComponent<SpriteRenderer>().color = new Color(99f, 255f, 0f);
        yield return new WaitForSeconds(boostTimer);

        // after timer is over back to default settings
        projectileSpeed = projectileSpeedTemp;
        projectileFiringPeriod = projectileFiringPeriodTemp;
        laserPrefab.GetComponent<SpriteRenderer>().color = Color.white;
        isBoosted = false;
    }

    IEnumerator EndProjectileSpeed()
    {
        isBoosted = true;

        //Change Speed and firing rate to boosted rate
        projectileSpeed = projectileSpeedBoost;
        projectileFiringPeriod = projectileFiringPeriodBoost;

        // change color to give player visual feedback
        laserPrefab.GetComponent<SpriteRenderer>().color = new Color(99f, 255f, 0f);

        yield return new WaitForSeconds(0);

        // after timer is over back to default settings
        projectileSpeed = projectileSpeedTemp;
        projectileFiringPeriod = projectileFiringPeriodTemp;
        laserPrefab.GetComponent<SpriteRenderer>().color = Color.white;
        isBoosted = false;
    }

    IEnumerator StartProtectShield(Collider2D other)
    {
        isShielded = true;
        Destroy(other.gameObject);
        protectShieldPrefab.SetActive(true);
        yield return new WaitForSeconds(shieldTimer);
        protectShieldPrefab.SetActive(false);
        isShielded = false;
    }

    IEnumerator EndProtectShield()
    {
        isShielded = true;
        protectShieldPrefab.SetActive(true);
        yield return new WaitForSeconds(0);
        protectShieldPrefab.SetActive(false);
        isShielded = false;
    }

    private void UpgradeLaser()
    {
        var index = weaponUpgrade.GetIndex();
        laserPrefab.GetComponent<DamageDealer>().SetDamage(upgradeMin[index], upgradeMax[index]);
        currentMin = upgradeMin[index];
        currentMax = upgradeMax[index];
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }


    IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);   
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0 )
        {
            playerIsDead = true;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public bool PlayerIsDead()
    {
        return playerIsDead;
    }

    public void StopShield()
    {
        if (damageShielder != null)
        {
            print("stopping shield end level");
            StopCoroutine(damageShielder);
            damageShielder = StartCoroutine(EndProtectShield());
        }
    }

    public void StopBulletBooster()
    {
        if(projectileBooster != null)
        {
            print("stopping bullet end level");
            StopCoroutine(projectileBooster);
            projectileBooster = StartCoroutine(EndProjectileSpeed());
        } 
    }

    public int GetWeaponDamageMinimum()
    {
        return currentMin;
    }

    public int GetWeaponDamageMaximum()
    {
        return currentMax;
    }

    public void SetWeaponDamageMinimum(int min)
    {
        currentMin = min;
    }

    public void SetWeaponDamageMaximum(int max)
    {
        currentMax = max;
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public void SetCurrentHealth(int healthToModify)
    {
        health = healthToModify;
    }

    public void ResetCurrentHealth()
    {
        health = maxHealth;
    }

    public void SetLevelCount()
    {
        levelCount -= 1;
    }

    public int GetHealth()
    {
        return health;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
