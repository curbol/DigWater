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

    public static IEnumerable<T> GetNeighbors<T>(this T[,] grid, int gridX, int gridY, bool allowDiagonals = true)
    {
        return grid.GetNeighborCoordinates(gridX, gridY, allowDiagonals).Select(c => grid[c.X, c.Y]);
    }

    public static IEnumerable<Coordinate> GetNeighborCoordinates<T>(this T[,] grid, int gridX, int gridY, bool allowDiagonals = true)
    {
        for (int neightborX = gridX - 1; neightborX <= gridX + 1; neightborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
            {
                bool coordinateInRange = neightborX >= 0 && neightborX < grid.GetLength(0) && neighborY >= 0 && neighborY < grid.GetLength(1);
                bool validDirection = allowDiagonals || neightborX == gridX || neighborY == gridY;
                bool currentNode = neightborX == gridX && neighborY == gridY;

                if (coordinateInRange && validDirection && !currentNode)
                {
                    yield return new Coordinate(neightborX, neighborY);
                }
            }
        }
    }

    public static IEnumerable<Coordinate> GetNeighborCoordinatesInRadius<T>(this T[,] grid, int gridX, int gridY, float radius, bool allowDiagonals = true)
    {
        Queue<Coordinate> coordinatesToCheck = new Queue<Coordinate>();
        HashSet<Coordinate> checkedCoordinates = new HashSet<Coordinate>();
        Coordinate startCoordinate = new Coordinate(gridX, gridY);

        coordinatesToCheck.Enqueue(startCoordinate);
        while (coordinatesToCheck.Count() > 0)
        {
            Coordinate coordinateToCheck = coordinatesToCheck.Dequeue();
            checkedCoordinates.Add(coordinateToCheck);

            if (startCoordinate.EuclidianDistance(coordinateToCheck) <= radius)
            {
                if (coordinateToCheck != startCoordinate)
                {
                    yield return coordinateToCheck;
                }

                foreach (Coordinate neighborCoordinate in grid.GetNeighborCoordinates(coordinateToCheck.X, coordinateToCheck.Y))
                {
                    if (!checkedCoordinates.Contains(neighborCoordinate) && !coordinatesToCheck.Contains(neighborCoordinate))
                    {
                        coordinatesToCheck.Enqueue(neighborCoordinate);
                    }
                }
            }
        }
    }

    public static float ManhattanDistance(this Coordinate c1, Coordinate c2)
    {
        return Mathf.Abs(c1.X - c2.X) + Mathf.Abs(c1.Y - c2.Y);
    }

    public static float EuclidianDistance(this Coordinate c1, Coordinate c2)
    {
        return Mathf.Sqrt(Mathf.Pow(c1.X - c2.X, 2) + Mathf.Pow(c1.Y - c2.Y, 2));
    }
}