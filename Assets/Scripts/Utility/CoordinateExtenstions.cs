using System;
using System.Collections.Generic;
using UnityEngine;

public static class CoordinateExtenstions
{
    public static float ManhattanDistance(this Coordinate c1, Coordinate c2)
    {
        return Mathf.Abs(c1.X - c2.X) + Mathf.Abs(c1.Y - c2.Y);
    }

    public static float EuclidianDistance(this Coordinate c1, Coordinate c2)
    {
        return Mathf.Sqrt(Mathf.Pow(c1.X - c2.X, 2) + Mathf.Pow(c1.Y - c2.Y, 2));
    }

    public static IEnumerable<Coordinate> GetCoordinateLine(this Coordinate fromCoordinate, Coordinate toCoordinate)
    {
        int x = fromCoordinate.X;
        int y = fromCoordinate.Y;
        int dx = toCoordinate.X - fromCoordinate.X;
        int dy = toCoordinate.Y - fromCoordinate.Y;
        int longest = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
        int shortest = Mathf.Min(Mathf.Abs(dx), Mathf.Abs(dy));
        bool inverted = Mathf.Abs(dx) != longest;
        int step = !inverted ? Math.Sign(dx) : Math.Sign(dy);
        int gradientStep = !inverted ? Math.Sign(dy) : Math.Sign(dx);
        int gradientAccumulation = longest / 2;

        for (int i = 0; i < longest; i++)
        {
            yield return new Coordinate(x, y);

            if (!inverted)
            {
                x += step;
            }
            else
            {
                y += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                gradientAccumulation -= longest;

                if (!inverted)
                {
                    y += gradientStep;
                }
                else
                {
                    x += gradientStep;
                }
            }
        }
    }
}