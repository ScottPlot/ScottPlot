using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Linq;
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

        int[] ActiveIndexesX;
        int[] ActiveIndexesY;
        AxisLimits1D[] MouseDownLimitsX;
        AxisLimits1D[] MouseDownLimitsY;

        public PlotControl(Plot plt)
        {
            this.plt = plt;
            info = plt.Info;
            SetActiveAxis(0, 0);
        }

        public void SetActiveAxis(int xIndex, int yIndex)
        {
            ActiveIndexesX = new int[1] { xIndex };
            ActiveIndexesY = new int[1] { yIndex };
        }

        public void SetActiveAxes(int[] xIndexes, int[] yIndexes)
        {
            ActiveIndexesX = xIndexes.ToArray();
            ActiveIndexesY = yIndexes.ToArray();
        }

        private void RememberMouseDownLimits()
        {
            MouseDownLimitsX = new AxisLimits1D[ActiveIndexesX.Length];
            for (int i = 0; i < ActiveIndexesX.Length; i++)
                MouseDownLimitsX[i] = info.XAxes[ActiveIndexesX[i]].GetLimits();

            MouseDownLimitsY = new AxisLimits1D[ActiveIndexesY.Length];
            for (int i = 0; i < ActiveIndexesY.Length; i++)
                MouseDownLimitsY[i] = info.YAxes[ActiveIndexesY[i]].GetLimits();
        }

        public void MouseDownLeft(float xPixel, float yPixel)
        {
            IsLeftDown = true;
            LeftDownX = xPixel;
            LeftDownY = yPixel;
            RememberMouseDownLimits();
        }

        public void MouseDownCenter(float xPixel, float yPixel)
        {
            IsCenterDown = true;
            CenterDownX = xPixel;
            CenterDownY = yPixel;
            RememberMouseDownLimits();
        }

        public void MouseDownRight(float xPixel, float yPixel)
        {
            IsRightDown = true;
            RightDownX = xPixel;
            RightDownY = yPixel;
            RememberMouseDownLimits();
        }

        public void MouseUpLeft(float xPixel, float yPixel)
        {
            IsLeftDown = false;
        }

        public void MouseUpCenter(float xPixel, float yPixel)
        {
            IsCenterDown = false;
            foreach (int xIndex in ActiveIndexesX)
                plt.AutoX(xIndex);
            foreach (int yIndex in ActiveIndexesY)
                plt.AutoY(yIndex);
        }

        public void MouseUpRight(float xPixel, float yPixel)
        {
            IsRightDown = false;
        }

        public void MouseMove(float xPixel, float yPixel)
        {
            if (IsLeftDown == false && IsRightDown == false)
                return;

            for (int i = 0; i < ActiveIndexesX.Length; i++)
            {
                info.XAxes[i].SetLimits(MouseDownLimitsX[i]);
                if (IsLeftDown)
                    info.XAxes[i].PanPx(xPixel - LeftDownX);
                else if (IsRightDown)
                    info.XAxes[i].ZoomPx(xPixel - RightDownX);
            }

            for (int i = 0; i < ActiveIndexesY.Length; i++)
            {
                info.YAxes[i].SetLimits(MouseDownLimitsY[i]);
                if (IsLeftDown)
                    info.YAxes[i].PanPx(yPixel - LeftDownY);
                else if (IsRightDown)
                    info.YAxes[i].ZoomPx(yPixel - RightDownY);
            }
        }
    }
}
