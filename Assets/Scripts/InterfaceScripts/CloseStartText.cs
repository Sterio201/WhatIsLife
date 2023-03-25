using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseStartText : MonoBehaviour
{
    [SerializeField] EventsLabirintian eventsLabirintian;
    [SerializeField] Text startText;
    [SerializeField] GameObject continueText;

    bool readyClick;
    float time;

    private void Start()
    {
        if (eventsLabirintian.typePlay == TypePlay.Arcade)
        {
            startText.text = "И снова новая жизнь...";
            time = 2f;
        }
        else if (eventsLabirintian.typePlay == TypePlay.Story)
        {
            startText.text = "Представь же,  друг мой, что жизнь это некий большой лабиринт. Кто-то бесконечно бродит по его коридорам в поисках пути к заветной мечте, что представляет из себя непосредственно выход из лабиринта. Находятся и те, кто выход находят.Но почему же тогда таковыми не могут быть все? Потому что вечно бродить по лабиринту не дано никому...";
            PlayerPrefs.SetString("FirstPlay", "play");
            time = 5f;
        }
        readyClick = false;
       
        StartCoroutine(ActivateClick());
    }

    IEnumerator ActivateClick()
    {
        yield return new WaitForSeconds(time);
        continueText.SetActive(true);
        readyClick = true;
    }

    public void ClickPanel()
    {
        if (readyClick)
        {
            gameObject.SetActive(false);
            eventsLabirintian.ready = true;
        }
    }
}