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
        private const int padding = 3;
        private const int ShadowWidth = 2;
        private const int ShadowHeight = 2;

        private static IEnumerable<Plottable> GetPlottableInLegend(Settings settings)
        {
            return settings.plottables.Where(p => p.visible && p.label != null);
        }

        public static Rectangle GetLegendFrame(Settings settings)
        {
            Size FrameSize = GetFrameSize(settings);

            Point FullFrameLocation = GetFullFrameLocation(settings, FrameSize);
            Size FullFrameSize = GetFullFrameSize(FrameSize, settings.legend.shadow);
            return new Rectangle(FullFrameLocation, Size.Add(FullFrameSize, new Size(1, 1)));
        }

        public static void DrawLegend(Settings settings)
        {
            SizeF maxLabelSize = MaxLegendLabelSize(settings);
            int stubWidth = 40 * (int)settings.legend.font.Size / 12;

            Size FrameSize = GetFrameSize(settings);
            Point FrameOffset = GetFrameOffset(settings.legend.shadow);
            Rectangle FrameRect = new Rectangle(FrameOffset, FrameSize);

            settings.gfxLegend.Clear(settings.legend.colorBackground);

            if (settings.legend.shadow != shadowDirection.none)
            {
                Point shadowOffset = GetShadowOffset(settings.legend.shadow);
                Rectangle shadowRect = new Rectangle(shadowOffset, FrameSize);
                settings.gfxLegend.FillRectangle(new SolidBrush(settings.legend.colorShadow), shadowRect);
            }

            settings.gfxLegend.FillRectangle(new SolidBrush(settings.legend.colorBackground), FrameRect);
            settings.gfxLegend.DrawRectangle(new Pen(settings.legend.colorFrame), FrameRect);

            foreach (var (p, index) in GetPlottableInLegend(settings).Select((x, i) => (x, i)))
            {
                Point legendItemLocation = new Point(FrameOffset.X,
                    padding + index * (int)(maxLabelSize.Height) + FrameOffset.Y);
                DrawLegendItemString(p, settings, legendItemLocation, padding, stubWidth, maxLabelSize.Height);
                DrawLegendItemLine(p, settings, legendItemLocation, padding, stubWidth, maxLabelSize.Height);
                DrawLegendItemMarker(p, settings, legendItemLocation, padding, stubWidth, maxLabelSize.Height);
            }
        }

        public static SizeF MaxLegendLabelSize(Settings settings)
        {
            SizeF maxLabelSize = new SizeF();

            foreach (Plottable plottable in GetPlottableInLegend(settings))
            {
                SizeF labelSize = settings.gfxLegend.MeasureString(plottable.label, settings.legend.font);
                if (labelSize.Width > maxLabelSize.Width)
                    maxLabelSize.Width = labelSize.Width;
                if (labelSize.Height > maxLabelSize.Height)
                    maxLabelSize.Height = labelSize.Height;
            }

            return maxLabelSize;
        }

        private static void DrawLegendItemString(Plottable plottable, Settings settings, Point itemLocation, int padding, int stubWidth, float legendFontLineHeight)
        {
            Brush brushText = new SolidBrush(settings.legend.colorText);
            var textLocation = itemLocation + new Size(padding + stubWidth + padding, 0);
            settings.gfxLegend.DrawString(plottable.label, settings.legend.font, brushText, textLocation);
        }

        private static void DrawLegendItemLine(Plottable plottable, Settings settings, Point itemLocation, int padding, int stubWidth, float legendFontLineHeight)
        {
            Pen pen = new Pen(plottable.color, 1);

            if (plottable is PlottableAxSpan)
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
                itemLocation.X + padding, itemLocation.Y + legendFontLineHeight / 2,
                itemLocation.X + padding + stubWidth, itemLocation.Y + legendFontLineHeight / 2);
        }

        private static void DrawLegendItemMarker(Plottable plottable, Settings settings, Point itemLocation, int padding, int stubWidth, float legendFontLineHeight)
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

            PointF corner1 = new PointF(itemLocation.X + 2 * padding + settings.legend.font.Size / 4, itemLocation.Y + settings.legend.font.Size / 4 * padding);
            PointF center = new PointF
            {
                X = corner1.X + settings.legend.font.Size / 4,
                Y = corner1.Y + settings.legend.font.Size / 4
            };

            SizeF bounds = new SizeF(settings.legend.font.Size / 2, settings.legend.font.Size / 2);
            RectangleF rect = new RectangleF(corner1, bounds);

            Font textFont = new Font("CourierNew", settings.legend.font.Size);
            switch (plottable.markerShape)
            {
                case MarkerShape.none:
                    //Nothing to do because the Drawline needs to be there for all cases, and it's already there
                    break;
                case MarkerShape.asterisk:
                    Point markerPositionAsterisk = itemLocation + new Size(padding * 2, (int)(legendFontLineHeight / 4));
                    settings.gfxLegend.DrawString("*", textFont, brushMarker, markerPositionAsterisk);
                    break;
                case MarkerShape.cross:
                    Point markerPositionCross = itemLocation + new Size(padding * 2, (int)(legendFontLineHeight / 8));
                    settings.gfxLegend.DrawString("+", textFont, brushMarker, markerPositionCross);
                    break;
                case MarkerShape.eks:
                    Point markerPositionEks = itemLocation + new Size(padding * 2, 0);
                    settings.gfxLegend.DrawString("x", textFont, brushMarker, markerPositionEks);
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
                    Point markerPositionHashTag = itemLocation + new Size(padding * 2, (int)(legendFontLineHeight / 8));
                    settings.gfxLegend.DrawString("#", textFont, brushMarker, markerPositionHashTag);
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
                    Point markerPositionVertical = itemLocation + new Size(padding * 2, 0);
                    settings.gfxLegend.DrawString("|", textFont, brushMarker, markerPositionVertical);
                    break;
            }
        }

        private static Point GetFrameOffset(shadowDirection shadowDir)
        {
            switch (shadowDir)
            {
                case shadowDirection.lowerLeft:
                    return new Point(ShadowWidth, 0);
                case shadowDirection.lowerRight:
                    return new Point(0, 0);
                case shadowDirection.upperLeft:
                    return new Point(ShadowWidth, ShadowHeight);
                case shadowDirection.upperRight:
                    return new Point(0, ShadowHeight);
                case shadowDirection.none:
                    return new Point(0, 0);
                default:
                    return new Point(0, 0);
            }
        }

        private static Size GetFrameSize(Settings settings)
        {
            int stubWidth = 40 * (int)settings.legend.font.Size / 12;
            SizeF maxLabelSize = MaxLegendLabelSize(settings);
            int Width = padding * 2 + (int)maxLabelSize.Width + padding + stubWidth;
            int Height = padding * 2 + (int)maxLabelSize.Height * GetPlottableInLegend(settings).Count();
            return new Size(Width, Height);
        }

        private static Point GetFullFrameLocation(Settings settings, Size FrameSize)
        {
            int LeftX = padding;
            int CenterX = (settings.dataSize.Width - FrameSize.Width) / 2;
            int RightX = settings.dataSize.Width - FrameSize.Width - padding;

            int UpperY = padding;
            int CenterY = (settings.dataSize.Height - FrameSize.Height) / 2;
            int LowerY = settings.dataSize.Height - FrameSize.Height - padding;

            switch (settings.legend.location)
            {
                case legendLocation.upperLeft:
                    return new Point(LeftX, UpperY);
                case legendLocation.upperCenter:
                    return new Point(CenterX, UpperY);
                case legendLocation.upperRight:
                    return new Point(RightX, UpperY);

                case legendLocation.middleLeft:
                    return new Point(LeftX, CenterY);
                case legendLocation.middleRight:
                    return new Point(RightX, CenterY);

                case legendLocation.lowerLeft:
                    return new Point(LeftX, LowerY);
                case legendLocation.lowerCenter:
                    return new Point(CenterX, LowerY);
                case legendLocation.lowerRight:
                    return new Point(RightX, LowerY);

                case legendLocation.none:
                    return new Point(LeftX, UpperY);

                default:
                    throw new NotImplementedException($"legend location {settings.legend.location} is not supported");
            }
        }

        private static Size GetFullFrameSize(Size FrameSize, shadowDirection shadowDir)
        {
            if (shadowDir == shadowDirection.none)
                return FrameSize;
            else
                return new Size(FrameSize.Width + ShadowWidth, FrameSize.Height + ShadowHeight);
        }

        // Shadow offset in final FullFrame
        private static Point GetShadowOffset(shadowDirection shadowDir)
        {
            switch (shadowDir)
            {
                case shadowDirection.lowerLeft:
                    return new Point(0, ShadowHeight);
                case shadowDirection.lowerRight:
                    return new Point(ShadowWidth, ShadowHeight);
                case shadowDirection.upperLeft:
                    return new Point(0, 0);
                case shadowDirection.upperRight:
                    return new Point(ShadowWidth, 0);
                case shadowDirection.none:
                    return new Point(0, 0);
                default:
                    return new Point(0, 0);
            }
        }
    }
}
