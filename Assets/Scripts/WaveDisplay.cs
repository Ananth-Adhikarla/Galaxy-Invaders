using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WaveDisplay : MonoBehaviour
{
    TextMeshProUGUI waveText;
    EnemySpawner enemySpawner;
    float timer = 1.5f;
    int count;

    void Start()
    {
        waveText = GetComponent<TextMeshProUGUI>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        count = enemySpawner.GetWaveNumber();
        StartCoroutine(DisplayWaveText());
    }

    void Update()
    {
        if (count != enemySpawner.GetWaveNumber())
        {
            count = enemySpawner.GetWaveNumber();
            StartCoroutine(DisplayWaveText());
        }
        else
        {
            return;
        }
    }

    IEnumerator DisplayWaveText()
    {
        waveText.enabled = true;
        waveText.text = "Wave " + ( 1 + enemySpawner.GetWaveNumber() ).ToString();
        yield return new WaitForSeconds(timer);
        waveText.enabled = false;
    }


}
