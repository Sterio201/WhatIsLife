using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    Text text;

    /*private void Start()
    {
        text = GetComponent<Text>();

        if (PlayerPrefs.HasKey("PlayerScore"))
        {
            text.text = "—чет - " + PlayerPrefs.GetInt("PlayerScore").ToString(); 
        }
        else
        {
            text.text = "";
        }
    }*/

    private void OnEnable()
    {
        text = GetComponent<Text>();

        if (PlayerPrefs.HasKey("PlayerScore"))
        {
            text.text = "—чет - " + PlayerPrefs.GetInt("PlayerScore").ToString();
        }
        else
        {
            text.text = "";
        }
    }
}