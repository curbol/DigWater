using System.Collections.Generic;

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
        for (int neightborX = gridX - 1; neightborX <= gridX + 1; neightborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
            {
                bool coordinateInRange = neightborX >= 0 && neightborX < grid.GetLength(0) && neighborY >= 0 && neighborY < grid.GetLength(1);
                bool validDirection = allowDiagonals || neightborX == gridX || neighborY == gridY;
                bool currentNode = neightborX == gridX && neighborY == gridY;

                if (coordinateInRange && validDirection && !currentNode)
                {
                    yield return grid[neightborX, neighborY];
                }
            }
        }
    }
}