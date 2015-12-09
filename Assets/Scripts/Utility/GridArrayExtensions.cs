using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GridArrayExtensions
{
    public static IEnumerable<T> Flatten<T>(this T[,] grid)
    {
        int sizeX = grid.GetLength(0);
        int sizeY = grid.GetLength(1);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                yield return grid[x, y];
            }
        }
    }

    public static IEnumerable<Coordinate> GetFloodFillCoordinates<T>(this T[,] grid, int startX, int startY)
    {
        int sizeX = grid.GetLength(0);
        int sizeY = grid.GetLength(1);
        bool[,] gridFlags = new bool[sizeX, sizeY];
        T targetValue = grid[startX, startY];
        Queue<Coordinate> queue = new Queue<Coordinate>();

        queue.Enqueue(new Coordinate(startX, startY));
        gridFlags[startX, startY] = true;

        while (queue.Count > 0)
        {
            Coordinate coordinate = queue.Dequeue();

            yield return coordinate;

            foreach (Coordinate neighbor in grid.GetNeighborCoordinates(coordinate.X, coordinate.Y, false))
            {
                if (!gridFlags[neighbor.X, neighbor.Y] && grid[neighbor.X, neighbor.Y].Equals(targetValue))
                {
                    queue.Enqueue(neighbor);
                    gridFlags[neighbor.X, neighbor.Y] = true;
                }
            }
        }
    }

    public static IEnumerable<Coordinate> GetNeighborCoordinates<T>(this T[,] grid, int gridX, int gridY, bool allowDiagonals = true)
    {
        int sizeX = grid.GetLength(0);
        int sizeY = grid.GetLength(1);

        return GetNeighborCoordinates(gridX, gridY, sizeX, sizeY, allowDiagonals);
    }

    public static IEnumerable<Coordinate> GetNeighborCoordinates(int gridX, int gridY, int sizeX, int sizeY, bool allowDiagonals = true)
    {
        for (int neightborX = gridX - 1; neightborX <= gridX + 1; neightborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
            {
                bool coordinateInBounds = neightborX >= 0 && neightborX < sizeX && neighborY >= 0 && neighborY < sizeY;
                bool validDirection = allowDiagonals || neightborX == gridX || neighborY == gridY;
                bool currentNode = neightborX == gridX && neighborY == gridY;

                if (coordinateInBounds && validDirection && !currentNode)
                {
                    yield return new Coordinate(neightborX, neighborY);
                }
            }
        }
    }

    public static IEnumerable<Coordinate> GetNeighborCoordinatesInRadius<T>(this T[,] grid, int gridX, int gridY, float radius)
    {
        int sizeX = grid.GetLength(0);
        int sizeY = grid.GetLength(1);

        return GetNeighborCoordinatesInRadius(gridX, gridY, sizeX, sizeY, radius);
    }

    public static IEnumerable<Coordinate> GetNeighborCoordinatesInRadius(int gridX, int gridY, int sizeX, int sizeY, float radius)
    {
        for (int neightborX = gridX - (int)Mathf.Ceil(radius); neightborX <= gridX + (int)Mathf.Ceil(radius); neightborX++)
        {
            for (int neighborY = gridY - (int)Mathf.Ceil(radius); neighborY <= gridY + (int)Mathf.Ceil(radius); neighborY++)
            {
                bool coordinateInBounds = neightborX >= 0 && neightborX < sizeX && neighborY >= 0 && neighborY < sizeY;
                bool inRadius = Mathf.Pow(neightborX - gridX, 2) + Mathf.Pow(neighborY - gridY, 2) <= Mathf.Pow(radius, 2);
                bool currentNode = neightborX == gridX && neighborY == gridY;

                if (coordinateInBounds && inRadius && !currentNode)
                {
                    yield return new Coordinate(neightborX, neighborY);
                }
            }
        }
    }

    public static IEnumerable<T> GetNeighbors<T>(this T[,] grid, int gridX, int gridY, bool allowDiagonals = true)
    {
        return grid.GetNeighborCoordinates(gridX, gridY, allowDiagonals).Select(c => grid[c.X, c.Y]);
    }
}