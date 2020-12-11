namespace ScottPlot.Renderable
{
    public class DefaultBottomAxis : Axis
    {
        public DefaultBottomAxis()
        {
            Edge = Edge.Bottom;
            AxTicks.MajorGridEnable = true;
        }
    }

    public class DefaultTopAxis : Axis
    {
        public DefaultTopAxis()
        {
            Edge = Edge.Top;
            AxisIndex = 1;
            AxTitle.IsVisible = true;
            AxTicks.MajorTickEnable = false;
            AxTicks.MinorTickEnable = false;
            AxTicks.MajorLabelEnable = false;
            AxTicks.MajorGridEnable = false;
        }
    }

    public class DefaultLeftAxis : Axis
    {
        public DefaultLeftAxis()
        {
            Edge = Edge.Left;
            AxTicks.MajorGridEnable = true;
        }
    }

    public class DefaultRightAxis : Axis
    {
        public DefaultRightAxis()
        {
            Edge = Edge.Right;
            AxisIndex = 1;
            AxTicks.MajorTickEnable = false;
            AxTicks.MinorTickEnable = false;
            AxTicks.MajorLabelEnable = false;
            AxTitle.IsVisible = false;
            AxTicks.MajorGridEnable = false;
        }
    }

    public class AdditionalRightAxis : Axis
    {
        public AdditionalRightAxis(int yAxisIndex, string title)
        {
            Edge = Edge.Right;
            AxisIndex = yAxisIndex;
            AxTitle.IsVisible = true;
            AxTicks.MajorTickEnable = true;
            AxTicks.MajorLabelEnable = true;
            AxTicks.MinorTickEnable = true;
            AxTicks.MajorGridEnable = false;
            AxTitle.Label = title;
        }
    }

    public class AdditionalLeftAxis : Axis
    {
        public AdditionalLeftAxis(int yAxisIndex, string title)
        {
            Edge = Edge.Left;
            AxisIndex = yAxisIndex;
            AxTitle.IsVisible = true;
            AxTicks.MajorTickEnable = true;
            AxTicks.MajorLabelEnable = true;
            AxTicks.MinorTickEnable = true;
            AxTicks.MajorGridEnable = false;
            AxTitle.Label = title;
        }
    }

    public class AdditionalTopAxis : Axis
    {
        public AdditionalTopAxis(int xAxisIndex, string title)
        {
            Edge = Edge.Top;
            AxisIndex = xAxisIndex;
            AxTitle.IsVisible = true;
            AxTicks.MajorTickEnable = true;
            AxTicks.MajorLabelEnable = true;
            AxTicks.MinorTickEnable = true;
            AxTicks.MajorGridEnable = false;
            AxTitle.Label = title;
        }
    }

    public class AdditionalBottomAxis : Axis
    {
        public AdditionalBottomAxis(int xAxisIndex, string title)
        {
            Edge = Edge.Bottom;
            AxisIndex = xAxisIndex;
            AxTitle.IsVisible = true;
            AxTicks.MajorTickEnable = true;
            AxTicks.MajorLabelEnable = true;
            AxTicks.MinorTickEnable = true;
            AxTicks.MajorGridEnable = false;
            AxTitle.Label = title;
        }
    }
}
