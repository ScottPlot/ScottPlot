using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Drawing;

namespace ScottPlot
{
    // TODO: capitalize these in 4.1
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

    // TODO: capitalize these in 4.1
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

        public static Config.LegendItem[] GetLegendItems(Settings settings)
        {
            // todo: linq
            var items = new List<Config.LegendItem>();

            foreach (Plottable plottable in settings.plottables)
            {
                var legendItems = plottable.GetLegendItems();

                if (legendItems is null)
                    continue;

                foreach (var plottableItem in legendItems)
                    if (plottableItem.label != null)
                        items.Add(plottableItem);
            }

            if (settings.legend.reverseOrder)
                items.Reverse();

            return items.ToArray();
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

            var legendItems = GetLegendItems(settings);
            for (int i = 0; i < legendItems.Length; i++)
            {
                float itemY = padding + i * maxLabelSize.Height + frameOffset.Y;
                float itemX = frameOffset.X;
                PointF legendItemLocation = new PointF(itemX, itemY);
                DrawLegendItemString(legendItems[i], settings, legendItemLocation, padding, stubWidth, maxLabelSize.Height);
                DrawLegendItemLine(legendItems[i], settings, legendItemLocation, padding, stubWidth, maxLabelSize.Height);
                DrawLegendItemMarker(legendItems[i], settings, legendItemLocation, padding, stubWidth, maxLabelSize.Height);
            }
        }

        private static SizeF MaxLegendLabelSize(Settings settings)
        {
            SizeF maxLabelSize = new SizeF();

            foreach (var legendItem in GetLegendItems(settings))
            {
                SizeF labelSize = Drawing.GDI.MeasureString(settings.gfxLegend, legendItem.label, settings.legend.font);
                if (labelSize.Width > maxLabelSize.Width)
                    maxLabelSize.Width = labelSize.Width;
                if (labelSize.Height > maxLabelSize.Height)
                    maxLabelSize.Height = labelSize.Height;
            }

            return maxLabelSize;
        }

        private static void DrawLegendItemString(Config.LegendItem legendItem, Settings settings, PointF itemLocation, int padding, int stubWidth, float legendFontLineHeight)
        {
            Brush brushText = new SolidBrush(settings.legend.colorText);
            var textLocation = itemLocation + new Size(padding + stubWidth + padding, 0);
            settings.gfxLegend.DrawString(legendItem.label, settings.legend.font, brushText, textLocation);
        }

        private static void DrawLegendItemLine(Config.LegendItem legendItem, Settings settings, PointF itemLocation, int padding, int stubWidth, float ySpaceBetweenItems)
        {
            if (legendItem.lineWidth == 0 || legendItem.lineStyle == LineStyle.None)
                return;

            float yCenter = itemLocation.Y + ySpaceBetweenItems / 2;
            PointF xMin = new PointF(itemLocation.X + padding, yCenter);
            PointF xMax = new PointF(itemLocation.X + padding + stubWidth, yCenter);

            if (legendItem.lineWidth < 10)
            {
                Pen pen = GDI.Pen(legendItem.color, (float)legendItem.lineWidth, legendItem.lineStyle);
                settings.gfxLegend.DrawLine(pen, xMin, xMax);
            }
            else
            {
                Brush brush = new SolidBrush(legendItem.color);
                float halfHeight = (float)(legendItem.lineWidth / 2);
                PointF topLeft = new PointF(xMin.X, xMin.Y - halfHeight);
                float symbolWidth = xMax.X - xMin.X;
                settings.gfxLegend.FillRectangle(brush, topLeft.X, topLeft.Y, symbolWidth, (float)legendItem.lineWidth);
            }
        }

        private static void DrawLegendItemMarker(Config.LegendItem legendItem, Settings settings, PointF itemLocation, int padding, int stubWidth, float legendFontLineHeight)
        {
            if (legendItem.markerSize == 0 || legendItem.markerShape == MarkerShape.none)
                return;

            Brush brushMarker = new SolidBrush(legendItem.color);
            Pen penMarker = new Pen(legendItem.color, 1);

            PointF corner1 = new PointF(itemLocation.X + 2 * padding + settings.legend.font.Size / 4, itemLocation.Y + settings.legend.font.Size / 4 * padding);
            PointF center = new PointF
            {
                X = corner1.X + settings.legend.font.Size / 4,
                Y = corner1.Y + settings.legend.font.Size / 4
            };

            RectangleF rect = new RectangleF(
                    x: itemLocation.X + 2 * padding + settings.legend.font.Size / 4,
                    y: itemLocation.Y + settings.legend.font.Size / 4 + padding,
                    width: settings.legend.font.Size / 2,
                    height: settings.legend.font.Size / 2
                    );

            Font textFont = new Font("CourierNew", settings.legend.font.Size);

            // TODO: move this switch to tools or something. 
            // Sureley the legend isn't the only place this switch lives.
            switch (legendItem.markerShape)
            {
                case MarkerShape.none:
                    //Nothing to do because the Drawline needs to be there for all cases, and it's already there
                    break;
                case MarkerShape.asterisk:
                    PointF markerPositionAsterisk = itemLocation + new Size(padding * 2, (int)(legendFontLineHeight / 4));
                    settings.gfxLegend.DrawString("*", textFont, brushMarker, markerPositionAsterisk);
                    break;
                case MarkerShape.cross:
                    PointF markerPositionCross = itemLocation + new Size(padding * 2, (int)(legendFontLineHeight / 8));
                    settings.gfxLegend.DrawString("+", textFont, brushMarker, markerPositionCross);
                    break;
                case MarkerShape.eks:
                    PointF markerPositionEks = itemLocation + new Size(padding * 2, 0);
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
                    PointF markerPositionHashTag = itemLocation + new Size(padding * 2, (int)(legendFontLineHeight / 8));
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
                    PointF markerPositionVertical = itemLocation + new Size(padding * 2, 0);
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
            int height = padding * 2 + (int)maxLabelSize.Height * GetLegendItems(settings).Count();
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
