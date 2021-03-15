using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;
    bool isQuit = false;

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

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        if(isQuit == true)
        {
            return;
        }
        else
        {
            score += scoreValue;
        }
        
    }

    public void ResetScore(int value)
    {
        score = value;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
        score = 0;
    }

    public void SetIsGameQuit(bool quit)
    {
        isQuit = quit;
    }
}
