using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMainMenu : MonoBehaviour
{
    [HideInInspector]
    public bool isReady;

    private void Start()
    {
        isReady = false;
    }

    public void ExitMenu()
    {
        if (isReady)
        {
            SceneManager.LoadScene(0);
        }
    }
}