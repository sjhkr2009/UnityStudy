namespace LineChart
{
    public readonly struct LineStyle
    {
        public string ColorHex { get; }
        public float Thickness { get; }

        public LineStyle(string colorHex, float thickness = 1f)
        {
            ColorHex = colorHex;
            Thickness = thickness;
        }

        public static LineStyle Default => new LineStyle("#FFFFFF", 1f);
    }
}