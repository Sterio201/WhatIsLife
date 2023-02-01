using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaze : MonoBehaviour
{
    public SpawnerRectangular spawnerRectangular;
    public SpawnerTriangle spawnerTriangle;

    MazeScoreGenerate mazeScore;

    private void Awake()
    {
        int rand = Random.Range(0, 2);
        mazeScore = GetComponent<MazeScoreGenerate>();

        /*if(rand == 0)
        {
            mazeScore.typeMaze = TypeMaze.Rectangular;
            spawnerRectangular.scoreGenerate = mazeScore;
            spawnerRectangular.enabled = true;
        }
        else if(rand == 1)
        {
            mazeScore.typeMaze = TypeMaze.Triangle;
            spawnerTriangle.scoreGenerate = mazeScore;
            spawnerTriangle.enabled = true;
        }*/

        mazeScore.typeMaze = TypeMaze.Triangle;
        spawnerTriangle.scoreGenerate = mazeScore;
        spawnerTriangle.enabled = true;
    }
}