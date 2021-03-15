using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{

    TextMeshProUGUI scoreText;
    GameSession gameSession;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        if(gameSession == null)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
        else
        {
            scoreText.text = "HI: " + gameSession.GetScore().ToString();
        }
        
    }
}
