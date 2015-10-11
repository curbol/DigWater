using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private List<Vector3> vertices;
    private List<int> triangles;

    public SquareGrid SquareGrid { get; set; }

    public void GenerateMesh(int[,] map, float squareSize)
    {
        SquareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < SquareGrid.Squares.GetLength(0); x++)
        {
            for (int y = 0; y < SquareGrid.Squares.GetLength(1); y++)
            {
                TriangulateSquare(SquareGrid.Squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void TriangulateSquare(MeshSquare square)
    {
        switch (square.Configuration)
        {
            // 0 points
            case 0:
                break;

            // 1 point
            case 1:
                MeshFromPoints(square.CenterLeft, square.CenterBottom, square.BottomLeft);
                break;
            case 2:
                MeshFromPoints(square.BottomRight, square.CenterBottom, square.CenterRight);
                break;
            case 4:
                MeshFromPoints(square.TopRight, square.CenterRight, square.CenterTop);
                break;
            case 8:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterLeft);
                break;

            // 2 points
            case 3:
                MeshFromPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 5:
                MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
                break;
            case 6:
                MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
                break;
            case 9:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
                break;
            case 10:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;
            case 12:
                MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
                break;

            // 3 points
            case 7:
                MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 11:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
                break;
            case 13:
                MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
                break;
            case 14:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                break;

            // 4 points
            case 15:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                break;
        }
    }

    private void MeshFromPoints(params MeshNode[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
        {
            triangles.AddRange(GetTriangle(points[0], points[1], points[2]));
        }
        if (points.Length >= 4)
        {
            triangles.AddRange(GetTriangle(points[0], points[2], points[3]));
        }
        if (points.Length >= 5)
        {
            triangles.AddRange(GetTriangle(points[0], points[3], points[4]));
        }
        if (points.Length >= 6)
        {
            triangles.AddRange(GetTriangle(points[0], points[4], points[5]));
        }
    }

    private void AssignVertices(MeshNode[] points)
    {
        for(int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex == null)
            {
                points[i].VertexIndex = vertices.Count;
                vertices.Add(points[i].Position);
            }
        }
    }

    private static int[] GetTriangle(MeshNode a, MeshNode b, MeshNode c)
    {
        int[] triangle = null;

        if (a.VertexIndex != null && b.VertexIndex != null && c.VertexIndex != null)
        {
            triangle = new int[] { (int)a.VertexIndex, (int)b.VertexIndex, (int)c.VertexIndex };
        }

        return triangle;
    }

    private void OnDrawGizmos()
    {
        //if (SquareGrid != null)
        //{
        //    for (int x = 0; x < SquareGrid.Squares.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < SquareGrid.Squares.GetLength(1); y++)
        //        {
        //            Gizmos.color = SquareGrid.Squares[x, y].TopLeft.Active ? Color.red : Color.magenta;
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].TopLeft.Position, Vector2.one * .4f);

        //            Gizmos.color = SquareGrid.Squares[x, y].TopRight.Active ? Color.blue : Color.cyan;
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].TopRight.Position, Vector2.one * .4f);

        //            Gizmos.color = SquareGrid.Squares[x, y].BottomLeft.Active ? Color.green : Color.yellow;
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].BottomLeft.Position, Vector2.one * .4f);

        //            Gizmos.color = SquareGrid.Squares[x, y].BottomRight.Active ? Color.black : Color.white;
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].BottomRight.Position, Vector2.one * .4f);

        //            Gizmos.color = Color.gray;
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].CenterTop.Position, Vector2.one * .15f);
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].CenterLeft.Position, Vector2.one * .15f);
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].CenterRight.Position, Vector2.one * .15f);
        //            Gizmos.DrawCube(SquareGrid.Squares[x, y].CenterBottom.Position, Vector2.one * .15f);
        //        }
        //    }
        //}
    }
}