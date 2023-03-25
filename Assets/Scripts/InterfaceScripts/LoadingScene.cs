using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] GameObject panelLoad;
    [SerializeField] Image imageLoad;

    AsyncOperation asyncOperation;
    bool readyLoad;

    public void Shift(int id)
    {
        gameObject.SetActive(true);
        readyLoad = false;

        if(panelLoad != null)
        {
            StartCoroutine(StartAnimLoad(id));
        }
        else
        {
            SceneManager.LoadScene(id);
        }
    }

    IEnumerator StartAnimLoad(int id_scene)
    {
        panelLoad.SetActive(true);
        yield return new WaitForSeconds(1f);

        asyncOperation = SceneManager.LoadSceneAsync(id_scene);
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