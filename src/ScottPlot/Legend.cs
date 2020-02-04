using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public enum legendLocation
    {
        none,
        upperLeft,
        upperRight,
        upperCenter,
        middleLeft,
        middleRight,
        lowerLeft,
        lowerRight,
        lowerCenter,
    }

    public enum shadowDirection
    {
        none,
        upperLeft,
        upperRight,
        lowerLeft,
        lowerRight,
    }

    public class LegendTools
    {

        public static Rectangle GetLegendFrame(Settings settings, Graphics gfxLegend)
        {
            // note which plottables are to be included in the legend
            List<int> plottableIndexesNeedingLegend = new List<int>();
            for (int i = 0; i < settings.plottables.Count(); i++)
                if (settings.plottables[i].label != null && settings.plottables[i].visible)
                    plottableIndexesNeedingLegend.Add(i);
            plottableIndexesNeedingLegend.Reverse();

            // figure out where on the graph things should be
            int padding = 3;
            int stubWidth = 40 * (int)settings.legend.font.Size / 12;
            SizeF maxLabelSize = MaxLegendLabelSize(settings);
            float frameWidth = padding * 2 + maxLabelSize.Width + padding + stubWidth;
            float frameHeight = padding * 2 + maxLabelSize.Height * plottableIndexesNeedingLegend.Count();
            Size frameSize = new Size((int)frameWidth, (int)frameHeight);
            Point[] frameAndTextLocations = GetLocations(settings, padding * 2, frameSize, maxLabelSize.Width);
            Point frameLocation = frameAndTextLocations[0];
            Point shadowLocation = frameAndTextLocations[2];
            Point fullFrameLocation = new Point(Math.Min(frameLocation.X, shadowLocation.X), Math.Min(frameLocation.Y, shadowLocation.Y));
            return new Rectangle(fullFrameLocation,
                                new Size(frameSize.Width + Math.Abs(frameLocation.X - shadowLocation.X) + 1,
                                frameSize.Height + Math.Abs(frameLocation.Y - shadowLocation.Y) + 1));
        }

        public static void DrawLegend(Settings settings)
        {
            // note which plottables are to be included in the legend
            List<int> plottableIndexesNeedingLegend = new List<int>();
            for (int i = 0; i < settings.plottables.Count(); i++)
                if (settings.plottables[i].label != null && settings.plottables[i].visible)
                    plottableIndexesNeedingLegend.Add(i);
            plottableIndexesNeedingLegend.Reverse();

            // figure out where on the graph things should be
            int padding = 3;
            int stubWidth = 40 * (int)settings.legend.font.Size / 12;
            SizeF maxLabelSize = MaxLegendLabelSize(settings);
            float frameWidth = padding * 2 + maxLabelSize.Width + padding + stubWidth;
            float frameHeight = padding * 2 + maxLabelSize.Height * plottableIndexesNeedingLegend.Count();
            Size frameSize = new Size((int)frameWidth, (int)frameHeight);
            Point[] frameAndTextLocations = GetLocations(settings, padding * 2, frameSize, maxLabelSize.Width);
            Point frameLocation = frameAndTextLocations[0];
            Point textLocation = frameAndTextLocations[1];
            Point shadowLocation = frameAndTextLocations[2];

            // move legend to 0, 0 position
            Point frameZeroOffset = new Point(frameLocation.X > shadowLocation.X ? frameLocation.X - shadowLocation.X : 0,
                                              frameLocation.Y > shadowLocation.Y ? frameLocation.Y - shadowLocation.Y : 0);

            Point shadowZeroOffset = new Point(shadowLocation.X > frameLocation.X ? shadowLocation.X - frameLocation.X : 0,
                                               shadowLocation.Y > frameLocation.Y ? shadowLocation.Y - frameLocation.Y : 0);
            textLocation.X -= frameLocation.X - frameZeroOffset.X;
            textLocation.Y -= frameLocation.Y - frameZeroOffset.Y;
            Rectangle frameRect = new Rectangle(frameZeroOffset, frameSize);
            Rectangle shadowRect = new Rectangle(shadowZeroOffset, frameSize);

            settings.gfxLegend.Clear(settings.legend.colorBackground);
            // draw the legend background and shadow
            if (settings.legend.shadow != shadowDirection.none)
                settings.gfxLegend.FillRectangle(new SolidBrush(settings.legend.colorShadow), shadowRect);
            settings.gfxLegend.FillRectangle(new SolidBrush(settings.legend.colorBackground), frameRect);
            settings.gfxLegend.DrawRectangle(new Pen(settings.legend.colorFrame), frameRect);

            // draw the lines, markers, and text for each legend item
            foreach (int i in plottableIndexesNeedingLegend)
            {
                textLocation.Y -= (int)(maxLabelSize.Height);
                DrawLegendItemString(settings.plottables[i], settings, textLocation, padding, stubWidth, maxLabelSize.Height);
                DrawLegendItemLine(settings.plottables[i], settings, textLocation, padding, stubWidth, maxLabelSize.Height);
                DrawLegendItemMarker(settings.plottables[i], settings, textLocation, padding, stubWidth, maxLabelSize.Height);
            }
        }

        public static SizeF MaxLegendLabelSize(Settings settings)
        {
            SizeF maxLabelSize = new SizeF();

            foreach (Plottable plottable in settings.plottables)
            {
                if (plottable.label != null)
                {
                    SizeF labelSize = settings.gfxLegend.MeasureString(plottable.label, settings.legend.font);
                    if (labelSize.Width > maxLabelSize.Width)
                        maxLabelSize.Width = labelSize.Width;
                    if (labelSize.Height > maxLabelSize.Height)
                        maxLabelSize.Height = labelSize.Height;
                }
            }

            return maxLabelSize;
        }

        private static Point[] GetLocations(ScottPlot.Settings settings, int padding, Size frameSize, float legendFontMaxWidth)
        {
            Point frameLocation = new Point();
            Point textLocation = new Point();
            Point shadowLocation = new Point();

            // calculate locations even if it's not going to be displayed
            legendLocation loc = settings.legend.location;
            if (loc == legendLocation.none)
                loc = legendLocation.lowerRight;

            int frameWidth = frameSize.Width;
            int frameHeight = frameSize.Height;
            switch (loc)
            {
                case (legendLocation.lowerRight):
                    frameLocation.X = (int)(settings.dataSize.Width - frameWidth - padding);
                    frameLocation.Y = (int)(settings.dataSize.Height - frameHeight - padding);
                    textLocation.X = (int)(settings.dataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = settings.dataSize.Height - padding * 2;
                    break;
                case (legendLocation.upperLeft):
                    frameLocation.X = (int)(padding);
                    frameLocation.Y = (int)(padding);
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = (int)(frameHeight);
                    break;
                case (legendLocation.lowerLeft):
                    frameLocation.X = (int)(padding);
                    frameLocation.Y = (int)(settings.dataSize.Height - frameHeight - padding);
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = settings.dataSize.Height - padding * 2;
                    break;
                case (legendLocation.upperRight):
                    frameLocation.X = (int)(settings.dataSize.Width - frameWidth - padding);
                    frameLocation.Y = (int)(padding);
                    textLocation.X = (int)(settings.dataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = (int)(frameHeight);
                    break;
                case (legendLocation.upperCenter):
                    frameLocation.X = (int)((settings.dataSize.Width) / 2 - frameWidth / 2);
                    frameLocation.Y = (int)(padding);
                    textLocation.X = (int)(frameLocation.X + frameWidth - legendFontMaxWidth);
                    textLocation.Y = (int)(frameHeight);
                    break;
                case (legendLocation.lowerCenter):
                    frameLocation.X = (int)((settings.dataSize.Width) / 2 - frameWidth / 2);
                    frameLocation.Y = (int)(settings.dataSize.Height - frameHeight - padding);
                    textLocation.X = (int)(frameLocation.X + frameWidth - legendFontMaxWidth);
                    textLocation.Y = settings.dataSize.Height - padding * 2;
                    break;
                case (legendLocation.middleLeft):
                    frameLocation.X = (int)(padding);
                    frameLocation.Y = (int)(settings.dataSize.Height / 2 - frameHeight / 2);
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = (int)(frameLocation.Y + frameHeight - padding);
                    break;
                case (legendLocation.middleRight):
                    frameLocation.X = (int)(settings.dataSize.Width - frameWidth - padding);
                    frameLocation.Y = (int)(settings.dataSize.Height / 2 - frameHeight / 2);
                    textLocation.X = (int)(settings.dataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = (int)(frameLocation.Y + frameHeight - padding);
                    break;
                default:
                    throw new NotImplementedException($"legend location {settings.legend.location} is not supported");
            }

            switch (settings.legend.shadow)
            {
                case (shadowDirection.lowerRight):
                    shadowLocation.X = frameLocation.X + 2;
                    shadowLocation.Y = frameLocation.Y + 2;
                    break;
                case (shadowDirection.lowerLeft):
                    shadowLocation.X = frameLocation.X - 2;
                    shadowLocation.Y = frameLocation.Y + 2;
                    break;
                case (shadowDirection.upperRight):
                    shadowLocation.X = frameLocation.X + 2;
                    shadowLocation.Y = frameLocation.Y - 2;
                    break;
                case (shadowDirection.upperLeft):
                    shadowLocation.X = frameLocation.X - 2;
                    shadowLocation.Y = frameLocation.Y - 2;
                    break;
                default:
                    settings.legend.shadow = shadowDirection.none;
                    break;
            }

            textLocation.Y += padding;
            return new Point[] { frameLocation, textLocation, shadowLocation };
        }

        private static void DrawLegendItemString(Plottable plottable, Settings settings, Point textLocation, int padding, int stubWidth, float legendFontLineHeight)
        {
            Brush brushText = new SolidBrush(settings.legend.colorText);
            settings.gfxLegend.DrawString(plottable.label, settings.legend.font, brushText, textLocation);
        }

        private static void DrawLegendItemLine(Plottable plottable, Settings settings, Point textLocation, int padding, int stubWidth, float legendFontLineHeight)
        {
            Pen pen = new Pen(plottable.color, 1);

            if (plottable is PlottableVSpan || plottable is PlottableHSpan)
                pen.Width = 10;

            if (settings.legend.fixedLineWidth == false)
            {
                if (plottable is PlottableScatter)
                    pen.Width = (float)((PlottableScatter)plottable).lineWidth;
                if (plottable is PlottableSignal)
                    pen.Width = (float)((PlottableSignal)plottable).lineWidth;
            }

            // dont draw line if it's not on the plottable
            if ((plottable is PlottableScatter) && (((PlottableScatter)plottable).lineWidth) == 0)
                return;
            if ((plottable is PlottableSignal) && (((PlottableSignal)plottable).lineWidth) == 0)
                return;

            switch (plottable.lineStyle)
            {
                case LineStyle.Solid:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
                case LineStyle.Dash:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    break;
                case LineStyle.DashDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    break;
                case LineStyle.DashDotDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    break;
                case LineStyle.Dot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    break;
            }

            settings.gfxLegend.DrawLine(pen,
                textLocation.X - padding, textLocation.Y + legendFontLineHeight / 2,
                textLocation.X - padding - stubWidth, textLocation.Y + legendFontLineHeight / 2);
        }

        private static void DrawLegendItemMarker(Plottable plottable, Settings settings, Point textLocation, int padding, int stubWidth, float legendFontLineHeight)
        {
            Brush brushMarker = new SolidBrush(plottable.color);
            Pen penMarker = new Pen(plottable.color, 1);

            // dont draw marker if it's not on the plottable
            if (plottable.markerShape == MarkerShape.none)
                return;
            if ((plottable is PlottableScatter) && (((PlottableScatter)plottable).markerSize) == 0)
                return;
            if ((plottable is PlottableSignal) && (((PlottableSignal)plottable).markerSize) == 0)
                return;

            PointF corner1 = new PointF(textLocation.X - stubWidth + settings.legend.font.Size / 4, textLocation.Y + settings.legend.font.Size / 4 * padding);
            PointF center = new PointF
            {
                X = corner1.X + settings.legend.font.Size / 4,
                Y = corner1.Y + settings.legend.font.Size / 4
            };

            SizeF bounds = new SizeF(settings.legend.font.Size / 2, settings.legend.font.Size / 2);
            RectangleF rect = new RectangleF(corner1, bounds);

            switch (plottable.markerShape)
            {
                case MarkerShape.none:
                    //Nothing to do because the Drawline needs to be there for all cases, and it's already there
                    break;
                case MarkerShape.asterisk:
                    Font drawFontAsterisk = new Font("CourierNew", settings.legend.font.Size);
                    Point markerPositionAsterisk = new Point(textLocation.X - stubWidth, (int)(textLocation.Y + legendFontLineHeight / 4));
                    settings.gfxLegend.DrawString("*", drawFontAsterisk, brushMarker, markerPositionAsterisk);
                    break;
                case MarkerShape.cross:
                    Font drawFontCross = new Font("CourierNew", settings.legend.font.Size);
                    Point markerPositionCross = new Point(textLocation.X - stubWidth, (int)(textLocation.Y + legendFontLineHeight / 8));
                    settings.gfxLegend.DrawString("+", drawFontCross, brushMarker, markerPositionCross);
                    break;
                case MarkerShape.eks:
                    Font drawFontEks = new Font("CourierNew", settings.legend.font.Size);
                    Point markerPositionEks = new Point(textLocation.X - stubWidth, (int)(textLocation.Y));
                    settings.gfxLegend.DrawString("x", drawFontEks, brushMarker, markerPositionEks);
                    break;
                case MarkerShape.filledCircle:
                    settings.gfxLegend.FillEllipse(brushMarker, rect);
                    break;
                case MarkerShape.filledDiamond:
                    // Create points that define polygon.
                    PointF point1 = new PointF(center.X, center.Y + settings.legend.font.Size / 4);
                    PointF point2 = new PointF(center.X - settings.legend.font.Size / 4, center.Y);
                    PointF point3 = new PointF(center.X, center.Y - settings.legend.font.Size / 4);
                    PointF point4 = new PointF(center.X + settings.legend.font.Size / 4, center.Y);

                    PointF[] curvePoints = { point1, point2, point3, point4 };

                    //Draw polygon to screen
                    settings.gfxLegend.FillPolygon(brushMarker, curvePoints);
                    break;
                case MarkerShape.filledSquare:
                    settings.gfxLegend.FillRectangle(brushMarker, rect);
                    break;
                case MarkerShape.hashTag:
                    Font drawFontHashtag = new Font("CourierNew", settings.legend.font.Size);
                    Point markerPositionHashTag = new Point(textLocation.X - stubWidth, (int)(textLocation.Y + legendFontLineHeight / 8));
                    settings.gfxLegend.DrawString("#", drawFontHashtag, brushMarker, markerPositionHashTag);
                    break;
                case MarkerShape.openCircle:
                    settings.gfxLegend.DrawEllipse(penMarker, rect);
                    break;
                case MarkerShape.openDiamond:
                    // Create points that define polygon.
                    PointF point5 = new PointF(center.X, center.Y + settings.legend.font.Size / 4);
                    PointF point6 = new PointF(center.X - settings.legend.font.Size / 4, center.Y);
                    PointF point7 = new PointF(center.X, center.Y - settings.legend.font.Size / 4);
                    PointF point8 = new PointF(center.X + settings.legend.font.Size / 4, center.Y);

                    PointF[] curvePoints2 = { point5, point6, point7, point8 };

                    //Draw polygon to screen
                    settings.gfxLegend.DrawPolygon(penMarker, curvePoints2);
                    break;
                case MarkerShape.openSquare:
                    settings.gfxLegend.DrawRectangle(penMarker, corner1.X, corner1.Y, settings.legend.font.Size / 2, settings.legend.font.Size / 2);
                    break;
                case MarkerShape.triDown:
                    // Create points that define polygon.
                    PointF point14 = new PointF(center.X, center.Y + settings.legend.font.Size / 2);
                    PointF point15 = new PointF(center.X, center.Y);
                    PointF point16 = new PointF(center.X - settings.legend.font.Size / 2 * (float)0.866, center.Y - settings.legend.font.Size / 2 * (float)0.5);
                    PointF point17 = new PointF(center.X, center.Y);
                    PointF point18 = new PointF(center.X + settings.legend.font.Size / 2 * (float)0.866, center.Y - settings.legend.font.Size / 2 * (float)0.5);

                    PointF[] curvePoints4 = { point17, point14, point15, point16, point17, point18 };

                    //Draw polygon to screen
                    settings.gfxLegend.DrawPolygon(penMarker, curvePoints4);

                    break;

                case MarkerShape.triUp:
                    // Create points that define polygon.
                    PointF point9 = new PointF(center.X, center.Y - settings.legend.font.Size / 2);
                    PointF point10 = new PointF(center.X, center.Y);
                    PointF point11 = new PointF(center.X - settings.legend.font.Size / 2 * (float)0.866, center.Y + settings.legend.font.Size / 2 * (float)0.5);
                    PointF point12 = new PointF(center.X, center.Y);
                    PointF point13 = new PointF(center.X + settings.legend.font.Size / 2 * (float)0.866, center.Y + settings.legend.font.Size / 2 * (float)0.5);

                    PointF[] curvePoints3 = { point12, point9, point10, point11, point12, point13 };
                    //Draw polygon to screen
                    settings.gfxLegend.DrawPolygon(penMarker, curvePoints3);
                    break;

                case MarkerShape.verticalBar:
                    Font drawFontVertical = new Font("CourierNew", settings.legend.font.Size);
                    Point markerPositionVertical = new Point(textLocation.X - stubWidth, (int)(textLocation.Y));
                    settings.gfxLegend.DrawString("|", drawFontVertical, brushMarker, markerPositionVertical);
                    break;
            }
        }

    }
}
