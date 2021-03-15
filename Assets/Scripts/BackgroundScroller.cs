using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    Material myMaterial;
    Vector2 offset;
    EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0f, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().name == "Game Over")
        {
            myMaterial.mainTextureOffset += offset * Time.deltaTime;
        }
        if (enemySpawner == null) { return; }
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
        ChangeScrollSpeed();
        
    }

    private void ChangeScrollSpeed()
    {
        var speed = enemySpawner.GetBackgroundScrollSpeed();

        if (backgroundScrollSpeed != speed)
        { 
            backgroundScrollSpeed = speed;
            offset = new Vector2(0f, backgroundScrollSpeed);
            //myMaterial.mainTextureOffset += offset * Time.deltaTime;        
        }
        
    }
}
