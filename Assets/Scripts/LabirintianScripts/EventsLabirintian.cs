using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Events;

public class EventsLabirintian : MonoBehaviour
{
    [SerializeField] float timerEvent;
    [SerializeField] float timerLife;
    [SerializeField] ExitMainMenu mainMenu;
    [SerializeField] Transform playerPos;
    [SerializeField] List<float> chance;
    [SerializeField] List<string> textEvents;
    [SerializeField] Text eventText;
    [SerializeField] Animator darkAnim;
    [SerializeField] PlayableDirector director;
    public TypePlay typePlay;

    Vector2 startPlayerPos;
    MazeManager mazeManager;
    float currentTimeEvent;

    [HideInInspector]
    public float currentTimeLife;
    [HideInInspector]
    public static Vector3 posFinal;

    int IDevent;
    int countIvent;
    int age;

    [HideInInspector]
    public bool ready;

    private void Start()
    {
        mazeManager = GetComponent<MazeManager>();

        startPlayerPos = playerPos.position;

        currentTimeEvent = timerEvent;
        currentTimeLife = timerLife;

        countIvent = 0;
        age = 0;

        if(typePlay == TypePlay.Story)
        {
            textEvents[0] = "����� ����� ������������ ��� ������ ����...";
        }

        textEvents.Add("������� ����� �����������");

        ready = false;
    }

    // ��������� �������
    private void Update()
    {
        if(ready)
        {
            if (currentTimeEvent > 0f)
            {
                currentTimeEvent -= Time.deltaTime;
            }
            else
            {
                if(countIvent >= 2 || typePlay == TypePlay.Arcade)
                {
                    IDevent = IDrandomevent(chance);
                    StartCoroutine(ActivateEvent(IDevent));
                }
                else if(typePlay == TypePlay.Story)
                {
                        if (countIvent == 0)
                        {
                            eventText.text = "�� �������� ����, ��������� ��� ������ ������������ � ���������� ������.";
                        }
                        else if (countIvent == 1)
                        {
                            eventText.text = "�� ����� ������� �������� ���, ��� ����� ���.";
                        }

                        countIvent++;
                        eventText.gameObject.GetComponent<Animator>().Play("ShowEventTitle");
                }

                currentTimeEvent = timerEvent;
            }
        }
    }

    // ��������� �������� ����
    private void FixedUpdate()
    {
        if (ready)
        {
            if((currentTimeLife <= 0 && typePlay == TypePlay.Arcade) || Vector3.Distance(playerPos.position, posFinal) <= 0.7f)
            {
                StartCoroutine(Ending());
            }
            else if(typePlay == TypePlay.Arcade)
            {
                currentTimeLife -= Time.deltaTime;
            }
        }
    }

    // ����������� ID ���������� �������
    int IDrandomevent(List<float> massEvents)
    {
        float sum = 0f;

        List<float> currentMassEvents = new List<float>();
        currentMassEvents = massEvents;

        for(int i = 0; i< currentMassEvents.Count; i++)
        {
            sum += currentMassEvents[i];
        }

        float rand = (int)Random.Range(0, sum);

        List<float> currentChance = new List<float>();
        for(int i = 0; i< currentMassEvents.Count; i++)
        {
            if(i == 0)
            {
                currentChance.Add(currentMassEvents[i]);
            }
            else
            {
                currentChance.Add(currentChance[i - 1] + currentMassEvents[i]);
            }
        }

        for(int i = 0; i<currentChance.Count; i++)
        {
            if(i == 0)
            {
                if(rand <= currentChance[i])
                {
                    return 0;
                }
            }
            else
            {
                if (rand > currentChance[i-1] && rand <= currentChance[i])
                {
                    return i;
                }
            }
        }

        return 0;
    }

    // ��������� �������
    IEnumerator ActivateEvent(int id)
    {
        if(id >= 4)
        {
            eventText.text = textEvents[textEvents.Count - 1];
        }
        else
        {
            eventText.text = textEvents[id];
        }
        eventText.gameObject.GetComponent<Animator>().Play("ShowEventTitle");

        yield return new WaitForSeconds(5f);
        if (ready)
        {
            darkAnim.gameObject.SetActive(true);
            darkAnim.Play("dark");
            yield return new WaitForSeconds(0.35f);

            switch (id)
            {
                case 0:
                    if (typePlay == TypePlay.Story)
                    {
                        AllIvents.pointerExitShow.Invoke();
                    }
                    else
                    {
                        AllIvents.generateMaze.Invoke();
                    }
                    break;

                case 1:
                    AllIvents.clearMaze.Invoke();
                    yield return new WaitForSeconds(4.5f);
                    AllIvents.generateMaze.Invoke();
                    break;

                case 2:
                    AllIvents.shiftPositionPlayer.Invoke(playerPos);
                    break;

                case 3:
                    AllIvents.pointerExitShow.Invoke();
                    break;

                case 4:
                    playerPos.position = startPlayerPos;
                    mazeManager.GrowingUp();
                    age++;
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(0.5f);
            darkAnim.gameObject.SetActive(false);

            countIvent++;
        }
    }

    IEnumerator Ending()
    {
        ready = false;
        // ��������� ��������

        if(typePlay == TypePlay.Arcade || age > 0)
        {
            if (Vector3.Distance(playerPos.position, posFinal) <= 0.7f)
            {
                eventText.text = "� ��� �� ������ ����� �����, �� ��� ������... ���� ������ ������ ���� � �� � ���� ����������...";
            }
            else
            {
                eventText.text = "��� ��, � ������ �� ������ �� � �� �������, ��� ���� ����� �������...";
            }

            director.Play();

            if (PlayerPrefs.HasKey("target"))
            {
                int currentCount = PlayerPrefs.GetInt("target");
                currentCount++;
                PlayerPrefs.SetInt("target", currentCount);
            }
            else
            {
                PlayerPrefs.SetInt("target", 1);
            }

            yield return new WaitForSeconds(15f);

            mainMenu.isReady = true;
        }
        else
        {
            ready = true;
            StartCoroutine(ActivateEvent(4));
        }
    }
}

public enum TypePlay { Arcade, Story }