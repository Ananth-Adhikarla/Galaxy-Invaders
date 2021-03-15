using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attribution : MonoBehaviour
{
    bool isActive = false;

    public void OnMouseDown()
    {
        if (!isActive)
        {
            isActive = true;
            gameObject.SetActive(true);
        }
        else
        {
            // not sure why need this for game over page
            isActive = false;
            gameObject.SetActive(false);
        } 
    }

    public void ControlsToggle()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

}
