using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public SpawnerRectangular spawnerRectangular;
    public SpawnerTriangle spawnerTriangle;

    MazeScoreGenerate mazeScore;
    EventsLabirintian events;

    [SerializeField] Transform child;
    [SerializeField] Transform adult;

    private void Awake()
    {
        mazeScore = GetComponent<MazeScoreGenerate>();
        events = GetComponent<EventsLabirintian>();

        if(events.typePlay == TypePlay.Arcade)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                spawnerRectangular.scoreGenerate = mazeScore;
                spawnerRectangular.enabled = true;
            }
            else if (rand == 1)
            {
                spawnerTriangle.scoreGenerate = mazeScore;
                spawnerTriangle.enabled = true;
            }
        }
        else if(events.typePlay == TypePlay.Story)
        {
            spawnerTriangle.scoreGenerate = mazeScore;
            spawnerTriangle.labirintPos = adult;
            spawnerTriangle.enabled = true;

            spawnerRectangular.scoreGenerate = mazeScore;
            spawnerRectangular.labirintPos = child;
            spawnerRectangular.enabled = true;

            spawnerTriangle.enabled = false;
            adult.gameObject.SetActive(false);

            spawnerRectangular.FirstPointer();
        }

        /*mazeScore.typeMaze = TypeMaze.Triangle;
        spawnerTriangle.scoreGenerate = mazeScore;
        spawnerTriangle.enabled = true;*/
    }

    public void GrowingUp()
    {
        spawnerRectangular.enabled = false;
        child.gameObject.SetActive(false);

        spawnerTriangle.enabled = true;
        adult.gameObject.SetActive(true);

        spawnerTriangle.ShiftPointer();
    }
}