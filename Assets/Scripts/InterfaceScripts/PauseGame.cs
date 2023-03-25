using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    bool isPause;

    public void Pause()
    {
        if (isPause)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

        isPause = !isPause;
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}