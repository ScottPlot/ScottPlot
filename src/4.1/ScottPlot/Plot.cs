using System;

namespace ScottPlot
{
    public class Plot
    {
        public float Width {get { return info.Width; } }
        public float Height { get { return info.Height; } }

        private readonly PlotInfo info = new PlotInfo();

        public Plot(float width = 600, float height = 400)
        {
            if ((width < 1) || (height < 1))
                throw new ArgumentException("Width and height must be greater than 1");

            info.Resize(width, height, width - 20, height - 10, 10, 10);
        }
    }
}
