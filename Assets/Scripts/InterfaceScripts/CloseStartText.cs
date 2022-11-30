using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseStartText : MonoBehaviour
{
    [SerializeField] EventsLabirintian eventsLabirintian;
    bool readyClick;

    private void Start()
    {
        readyClick = false;
        StartCoroutine(ActivateClick());
    }

    IEnumerator ActivateClick()
    {
        yield return new WaitForSeconds(7f);
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