using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelDisplay : MonoBehaviour
{

    TextMeshProUGUI levelText;
    float timer = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(DisplayLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DisplayLevel()
    {
        levelText.enabled = true;
        levelText.text = SceneManager.GetActiveScene().name;
        yield return new WaitForSeconds(timer);
        levelText.enabled = false;
    }
}
