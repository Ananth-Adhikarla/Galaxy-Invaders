using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int minDamage = 0;
    [SerializeField] int maxDamage = 0;
    int damage = 0;

    private void Start()
    {
        damage = Random.Range(minDamage, maxDamage);
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }

    public void SetDamage(int min , int max)
    {
        minDamage = min;
        maxDamage = max;
}
}
