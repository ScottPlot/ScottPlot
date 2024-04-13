namespace ScottPlot.Renderable
{
    public class DefaultBottomAxis : Axis
    {
        public DefaultBottomAxis() : base(0, Edge.Bottom)
        {
            Grid(true);
        }
    }

    public class DefaultTopAxis : Axis
    {
        public DefaultTopAxis() : base(1, Edge.Top)
        {
            Grid(false);
            Ticks(false);
        }
    }

    public class DefaultLeftAxis : Axis
    {
        public DefaultLeftAxis() : base(0, Edge.Left)
        {
            Grid(true);
        }
    }

    public class DefaultRightAxis : Axis
    {
        public DefaultRightAxis() : base(1, Edge.Right)
        {
            Grid(false);
            Ticks(false);
        }
    }

    public class AdditionalRightAxis : Axis
    {
        public AdditionalRightAxis(int yAxisIndex, string title) : base(yAxisIndex, Edge.Right)
        {
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }

    public class AdditionalLeftAxis : Axis
    {
        public AdditionalLeftAxis(int yAxisIndex, string title) : base(yAxisIndex, Edge.Left)
        {
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }

    public class AdditionalTopAxis : Axis
    {
        public AdditionalTopAxis(int xAxisIndex, string title) : base(xAxisIndex, Edge.Top)
        {
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }

    public class AdditionalBottomAxis : Axis
    {
        public AdditionalBottomAxis(int xAxisIndex, string title) : base(xAxisIndex, Edge.Bottom)
        {
            Grid(false);
            Ticks(true);
            Label(title);
        }
    }
}
