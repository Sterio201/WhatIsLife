using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] GameObject panelLoad;
    [SerializeField] Image imageLoad;
    [SerializeField] int nomerScene;

    AsyncOperation asyncOperation;
    bool readyLoad;

    public void Shift()
    {
        readyLoad = false;

        if(panelLoad != null)
        {
            StartCoroutine(StartAnimLoad());
        }
        else
        {
            SceneManager.LoadScene(nomerScene);
        }
    }

    IEnumerator StartAnimLoad()
    {
        panelLoad.SetActive(true);
        yield return new WaitForSeconds(1f);

        asyncOperation = SceneManager.LoadSceneAsync(nomerScene);
        asyncOperation.allowSceneActivation = true;

        readyLoad = true;
    }

    private void Update()
    {
        if (readyLoad)
        {
            imageLoad.fillAmount = asyncOperation.progress;
        }
    }
}