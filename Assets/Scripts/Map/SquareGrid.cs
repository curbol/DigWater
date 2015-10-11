using UnityEngine;

public class SquareGrid
{
    public MeshSquare[,] Squares { get; set; }

    public SquareGrid(int [,] map, float squareSize)
    {
        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);

        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        MeshControlNode[,] controllNodes = new MeshControlNode[nodeCountX, nodeCountY];

        for (int x = 0; x < nodeCountX; x++)
        {
            for (int y = 0; y < nodeCountY; y++)
            {
                Vector2 position = new Vector2(-mapWidth / 2f + x * squareSize + squareSize / 2f, -mapHeight / 2f + y * squareSize + squareSize / 2f);
                controllNodes[x, y] = new MeshControlNode(position, map[x, y] == 1, squareSize);
            }
        }

        Squares = new MeshSquare[nodeCountX - 1, nodeCountY - 1];
        for (int x = 0; x < nodeCountX - 1; x++)
        {
            for (int y = 0; y < nodeCountY - 1; y++)
            {
                Squares[x, y] = new MeshSquare(controllNodes[x, y + 1], controllNodes[x + 1, y + 1], controllNodes[x + 1, y], controllNodes[x, y]);
            }
        }
    }
}
