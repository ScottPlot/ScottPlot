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
        private const int shadowWidth = 2;
        private const int shadowHeight = 2;

        private static IEnumerable<Plottable> GetPlottableInLegend(Settings settings)
        {
            return settings.plottables.Where(p => p.visible && p.label != null);
        }

        public static Rectangle GetLegendFrame(Settings settings)
        {
            Size frameSize = GetFrameSize(settings);

            Point fullFrameLocation = GetFullFrameLocation(settings, frameSize);
            Size fullFrameSize = GetFullFrameSize(frameSize, settings.legend.shadow);
            return new Rectangle(fullFrameLocation, Size.Add(fullFrameSize, new Size(1, 1)));
        }

        public static void DrawLegend(Settings settings)
        {
            SizeF maxLabelSize = MaxLegendLabelSize(settings);
            int stubWidth = 40 * (int)settings.legend.font.Size / 12;

            Size frameSize = GetFrameSize(settings);
            Point frameOffset = GetFrameOffset(settings.legend.shadow);
            Rectangle frameRect = new Rectangle(frameOffset, frameSize);

            settings.gfxLegend.Clear(settings.legend.colorBackground);

            if (settings.legend.shadow != shadowDirection.none)
            {
                Point shadowOffset = GetShadowOffset(settings.legend.shadow);
                Rectangle shadowRect = new Rectangle(shadowOffset, frameSize);
                settings.gfxLegend.FillRectangle(new SolidBrush(settings.legend.colorShadow), shadowRect);
            }

            settings.gfxLegend.FillRectangle(new SolidBrush(settings.legend.colorBackground), frameRect);
            settings.gfxLegend.DrawRectangle(new Pen(settings.legend.colorFrame), frameRect);

            foreach (var (p, index) in GetPlottableInLegend(settings).Select((x, i) => (x, i)))
            {
                Point legendItemLocation = new Point(frameOffset.X,
                    padding + index * (int)(maxLabelSize.Height) + frameOffset.Y);
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
                    PointF[] curvePoints =
                    {
                        new PointF(center.X, center.Y + settings.legend.font.Size / 4),
                        new PointF(center.X - settings.legend.font.Size / 4, center.Y),
                        new PointF(center.X, center.Y - settings.legend.font.Size / 4),
                        new PointF(center.X + settings.legend.font.Size / 4, center.Y),
                    };
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
                    PointF[] curvePoints2 =
                    {
                        new PointF(center.X, center.Y + settings.legend.font.Size / 4),
                        new PointF(center.X - settings.legend.font.Size / 4, center.Y),
                        new PointF(center.X, center.Y - settings.legend.font.Size / 4),
                        new PointF(center.X + settings.legend.font.Size / 4, center.Y),
                    };
                    //Draw polygon to screen
                    settings.gfxLegend.DrawPolygon(penMarker, curvePoints2);
                    break;
                case MarkerShape.openSquare:
                    settings.gfxLegend.DrawRectangle(penMarker, corner1.X, corner1.Y, settings.legend.font.Size / 2, settings.legend.font.Size / 2);
                    break;
                case MarkerShape.triDown:
                    // Create points that define polygon.
                    PointF[] curvePoints4 =
                    {
                        new PointF(center.X, center.Y),
                        new PointF(center.X, center.Y + settings.legend.font.Size / 2),
                        new PointF(center.X, center.Y),
                        new PointF(center.X - settings.legend.font.Size / 2 * (float)0.866, center.Y - settings.legend.font.Size / 2 * (float)0.5),
                        new PointF(center.X, center.Y),
                        new PointF(center.X + settings.legend.font.Size / 2 * (float)0.866, center.Y - settings.legend.font.Size / 2 * (float)0.5),
                    };
                    //Draw polygon to screen
                    settings.gfxLegend.DrawPolygon(penMarker, curvePoints4);
                    break;
                case MarkerShape.triUp:
                    // Create points that define polygon.
                    PointF[] curvePoints3 =
                    {
                        new PointF(center.X, center.Y),
                        new PointF(center.X, center.Y - settings.legend.font.Size / 2),
                        new PointF(center.X, center.Y),
                        new PointF(center.X - settings.legend.font.Size / 2 * (float)0.866, center.Y + settings.legend.font.Size / 2 * (float)0.5),
                        new PointF(center.X, center.Y),
                        new PointF(center.X + settings.legend.font.Size / 2 * (float)0.866, center.Y + settings.legend.font.Size / 2 * (float)0.5),
                    };
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
                    return new Point(shadowWidth, 0);
                case shadowDirection.lowerRight:
                    return new Point(0, 0);
                case shadowDirection.upperLeft:
                    return new Point(shadowWidth, shadowHeight);
                case shadowDirection.upperRight:
                    return new Point(0, shadowHeight);
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
            int width = padding * 2 + (int)maxLabelSize.Width + padding + stubWidth;
            int height = padding * 2 + (int)maxLabelSize.Height * GetPlottableInLegend(settings).Count();
            return new Size(width, height);
        }

        private static Point GetFullFrameLocation(Settings settings, Size frameSize)
        {
            int framePadding = padding * 2;

            int leftX = framePadding;
            int centerX = (settings.dataSize.Width - frameSize.Width) / 2;
            int rightX = settings.dataSize.Width - frameSize.Width - framePadding;

            int upperY = framePadding;
            int centerY = (settings.dataSize.Height - frameSize.Height) / 2;
            int lowerY = settings.dataSize.Height - frameSize.Height - framePadding;

            switch (settings.legend.location)
            {
                case legendLocation.upperLeft:
                    return new Point(leftX, upperY);
                case legendLocation.upperCenter:
                    return new Point(centerX, upperY);
                case legendLocation.upperRight:
                    return new Point(rightX, upperY);

                case legendLocation.middleLeft:
                    return new Point(leftX, centerY);
                case legendLocation.middleRight:
                    return new Point(rightX, centerY);

                case legendLocation.lowerLeft:
                    return new Point(leftX, lowerY);
                case legendLocation.lowerCenter:
                    return new Point(centerX, lowerY);
                case legendLocation.lowerRight:
                    return new Point(rightX, lowerY);

                case legendLocation.none:
                    return new Point(leftX, upperY);

                default:
                    throw new NotImplementedException($"legend location {settings.legend.location} is not supported");
            }
        }

        private static Size GetFullFrameSize(Size frameSize, shadowDirection shadowDir)
        {
            if (shadowDir == shadowDirection.none)
                return frameSize;
            else
                return new Size(frameSize.Width + shadowWidth, frameSize.Height + shadowHeight);
        }

        // Shadow offset in final FullFrame
        private static Point GetShadowOffset(shadowDirection shadowDir)
        {
            switch (shadowDir)
            {
                case shadowDirection.lowerLeft:
                    return new Point(0, shadowHeight);
                case shadowDirection.lowerRight:
                    return new Point(shadowWidth, shadowHeight);
                case shadowDirection.upperLeft:
                    return new Point(0, 0);
                case shadowDirection.upperRight:
                    return new Point(shadowWidth, 0);
                case shadowDirection.none:
                    return new Point(0, 0);
                default:
                    return new Point(0, 0);
            }
        }
    }
}
