using System.Collections.Generic;

namespace LineChart
{
    public class ChartData
    {
        public string Title { get; set; }

        public AxisData XAxis { get; set; } = new AxisData("X");
        public AxisData YAxis { get; set; } = new AxisData("Y");

        public List<LineData> LineDatas { get; } = new List<LineData>();

        public ChartData(string title = "")
        {
            Title = title;
        }

        public ChartBounds CalculateBounds()
        {
            if (LineDatas.Count == 0)
                return new ChartBounds();

            bool hasAnyPoint = false;
            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (var s in LineDatas)
            {
                foreach (var p in s.Points)
                {
                    hasAnyPoint = true;

                    if (p.X < minX) minX = p.X;
                    if (p.X > maxX) maxX = p.X;
                    if (p.Y < minY) minY = p.Y;
                    if (p.Y > maxY) maxY = p.Y;
                }
            }

            if (!hasAnyPoint)
                return new ChartBounds();

            return new ChartBounds(minX, maxX, minY, maxY);
        }
    }

    public readonly struct ChartBounds
    {
        public bool HasValue { get; }
        public double MinX { get; }
        public double MaxX { get; }
        public double MinY { get; }
        public double MaxY { get; }

        public ChartBounds(double minX, double maxX, double minY, double maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            HasValue = true;
        }
    }
}