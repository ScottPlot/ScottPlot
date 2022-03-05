namespace ScottPlot.Renderable
{
    public class DefaultBottomAxis : Axis
    {
        public DefaultBottomAxis()
        {
            Edge = Edge.Bottom;
            Grid(true);
        }
    }

    public class DefaultTopAxis : Axis
    {
        public DefaultTopAxis()
        {
            Edge = Edge.Top;
            AxisIndex = 1;
            Grid(false);
            Ticks(false);
        }
    }

    public class DefaultLeftAxis : Axis
    {
        public DefaultLeftAxis()
        {
            Edge = Edge.Left;
            Grid(true);
        }
    }

    public class DefaultRightAxis : Axis
    {
        public DefaultRightAxis()
        {
            Edge = Edge.Right;
            AxisIndex = 1;
            Grid(false);
            Ticks(false);
        }
    }

    public class AdditionalRightAxis : Axis
    {
        public AdditionalRightAxis(int yAxisIndex, string title)
        {
            Edge = Edge.Right;
            AxisIndex = yAxisIndex;
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }

    public class AdditionalLeftAxis : Axis
    {
        public AdditionalLeftAxis(int yAxisIndex, string title)
        {
            Edge = Edge.Left;
            AxisIndex = yAxisIndex;
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }

    public class AdditionalTopAxis : Axis
    {
        public AdditionalTopAxis(int xAxisIndex, string title)
        {
            Edge = Edge.Top;
            AxisIndex = xAxisIndex;
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }

    public class AdditionalBottomAxis : Axis
    {
        public AdditionalBottomAxis(int xAxisIndex, string title)
        {
            Edge = Edge.Bottom;
            AxisIndex = xAxisIndex;
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }
}
