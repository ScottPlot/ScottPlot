namespace ScottPlot.Renderable
{
    public class DefaultBottomAxis : Axis
    {
        public DefaultBottomAxis()
        {
            Edge = Edge.Bottom;
            Ticks.MajorGridEnable = true;
        }
    }

    public class DefaultTopAxis : Axis
    {
        public DefaultTopAxis()
        {
            Edge = Edge.Top;
            AxisIndex = 1;
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
            Ticks.MajorGridEnable = true;
        }
    }

    public class DefaultRightAxis : Axis
    {
        public DefaultRightAxis()
        {
            Edge = Edge.Right;
            AxisIndex = 1;

            Ticks.MajorTickEnable = false;
            Ticks.MinorTickEnable = false;
            Ticks.MajorLabelEnable = false;
            Title.IsVisible = false;
            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalRightAxis : Axis
    {
        public AdditionalRightAxis(int yAxisIndex, bool visible)
        {
            Edge = Edge.Right;
            AxisIndex = yAxisIndex;

            Title.IsVisible = visible;
            Ticks.MajorTickEnable = visible;
            Ticks.MajorLabelEnable = visible;
            Ticks.MinorTickEnable = visible;

            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalLeftAxis : Axis
    {
        public AdditionalLeftAxis(int yAxisIndex, bool visible)
        {
            Edge = Edge.Left;
            AxisIndex = yAxisIndex;
            PixelSize = 60;
            PixelOffset = 60;

            Title.IsVisible = visible;
            Ticks.MajorTickEnable = visible;
            Ticks.MajorLabelEnable = visible;
            Ticks.MinorTickEnable = visible;

            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalTopAxis : Axis
    {
        public AdditionalTopAxis(int xAxisIndex, bool visible)
        {
            Edge = Edge.Top;
            AxisIndex = xAxisIndex;
            PixelSize = 50;
            PixelOffset = 50;

            Title.IsVisible = visible;
            Ticks.MajorTickEnable = visible;
            Ticks.MajorLabelEnable = visible;
            Ticks.MinorTickEnable = visible;

            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalBottomAxis : Axis
    {
        public AdditionalBottomAxis(int xAxisIndex, bool visible)
        {
            Edge = Edge.Bottom;
            AxisIndex = xAxisIndex;
            PixelSize = 50;
            PixelOffset = 50;

            Title.IsVisible = visible;
            Ticks.MajorTickEnable = visible;
            Ticks.MajorLabelEnable = visible;
            Ticks.MinorTickEnable = visible;

            Ticks.MajorGridEnable = false;
        }
    }
}
