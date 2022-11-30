using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaze : MonoBehaviour
{
    public SpawnerRectangular spawnerRectangular;
    public SpawnerTriangle spawnerTriangle;

    private void Awake()
    {
        int rand = Random.Range(0, 2);

        if(rand == 0)
        {
            spawnerRectangular.enabled = true;
        }
        else if(rand == 1)
        {
            spawnerTriangle.enabled = true;
        }
    }
}