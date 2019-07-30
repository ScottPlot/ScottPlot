using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class Tick
    {

        //public string text;
        public double value;
        public Settings settings;
        public Pen tickPen;
        public Brush tickBrush;
        public Pen gridPen;
        public int exponent;

        public Tick(Settings settings, double value, int exponent = 0)
        {
            this.value = value;
            this.settings = settings;
            this.exponent = exponent;
            tickPen = new Pen(settings.tickColor);
            tickBrush = new SolidBrush(settings.tickColor);
            gridPen = new Pen(settings.gridColor);
        }

        public string text
        {
            get
            {
                double dividedValue = value / Math.Pow(10, exponent);
                return string.Format("{0}", Math.Round(dividedValue, 2));
            }
        }

        public string textMultiplier
        {
            get
            {
                if (exponent != 0)
                    return string.Format("10e{0}", exponent);
                else
                    return "";
            }
        }

        public void RenderGridHorizontalLine(Settings settings)
        {
            double unitsFromAxisEdge = value - settings.axis[2];
            int yPx = settings.dataSize.Height - (int)(unitsFromAxisEdge * settings.yAxisScale);
            settings.gfxData.DrawLine(gridPen, 0, yPx, settings.dataSize.Width, yPx);
        }
        public void RenderGridVerticalLine(Settings settings)
        {
            double unitsFromAxisEdge = value - settings.axis[0];
            int xPx = (int)(unitsFromAxisEdge * settings.xAxisScale);
            settings.gfxData.DrawLine(gridPen, xPx, 0, xPx, settings.dataSize.Height);
        }

        public void RenderTickOnLeft(Settings settings)
        {
            double unitsFromAxisEdge = value - settings.axis[2];
            int xPx = settings.dataOrigin.X;
            int yPx = (int)(unitsFromAxisEdge * settings.yAxisScale);
            yPx = settings.figureSize.Height - yPx - settings.axisLabelPadding[2];

            settings.gfxFigure.DrawLine(tickPen, xPx, yPx, xPx - settings.tickSize, yPx);
            settings.gfxFigure.DrawString(text, settings.tickFont, tickBrush, xPx - settings.tickSize, yPx, settings.sfEast);
        }

        public void RenderTickOnBottom(Settings settings)
        {
            double unitsFromAxisEdge = value - settings.axis[0];
            int xPx = (int)(unitsFromAxisEdge * settings.xAxisScale) + settings.axisLabelPadding[0];
            int yPx = settings.figureSize.Height - settings.axisLabelPadding[2];

            settings.gfxFigure.DrawLine(tickPen, xPx, yPx, xPx, yPx + settings.tickSize);
            settings.gfxFigure.DrawString(text, settings.tickFont, tickBrush, xPx, yPx + settings.tickSize, settings.sfNorth);
        }

        public Size TextSize()
        {
            return System.Windows.Forms.TextRenderer.MeasureText(text, settings.tickFont);
        }

    }

    public class Ticks
    {
        Settings settings;

        public Ticks(Settings settings)
        {
            this.settings = settings;
            settings.ticksX = GetTicks(settings, settings.axis[0], settings.axis[1], settings.xAxisScale, 40, settings.dataSize.Width, settings.gridSpacingX);
            settings.ticksY = GetTicks(settings, settings.axis[2], settings.axis[3], settings.yAxisScale, 20, settings.dataSize.Height, settings.gridSpacingY);
        }

        public void RenderTicks()
        {
            if (settings.displayTicksX)
                foreach (Tick tick in settings.ticksX)
                    tick.RenderTickOnBottom(settings);

            if (settings.displayTicksY)
                foreach (Tick tick in settings.ticksY)
                    tick.RenderTickOnLeft(settings);

            RenderTickMultipliers();
        }

        private void RenderTickMultipliers()
        {
            if (settings.ticksX.Count > 0)
            {
                string multiplierLabelX = settings.ticksX[0].textMultiplier;
                SizeF multiplierLabelXsize = settings.gfxFigure.MeasureString(multiplierLabelX, settings.tickFont);
                settings.gfxFigure.DrawString(multiplierLabelX, settings.tickFont, settings.ticksX[0].tickBrush,
                    settings.dataOrigin.X + settings.dataSize.Width,
                    settings.dataOrigin.Y + settings.dataSize.Height + multiplierLabelXsize.Height,
                    settings.sfNorthEast);
            }

            if (settings.ticksY.Count > 0)
            {
                string multiplierLabelY = settings.ticksY[0].textMultiplier;
                SizeF multiplierLabelYsize = settings.gfxFigure.MeasureString(multiplierLabelY, settings.tickFont);
                settings.gfxFigure.DrawString(multiplierLabelY, settings.tickFont, settings.ticksY[0].tickBrush,
                    settings.dataOrigin.X,
                    settings.dataOrigin.Y,
                    settings.sfSouthWest);
            }
        }

        public void RenderGrid()
        {
            if (settings.displayGrid)
            {
                foreach (Tick tick in settings.ticksX)
                    tick.RenderGridVerticalLine(settings);
                foreach (Tick tick in settings.ticksY)
                    tick.RenderGridHorizontalLine(settings);
            }
        }

        public Size GetMaxTickSize(List<Tick> ticks)
        {
            string longestTickLabel = "-88888"; // manually limit this with exponential notation
            SizeF longestTickSize = settings.gfxFigure.MeasureString(longestTickLabel, settings.tickFont);
            return new Size((int)longestTickSize.Width, (int)longestTickSize.Height);

            /*
            double largestWidth = 0;
            double largestHeight = 0;
            foreach (Tick tick in ticks)
            {
                SizeF tickSize = settings.gfxFigure.MeasureString(tick.text, settings.tickFont);
                if (tickSize.Width > largestWidth)
                    largestWidth = tickSize.Width;
                if (tickSize.Height > largestHeight)
                    largestHeight = tickSize.Height;
            }
            return new Size((int)largestWidth, (int)largestHeight);
            */
        }

        private List<Tick> GetTicks(Settings settings, double axisLow, double axisHigh, double axisScale, int tickSpacingPx, int axisSizePx, double useThisSpacing = 0)
        {
            double axisSpan = axisHigh - axisLow;
            if (tickSpacingPx == 40) // probably a horizontal axis
                if (axisSpan < Math.Pow(10, -3) / 2 || axisSpan > Math.Pow(10, 4))
                    tickSpacingPx = 60;
            double tickCountTarget = (double)axisSizePx / tickSpacingPx / 2;

            double tickSpacing = 0;
            if (useThisSpacing > 0)
            {
                tickSpacing = useThisSpacing;
            }
            else
            {
                // determine an ideal tick spacing (multiple of 2, 5, or 10)
                for (int tickSpacingPower = 1000; tickSpacingPower > -1000; tickSpacingPower--)
                {
                    tickSpacing = Math.Pow(10, tickSpacingPower);
                    if (tickSpacing > axisSpan)
                        continue;
                    double tickCount = axisSpan / tickSpacing;
                    if (tickCount >= tickCountTarget)
                    {
                        if (tickCount >= tickCountTarget * 5)
                            tickSpacing *= 5;
                        else if (tickCount >= tickCountTarget * 2)
                            tickSpacing *= 2;
                        break;
                    }
                }
            }

            // determine the exponential notation for the axis span
            int exponent;
            double largestValue = Math.Max(Math.Abs(axisLow), Math.Abs(axisHigh));
            for (exponent = -1000; exponent < 1000; exponent++)
            {
                if (Math.Pow(10, exponent) > largestValue)
                    break;
            }
            exponent -= 2;
            if (Math.Abs(exponent) < 4)
                exponent = 0;


            // create the ticks
            List<Tick> ticks = new List<Tick>();
            if (tickSpacing > 0)
            {
                int yTickCount = (int)(axisSpan / tickSpacing) + 2;
                double tickOffset = axisLow % tickSpacing;
                for (int i = 0; i < yTickCount; i++)
                {
                    double value = tickSpacing * i + axisLow - tickOffset;
                    if (value >= axisLow && value <= axisHigh)
                        ticks.Add(new Tick(settings, value, exponent));
                }
            }
            return ticks;
        }
    }
}
