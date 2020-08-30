using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    /// <summary>
    /// This class facilitates mouse interaction with Plot objects
    /// </summary>
    public class PlotControl
    {
        readonly Plot plt;
        readonly PlotInfo info;

        bool IsLeftDown;
        bool IsCenterDown;
        bool IsRightDown;

        float LeftDownX;
        float LeftDownY;
        float CenterDownX;
        float CenterDownY;
        float RightDownX;
        float RightDownY;
        AxisLimits MouseDownLimits;

        public PlotControl(Plot plt)
        {
            this.plt = plt;
            info = plt.Info;
        }

        public void MouseDownLeft(float xPixel, float yPixel)
        {
            IsLeftDown = true;
            LeftDownX = xPixel;
            LeftDownY = yPixel;
            MouseDownLimits = info.GetLimits();
        }

        public void MouseDownCenter(float xPixel, float yPixel)
        {
            IsCenterDown = true;
            CenterDownX = xPixel;
            CenterDownY = yPixel;
            MouseDownLimits = info.GetLimits();
        }

        public void MouseDownRight(float xPixel, float yPixel)
        {
            IsRightDown = true;
            RightDownX = xPixel;
            RightDownY = yPixel;
            MouseDownLimits = info.GetLimits();
        }

        public void MouseUpLeft(float xPixel, float yPixel)
        {
            IsLeftDown = false;
        }

        public void MouseUpCenter(float xPixel, float yPixel)
        {
            IsCenterDown = false;
            plt.AutoAxis();
        }

        public void MouseUpRight(float xPixel, float yPixel)
        {
            IsRightDown = false;
        }

        public void MouseMove(float xPixel, float yPixel)
        {
            if (IsLeftDown)
            {
                info.SetLimits(MouseDownLimits);
                info.MousePan(xPixel - LeftDownX, yPixel - LeftDownY);
            }
            else if (IsRightDown)
            {
                info.SetLimits(MouseDownLimits);
                info.MouseZoom(xPixel - RightDownX, yPixel - RightDownY);
            }
        }
    }
}
