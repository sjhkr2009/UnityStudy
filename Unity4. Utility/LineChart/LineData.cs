using System.Collections.Generic;

namespace LineChart
{
    public class LineData
    {
        public string Name { get; set; }
        public List<PointData> Points { get; } = new List<PointData>();
        public LineStyle Style { get; set; } = LineStyle.Default;

        public LineData(string name)
        {
            Name = name;
        }

        public LineData(string name, IEnumerable<PointData> points)
        {
            Name = name;
            Points.AddRange(points);
        }
    }
}