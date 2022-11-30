using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell
{
    public int X;
    public int Y;

    public bool WallLeft = true;
    public bool WallBottom = true;
    public bool WallRight = true;

    public bool visited = false;

    public int distanceFromStart;
}

public class MazeGenerater 
{
    public int Width = 12;
    public int Height = 8;

    public Vector2 posFinal;

    // Для построения прямоугольного лабиринта
    public MazeGeneratorCell[,] GeneratorMazeRect()
    {
        MazeGeneratorCell[,] maze = new MazeGeneratorCell[Width, Height];

        for(int x = 0; x<maze.GetLength(0); x++)
        {
            for(int y = 0; y<maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeGeneratorCell {X = x, Y = y};
            }
        }

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            maze[x, Height - 1].WallLeft = false;
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            maze[Width - 1, y].WallBottom = false;
        }

        RemoveWallsWithBacktracker(maze);
        Finish(maze);

        return maze;
    }

    // Для построения треугольного лабиринта
    public MazeGeneratorCell[][] GeneratorMazeTrian()
    {
        MazeGeneratorCell[][] maze = new MazeGeneratorCell[Height][];
        int startWidth = Height;

        for(int i = 0; i<maze.Length; i++)
        {
            maze[i] = new MazeGeneratorCell[startWidth];
            startWidth--;
        }

        for(int y = 0; y<maze.Length; y++)
        {
            for(int x = 0; x<maze[y].Length; x++)
            {
                maze[y][x] = new MazeGeneratorCell { X = x, Y = y };
            }
        }

        RemoveWallsWithBacktracker(maze);
        Finish(maze);

        return maze;
    }

    void RemoveWallsWithBacktracker(MazeGeneratorCell[,] cells)
    {
        MazeGeneratorCell current = cells[0, 0];
        current.visited = true;
        current.distanceFromStart = 0;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();

        do
        {
            List<MazeGeneratorCell> unvisitedCells = new List<MazeGeneratorCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !cells[x - 1, y].visited) unvisitedCells.Add(cells[x - 1, y]);
            if (y > 0 && !cells[x, y - 1].visited) unvisitedCells.Add(cells[x, y - 1]);
            if (x < Width - 2 && !cells[x + 1, y].visited) unvisitedCells.Add(cells[x + 1, y]);
            if (y < Height - 2 && !cells[x, y + 1].visited) unvisitedCells.Add(cells[x, y + 1]);

            if (unvisitedCells.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedCells[Random.Range(0, unvisitedCells.Count)];

                RemoveWall(current, chosen);

                chosen.visited = true;
                stack.Push(chosen);
                current = chosen;
                current.distanceFromStart = stack.Count;
            }
            else
            {
                current = stack.Pop();
            }

        } while (stack.Count > 0);
    }

    void RemoveWallsWithBacktracker(MazeGeneratorCell[][] cells)
    {
        MazeGeneratorCell current = cells[0][0];
        current.visited = true;
        current.distanceFromStart = 0;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();

        do
        {
            List<MazeGeneratorCell> unvisitedCells = new List<MazeGeneratorCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !cells[y][x - 1].visited) unvisitedCells.Add(cells[y][x - 1]);
            if (x < cells[y].Length - 1 && !cells[y][x + 1].visited) unvisitedCells.Add(cells[y][x + 1]);

            if (y > 0)
            {
                if (!cells[y - 1][x].visited)
                {
                    unvisitedCells.Add(cells[y - 1][x]);
                }

                if(x < cells[y].Length)
                {
                    if (!cells[y - 1][x + 1].visited)
                    {
                        unvisitedCells.Add(cells[y - 1][x + 1]);
                    }
                }
            }

            if (y < cells.Length - 1)
            {
                if(x < cells[y].Length-1)
                {
                    if (!cells[y + 1][x].visited)
                    {
                        unvisitedCells.Add(cells[y + 1][x]);
                    }
                }

                if(x > 0)
                {
                    if (!cells[y + 1][x - 1].visited)
                    {
                        unvisitedCells.Add(cells[y + 1][x - 1]);
                    }
                }
            }

            if (unvisitedCells.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedCells[Random.Range(0, unvisitedCells.Count)];

                RemoveWallTriangle(current, chosen);

                chosen.visited = true;
                stack.Push(chosen);
                current = chosen;
                current.distanceFromStart = stack.Count;
            }
            else
            {
                current = stack.Pop();
            }

        } while (stack.Count > 0);
    }

    void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y)
            {
                a.WallBottom = false;
            }
            else
            {
                b.WallBottom = false;
            }
        }
        else
        {
            if(a.X > b.X)
            {
                a.WallLeft = false;
            }
            else
            {
                b.WallLeft = false;
            }
        }
    }

    void RemoveWallTriangle(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if(a.X == b.X)
        {
            if(a.Y > b.Y)
            {
                a.WallBottom = false;
                b.WallRight = false;
            }
            else
            {
                b.WallBottom = false;
                a.WallRight = false;
            }
        }
        else
        {
            if(a.Y == b.Y)
            {
                if(a.X > b.X)
                {
                    a.WallLeft = false;
                    b.WallRight = false;
                }
                else
                {
                    a.WallRight = false;
                    b.WallLeft = false;
                }
            }
            else
            {
                if(a.Y > b.Y)
                {
                    a.WallBottom = false;
                    b.WallLeft = false;
                }
                else
                {
                    b.WallBottom = false;
                    a.WallLeft = false;
                }
            }
        }
    }

    void Finish(MazeGeneratorCell[,] cells)
    {
        MazeGeneratorCell furthest = cells[0, 0];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            if (cells[x, Height - 2].distanceFromStart > furthest.distanceFromStart) furthest = cells[x, Height - 2];
            if (cells[x, 0].distanceFromStart > furthest.distanceFromStart) furthest = cells[x, 0];
        }

        for (int y = 0; y < cells.GetLength(1); y++)
        {
            if (cells[Width - 2, y].distanceFromStart > furthest.distanceFromStart) furthest = cells[Width - 2, y];
            if (cells[0, y].distanceFromStart > furthest.distanceFromStart) furthest = cells[0, y];
        }

        if (furthest.X == 0) furthest.WallLeft = false;
        else if (furthest.Y == 0) furthest.WallBottom = false;
        else if (furthest.X == Width - 2) cells[furthest.X + 1, furthest.Y].WallLeft = false;
        else if (furthest.Y == Height - 2) cells[furthest.X, furthest.Y + 1].WallBottom = false;

        posFinal = new Vector2(furthest.X + 0.5f, furthest.Y + 0.5f);
    }

    void Finish(MazeGeneratorCell[][] cells)
    {
        MazeGeneratorCell furthest = cells[0][0];

        for(int y = 0; y<cells.Length; y++)
        {
            if (cells[y][0].distanceFromStart > furthest.distanceFromStart) furthest = cells[y][0];
            if (cells[y][cells[y].Length - 1].distanceFromStart > furthest.distanceFromStart) furthest = cells[y][cells[y].Length - 1];
        }
        for(int x = 0; x<cells[0].Length; x++)
        {
            if (cells[0][x].distanceFromStart > furthest.distanceFromStart) furthest = cells[0][x];
        }

        if (furthest.X == 0) furthest.WallLeft = false;
        else if (furthest.Y == 0) furthest.WallBottom = false;
        else if (furthest.X + furthest.Y + 1 == cells.Length) furthest.WallRight = false;
        else if (furthest.Y == cells.Length - 1) furthest.WallLeft = false;
    }
}