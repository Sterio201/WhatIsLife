using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRectangular : MonoBehaviour
{
    [SerializeField] Cell cellPrefab;
    public Vector3 CellSize = new Vector3(1, 1, 0);
    public Vector2 posFinish;

    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] Transform labirintPos;
    [SerializeField] Transform PlayerTransform;

    [HideInInspector]
    public Vector2 posFinal;

    [HideInInspector]
    public Cell[,] cells;

    [HideInInspector]
    public MazeScoreGenerate scoreGenerate;

    private void Start()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell c = Instantiate(cellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);
                c.transform.parent = labirintPos;
                cells[x, y] = c;
            }
        }

        GenerateCells();

        AllIvents.generateMaze.AddListener(GenerateCells);
        AllIvents.clearMaze.AddListener(ClearingLabirintian);
        AllIvents.shiftPositionPlayer.AddListener(RandomPosPlayer);
        AllIvents.shiftColorMaze.AddListener(ShiftColorMaze);
    }

    /*private void Awake()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell c = Instantiate(cellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);
                c.transform.parent = labirintPos;
                cells[x, y] = c;
            }
        }

        GenerateCells();

        TestAllIvents.generateMaze.AddListener(GenerateCells);
        TestAllIvents.clearMaze.AddListener(ClearingLabirintian);
        TestAllIvents.shiftPositionPlayer.AddListener(RandomPosPlayer);
        TestAllIvents.shiftColorMaze.AddListener(ShiftColorMaze);
    }*/

    public void GenerateCells()
    {
        MazeGenerater generaterCell = new MazeGenerater();

        generaterCell.Width = width;
        generaterCell.Height = height;

        MazeGeneratorCell[,] maze = generaterCell.GeneratorMazeRect();

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                cells[x, y].wallLeft.SetActive(maze[x, y].WallLeft);
                cells[x, y].wallBottom.SetActive(maze[x, y].WallBottom);
            }
        }

        posFinal = generaterCell.posFinal;
        EventsLabirintian.posFinal = posFinal;

        scoreGenerate.GenerateScore(maze);
    }

    public void ClearingLabirintian()
    {
        for(int i = 0; i<width - 1; i++)
        {
            if(i > 0)
            {
                cells[i, 0].wallLeft.SetActive(false);
            }
        }
        for (int j = 0; j < width - 1; j++)
        {
            if(j > 0)
            {
                cells[0, j].wallBottom.SetActive(false);
            }
        }

        for(int i = 1; i<width-1; i++)
        {
            for(int j = 1; j < height - 1; j++)
            {
                cells[i, j].wallBottom.SetActive(false);
                cells[i, j].wallLeft.SetActive(false);
            }
        }
    }

    public Vector2 RandomPos()
    {
        float randX = Random.Range(0, (float)width - 1);
        float randY = Random.Range(0, (float)height - 1);

        return new Vector2(randX, randY);
    }

    public void RandomPosPlayer(Transform position)
    {
        float randX = Random.Range(0, (float)width - 1);
        float randY = Random.Range(0, (float)height - 1);

        position.position = new Vector3(randX, randY, position.position.z);
    }

    public void ShiftColorMaze(Color shiftColor)
    {
        GameObject colorWallLeft;
        GameObject colorWallDown;

        for(int i = 0; i<cells.GetLength(0); i++)
        {
            for(int j = 0; j<cells.GetLength(1); j++)
            {
                colorWallLeft = cells[i, j].gameObject.GetComponent<Transform>().GetChild(0).gameObject;
                colorWallDown = cells[i, j].gameObject.GetComponent<Transform>().GetChild(1).gameObject;

                colorWallLeft.GetComponent<SpriteRenderer>().color = 
                    Color.Lerp(colorWallLeft.GetComponent<SpriteRenderer>().color, shiftColor, Mathf.Abs(Mathf.Sin(Time.deltaTime)));

                colorWallDown.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(colorWallDown.GetComponent<SpriteRenderer>().color, shiftColor, Mathf.Abs(Mathf.Sin(Time.deltaTime)));
            }
        }
    }
}