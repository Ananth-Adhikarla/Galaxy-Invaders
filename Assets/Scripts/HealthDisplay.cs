using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TextMeshProUGUI healthText;
    Player player;

    //todo : add singleton pattern for multilevel
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }
        else
        {
            if (player.GetHealth() < 0)
            {
                var health = 0;
                healthText.text = health.ToString();
            }
            else
            {
                healthText.text = player.GetHealth().ToString();
            }
        }

        
    }
}
