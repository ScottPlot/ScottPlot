using ScottPlot.Renderer;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    /// <summary>
    /// This class facilitates mouse interaction with Plot objects.
    /// 
    /// User controls simply pass mouse state to/from this module, 
    /// keeping mouse interactivity logic out of user control code.
    /// </summary>
    public class MouseTracker
    {
        readonly Plot plt;
        readonly Dimensions info;

        Point LeftDown;
        Point CenterDown;
        Point RightDown;

        bool IsLeftDown { get => LeftDown != null; }
        bool IsCenterDown { get => CenterDown != null; }
        bool IsRightDown { get => RightDown != null; }

        public MouseTracker(Plot plt)
        {
            this.plt = plt;
            info = plt.Info;
            SetActiveAxis(0, 0);
        }

        public void SetActiveAxis(int xIndex, int yIndex)
        {
            SetActiveAxes(new int[] { xIndex }, new int[] { yIndex });
        }

        public void SetActiveAxes(int[] xIndexes, int[] yIndexes)
        {
            for (int i = 0; i < info.XAxes.Count; i++)
                info.XAxes[i].IsLocked = xIndexes.Contains(i) ? false : true;

            for (int i = 0; i < info.YAxes.Count; i++)
                info.YAxes[i].IsLocked = yIndexes.Contains(i) ? false : true;
        }

        private void RememberMouseDownLimits()
        {
            foreach (var xAxis in info.XAxes)
                if (!xAxis.IsLocked)
                    xAxis.RememberLimits();

            foreach (var yAxis in info.YAxes)
                if (!yAxis.IsLocked)
                    yAxis.RememberLimits();
        }

        public void MouseDownLeft(float xPixel, float yPixel)
        {
            LeftDown = new Point(xPixel, yPixel);
            RememberMouseDownLimits();
        }

        public void MouseDownCenter(float xPixel, float yPixel)
        {
            CenterDown = new Point(xPixel, yPixel);
            RememberMouseDownLimits();
        }

        public void MouseDownRight(float xPixel, float yPixel)
        {
            RightDown = new Point(xPixel, yPixel);
            RememberMouseDownLimits();
        }

        public void MouseUpLeft(float xPixel, float yPixel)
        {
            LeftDown = null;
        }

        public void MouseUpCenter(float xPixel, float yPixel)
        {
            CenterDown = null;

            for (int i = 0; i < info.XAxes.Count; i++)
                if (!info.XAxes[i].IsLocked)
                    plt.AutoX(i);

            for (int i = 0; i < info.YAxes.Count; i++)
                if (!info.YAxes[i].IsLocked)
                    plt.AutoY(i);
        }

        public void MouseUpRight(float xPixel, float yPixel)
        {
            RightDown = null;
        }

        public void MouseMove(float xPixel, float yPixel)
        {
            if (IsLeftDown == false && IsRightDown == false)
                return;

            foreach (var xAxis in info.XAxes)
            {
                if (!xAxis.IsLocked)
                {
                    xAxis.RecallLimits();
                    if (IsLeftDown)
                        xAxis.PanPx(xPixel - LeftDown.X);
                    else if (IsRightDown)
                        xAxis.ZoomPx(xPixel - RightDown.X);
                }
            }

            foreach (var yAxis in info.YAxes)
            {
                if (!yAxis.IsLocked)
                {
                    yAxis.RecallLimits();
                    if (IsLeftDown)
                        yAxis.PanPx(yPixel - LeftDown.Y);
                    else if (IsRightDown)
                        yAxis.ZoomPx(yPixel - RightDown.Y);
                }
            }
        }
    }
}
