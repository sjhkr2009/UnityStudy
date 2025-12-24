namespace LineChart
{
    public sealed class AxisData
    {
        public string Label { get; set; }
        public string Unit { get; set; }

        public AxisData(string label, string unit = "")
        {
            Label = label;
            Unit = unit;
        }
    }
}