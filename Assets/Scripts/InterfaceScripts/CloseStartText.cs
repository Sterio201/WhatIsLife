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
            startText.text = "� ����� ����� �����...";
            time = 2f;
        }
        else if (eventsLabirintian.typePlay == TypePlay.Story)
        {
            startText.text = "��������� ��,  ���� ���, ��� ����� ��� ����� ������� ��������. ���-�� ���������� ������ �� ��� ��������� � ������� ���� � �������� �����, ��� ������������ �� ���� ��������������� ����� �� ���������. ��������� � ��, ��� ����� �������.�� ������ �� ����� �������� �� ����� ���� ���? ������ ��� ����� ������� �� ��������� �� ���� ������...";
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