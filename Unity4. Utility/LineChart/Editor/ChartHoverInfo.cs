using System.Collections.Generic;
using LineChart;
using UnityEngine;

public struct ChartHoverInfo
{
    public struct Point
    {
        public string LineName;
        public PointData HitPoint;

        public Point(string lineName, PointData hitPoint)
        {
            LineName = lineName;
            HitPoint = hitPoint;
        }
    }
    public bool HasHit => HitPoints is { Count: > 0 };
    public List<Point> HitPoints { get; }
    public Vector2 ScreenPosition { get; }

    public ChartHoverInfo(Vector2 screenPosition, string lineName, PointData hitPoint)
    {
        HitPoints = new List<Point>() { new Point(lineName, hitPoint) };
        ScreenPosition = screenPosition;
    }

    public void AddPoint(string lineName, PointData hitPoint)
    {
        HitPoints.Add(new Point(lineName, hitPoint));
    }

    public static ChartHoverInfo None => new ChartHoverInfo();
}