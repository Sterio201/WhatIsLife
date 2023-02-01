using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeScoreGenerate : MonoBehaviour
{
    [SerializeField] GameObject scorePrefab;
    Stack<GameObject> scorePool;

    [HideInInspector]
    public TypeMaze typeMaze;

    public void GenerateScore(MazeGeneratorCell[,] cells)
    {
        if(scorePool == null)
        {
            scorePool = new Stack<GameObject>();
        }

        int CountScore = 5;
        List<GameObject> temp = new List<GameObject>();

        for(int i = 0; i<CountScore; i++)
        {
            int randX = Random.Range(0, cells.GetLength(0) - 2);
            int randY = Random.Range(0, cells.GetLength(1) - 2);

            GameObject newScore;

            if (scorePool.Count != 0)
            {
                newScore = scorePool.Pop();
            }
            else
            {
                newScore = Instantiate(scorePrefab);
            }

            newScore.SetActive(true);
            newScore.GetComponent<Collider2D>().enabled = true;
            newScore.GetComponent<PlayerScoreChange>().score = cells[randX, randY].distanceFromStart;
            newScore.transform.position = new Vector3(randX + 0.5f, randY + 0.5f, newScore.transform.position.z);

            temp.Add(newScore);
        }

        if(scorePool.Count == 0)
        {
            for (int i = 0; i < temp.Count; i++)
            {
                scorePool.Push(temp[i]);
            }
        }
    }

    public void GenerateScore(MazeGeneratorCell[][] cells)
    {
        if (scorePool == null)
        {
            scorePool = new Stack<GameObject>();
        }

        int CountScore = 5;
        List<GameObject> temp = new List<GameObject>();

        for (int i = 0; i < CountScore; i++)
        {
            int randY = Random.Range(0, cells.Length);
            int randX = Random.Range(0, cells[randY].Length);

            GameObject newScore;

            if (scorePool.Count != 0)
            {
                newScore = scorePool.Pop();
            }
            else
            {
                newScore = Instantiate(scorePrefab);
            }

            newScore.SetActive(true);
            newScore.GetComponent<Collider2D>().enabled = true;
            newScore.GetComponent<PlayerScoreChange>().score = cells[randY][randX].distanceFromStart;
            newScore.transform.position = new Vector3((randX * 2f + 1f * randY) + 1f, (randY * 1.7f) + 1f);

            temp.Add(newScore);
        }

        if (scorePool.Count == 0)
        {
            for (int i = 0; i < temp.Count; i++)
            {
                scorePool.Push(temp[i]);
            }
        }
    }
}

public enum TypeMaze {Rectangular, Triangle }