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
        public AdditionalRightAxis(int yAxisIndex)
        {
            Edge = Edge.Right;
            AxisIndex = yAxisIndex;
            Title.IsVisible = true;
            Ticks.MajorTickEnable = true;
            Ticks.MajorLabelEnable = true;
            Ticks.MinorTickEnable = true;
            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalLeftAxis : Axis
    {
        public AdditionalLeftAxis(int yAxisIndex)
        {
            Edge = Edge.Left;
            AxisIndex = yAxisIndex;
            Title.IsVisible = true;
            Ticks.MajorTickEnable = true;
            Ticks.MajorLabelEnable = true;
            Ticks.MinorTickEnable = true;
            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalTopAxis : Axis
    {
        public AdditionalTopAxis(int xAxisIndex)
        {
            Edge = Edge.Top;
            AxisIndex = xAxisIndex;
            Title.IsVisible = true;
            Ticks.MajorTickEnable = true;
            Ticks.MajorLabelEnable = true;
            Ticks.MinorTickEnable = true;
            Ticks.MajorGridEnable = false;
        }
    }

    public class AdditionalBottomAxis : Axis
    {
        public AdditionalBottomAxis(int xAxisIndex)
        {
            Edge = Edge.Bottom;
            AxisIndex = xAxisIndex;
            Title.IsVisible = true;
            Ticks.MajorTickEnable = true;
            Ticks.MajorLabelEnable = true;
            Ticks.MinorTickEnable = true;
            Ticks.MajorGridEnable = false;
        }
    }
}
