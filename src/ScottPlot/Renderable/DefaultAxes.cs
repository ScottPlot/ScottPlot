namespace ScottPlot.Renderable
{
    public class DefaultBottomAxis : Axis
    {
        public DefaultBottomAxis()
        {
            Edge = Edge.Bottom;
            PixelSize = 40;
            Ticks.MajorGridEnable = true;
        }
    }

    public class DefaultTopAxis : Axis
    {
        public DefaultTopAxis()
        {
            Edge = Edge.Top;
            AxisIndex = 1;
            PixelSize = 40;
            Title.Font.Bold = true;
            Title.IsVisible = true;

            Ticks.MajorTickEnable = false;
            Ticks.MinorTickEnable = false;
            Ticks.MajorLabelEnable = false;
            Ticks.MajorGridEnable = false;
        }
    }

    public class DefaultLeftAxis : Axis
    {
        public DefaultLeftAxis()
        {
            Edge = Edge.Left;
            PixelSize = 60;
            Ticks.MajorGridEnable = true;
        }
    }

    public class DefaultRightAxis : Axis
    {
        public DefaultRightAxis()
        {
            Edge = Edge.Right;
            AxisIndex = 1;
            PixelSize = 60;

            Ticks.MajorTickEnable = false;
            Ticks.MinorTickEnable = false;
            Ticks.MajorLabelEnable = false;
            Title.IsVisible = false;
            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalRightAxis : Axis
    {
        public AdditionalRightAxis(int xAxisIndex, bool visible)
        {
            Edge = Edge.Right;
            AxisIndex = xAxisIndex;
            PixelSize = 60;

            Title.IsVisible = visible;
            Ticks.MajorTickEnable = visible;
            Ticks.MajorLabelEnable = visible;
            Ticks.MinorTickEnable = visible;

            Ticks.MajorGridEnable = false;
        }
    }
}
