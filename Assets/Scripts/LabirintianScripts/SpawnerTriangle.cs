using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTriangle : MonoBehaviour
{
    [SerializeField] Cell cellPrefab;
    public Vector3 CellSize = new Vector3(1, 1, 0);
    public Vector2 posFinish;

    [SerializeField] int height;

    public Transform labirintPos;

    [HideInInspector]
    public Vector2 posFinal;

    [HideInInspector]
    public Cell[][] cells;

    [HideInInspector]
    public MazeScoreGenerate scoreGenerate;

    private void Start()
    {
        cells = new Cell[height][];
        int startWidth = height;

        float temp = 0f;

        for(int y = 0; y<cells.Length; y++)
        {
            cells[y] = new Cell[startWidth];
            startWidth--;

            for(int x = 0; x<cells[y].Length; x++)
            {
                Cell c = Instantiate(cellPrefab, new Vector3((x * CellSize.x) + temp, (y * (CellSize.y - 0.3f)), y * CellSize.z), Quaternion.identity);
                c.transform.parent = labirintPos;
                cells[y][x] = c;
            }

            temp += 1f;
        }

        GenerateCells();
    }

    private void OnEnable()
    {
        AllIvents.generateMaze.AddListener(GenerateCells);
        AllIvents.clearMaze.AddListener(ClearningMaze);
        AllIvents.shiftPositionPlayer.AddListener(RandomPosPlayer);
        AllIvents.shiftColorMaze.AddListener(ShiftColorMaze);
    }

    private void OnDisable()
    {
        AllIvents.generateMaze.RemoveListener(GenerateCells);
        AllIvents.clearMaze.RemoveListener(ClearningMaze);
        AllIvents.shiftPositionPlayer.RemoveListener(RandomPosPlayer);
        AllIvents.shiftColorMaze.RemoveListener(ShiftColorMaze);
    }

    public void ShiftPointer()
    {
        AllIvents.pointerExit.transform.position = posFinal;
    }

    public void GenerateCells()
    {
        MazeGenerater generaterCell = new MazeGenerater();

        generaterCell.Width = height;
        generaterCell.Height = height;

        MazeGeneratorCell[][] maze = generaterCell.GeneratorMazeTrian();

        for(int y = 0; y<maze.Length; y++)
        {
            for(int x = 0; x<maze[y].Length; x++)
            {
                cells[y][x].wallBottom.SetActive(maze[y][x].WallBottom);
                cells[y][x].wallLeft.SetActive(maze[y][x].WallLeft);
                cells[y][x].wallRight.SetActive(maze[y][x].WallRight);
            }
        }

        posFinal = generaterCell.posFinal;
        EventsLabirintian.posFinal = posFinal;

        scoreGenerate.GenerateScore(maze);
    }

    public void ClearningMaze()
    {
        for(int y = 0; y<cells.Length; y++)
        {
            for(int x = 0; x<cells[y].Length; x++)
            {
                if(y < cells.Length - 1)
                {
                    if(x != 0)
                    {
                        cells[y][x].wallLeft.SetActive(false);
                    }
                    
                    if(x < cells[y].Length - 1)
                    {
                        cells[y][x].wallRight.SetActive(false);
                    }
                }

                if (y > 0)
                {
                    cells[y][x].wallBottom.SetActive(false);
                }
            }
        }
    }

    public void RandomPosPlayer(Transform position)
    {
        float randY = Random.Range(0, cells.Length - 1);
        float randX = Random.Range(0, cells[(int)randY].Length - 1);

        randX += randY;

        position.position = new Vector3(randX, randY, position.position.z);
    }

    public void ShiftColorMaze(Color shiftColor)
    {
        GameObject colorWallLeft;
        GameObject colorWallDown;
        GameObject colorWallRight;

        for (int y = 0; y < cells.Length; y++)
        {
            for (int x = 0; x < cells[y].Length; x++)
            {
                colorWallLeft = cells[y][x].gameObject.GetComponent<Transform>().GetChild(1).gameObject;
                colorWallDown = cells[y][x].gameObject.GetComponent<Transform>().GetChild(0).gameObject;
                colorWallRight = cells[y][x].gameObject.GetComponent<Transform>().GetChild(2).gameObject;

                colorWallLeft.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(colorWallLeft.GetComponent<SpriteRenderer>().color, shiftColor, Mathf.Abs(Mathf.Sin(Time.deltaTime)));

                colorWallDown.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(colorWallDown.GetComponent<SpriteRenderer>().color, shiftColor, Mathf.Abs(Mathf.Sin(Time.deltaTime)));

                colorWallRight.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(colorWallRight.GetComponent<SpriteRenderer>().color, shiftColor, Mathf.Abs(Mathf.Sin(Time.deltaTime)));
            }
        }
    }
}