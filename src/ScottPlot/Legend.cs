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
        private class LegendItem
        {
            public string label;
            public int plottableIndex;
            public Color color;
            public MarkerShape markerShape;

            public LegendItem(string label, int plottableIndex, Color color, MarkerShape markerShape)
            {
                this.label = label;
                this.plottableIndex = plottableIndex;
                this.color = color;
                this.markerShape = markerShape;
            }
        }

        public static void DrawLegend(Settings settings)
        {

            if (settings.legendLocation == legendLocation.none)
                return;

            int padding = 3;
            int stubWidth = 20;
            int stubHeight = 3;

            Brush brushText = new SolidBrush(settings.legendFontColor);
            float legendFontLineHeight = settings.gfxData.MeasureString("TEST", settings.legendFont).Height;
            float legendFontMaxWidth = 0;

            // populate list of filled labels
            List<LegendItem> legendItems = new List<LegendItem>();
            for (int i = 0; i < settings.plottables.Count(); i++)
            {
                if (settings.plottables[i].label != null)
                {
                    var legendItem = new LegendItem(settings.plottables[i].label, i, settings.plottables[i].color, settings.plottables[i].markerShape);
                    legendItems.Add(legendItem);
                    float thisItemFontWidth = settings.gfxData.MeasureString(settings.plottables[i].label, settings.legendFont).Width;
                    if (thisItemFontWidth > legendFontMaxWidth)
                        legendFontMaxWidth = thisItemFontWidth;
                }
            }
            legendItems.Reverse();

            // figure out where the legend should be
            float frameWidth = padding * 2 + legendFontMaxWidth + padding + stubWidth;
            float frameHeight = padding * 2 + legendFontLineHeight * legendItems.Count();
            Size frameSize = new Size((int)frameWidth, (int)frameHeight);
            Point frameLocation = new Point((int)(settings.dataSize.Width - frameWidth - padding),
                 (int)(settings.dataSize.Height - frameHeight - padding));
            Point textLocation = new Point(settings.dataSize.Width, settings.dataSize.Height);
            switch (settings.legendLocation)
            {
                case (legendLocation.none):
                    break;
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
                    frameLocation.X = (int)((settings.dataSize.Width) / 2 - frameWidth / 2 - padding * 5);
                    frameLocation.Y = (int)(padding);
                    textLocation.X = (int)(settings.dataSize.Width / 2 - legendFontMaxWidth / 2 + padding / 2);
                    textLocation.Y = (int)(frameHeight);
                    break;
                case (legendLocation.lowerCenter):
                    frameLocation.X = (int)((settings.dataSize.Width) / 2 - frameWidth / 2 - padding * 5);
                    frameLocation.Y = (int)(settings.dataSize.Height - frameHeight - padding);
                    textLocation.X = (int)(settings.dataSize.Width / 2 - legendFontMaxWidth / 2 + padding / 2);
                    textLocation.Y = settings.dataSize.Height - padding * 2;
                    break;
                case (legendLocation.middleLeft):
                    frameLocation.X = (int)(padding);
                    frameLocation.Y = (int)(settings.dataSize.Height / 2 - frameHeight / 2 - padding);
                    textLocation.X = (int)(frameWidth - legendFontMaxWidth + padding);
                    textLocation.Y = (int)(settings.dataSize.Height / 2 + frameHeight / 2 - padding * 2);
                    break;
                case (legendLocation.middleRight):
                    frameLocation.X = (int)(settings.dataSize.Width - frameWidth - padding);
                    frameLocation.Y = (int)(settings.dataSize.Height / 2 - frameHeight / 2 - padding);
                    textLocation.X = (int)(settings.dataSize.Width - (legendFontMaxWidth + padding));
                    textLocation.Y = (int)(settings.dataSize.Height / 2 + frameHeight / 2 - padding * 2);
                    break;
                default:
                    throw new NotImplementedException($"legend location {settings.legendLocation} is not supported");
            }
            Rectangle frameRect = new Rectangle(frameLocation, frameSize);

            // figure out where the legend should be
            Point shadowLocation = new Point();
            switch (settings.legendShadowDirection)
            {
                case (shadowDirection.lowerRight):
                    shadowLocation.X = frameRect.X + 2;
                    shadowLocation.Y = frameRect.Y + 2;
                    break;
                case (shadowDirection.lowerLeft):
                    shadowLocation.X = frameRect.X - 2;
                    shadowLocation.Y = frameRect.Y + 2;
                    break;
                case (shadowDirection.upperRight):
                    shadowLocation.X = frameRect.X + 2;
                    shadowLocation.Y = frameRect.Y - 2;
                    break;
                case (shadowDirection.upperLeft):
                    shadowLocation.X = frameRect.X - 2;
                    shadowLocation.Y = frameRect.Y - 2;
                    break;
                default:
                    settings.legendShadowDirection = shadowDirection.none;
                    break;
            }
            Rectangle shadowRect = new Rectangle(shadowLocation, frameSize);

            // draw the legend background

            if (settings.legendLocation != legendLocation.none)
            {
                if (settings.legendShadowDirection != shadowDirection.none)
                    settings.gfxData.FillRectangle(new SolidBrush(settings.legendShadowColor), shadowRect);
                settings.gfxData.FillRectangle(new SolidBrush(settings.legendBackColor), frameRect);
                settings.gfxData.DrawRectangle(new Pen(settings.legendFrameColor), frameRect);
            }

            if (settings.legendLocation != legendLocation.none)
            {
                for (int i = 0; i < legendItems.Count; i++)
                {
                    textLocation.Y -= (int)(legendFontLineHeight);

                    settings.gfxData.DrawString(legendItems[i].label, settings.legendFont, brushText, textLocation);

                    //Determine whether a standard stub or a marker shall be plotted
                    Brush brushMarker = new SolidBrush(legendItems[i].color);
                    Pen pen = new Pen(legendItems[i].color);
                    PointF corner1 = new PointF(textLocation.X - stubWidth + 5, textLocation.Y + 3 * padding);
                    PointF center = new PointF
                    {
                        X = corner1.X + 3,
                        Y = corner1.Y + 3
                    };

                    SizeF bounds = new SizeF(5, 5);
                    RectangleF rect = new RectangleF(corner1, bounds);

                    switch (legendItems[i].markerShape)
                    {
                        case MarkerShape.none:
                            settings.gfxData.DrawLine(new Pen(legendItems[i].color, stubHeight),
                                textLocation.X - padding, textLocation.Y + legendFontLineHeight / 2,
                                textLocation.X - padding - stubWidth, textLocation.Y + legendFontLineHeight / 2);
                            break;
                        case MarkerShape.asterisk:
                            Font drawFontAsterisk = new Font("CourierNew", 16);
                            Point markerPositionAsterisk = new Point(textLocation.X - stubWidth, (int)(textLocation.Y + legendFontLineHeight / 4));
                            settings.gfxData.DrawString("*", drawFontAsterisk, brushMarker, markerPositionAsterisk);
                            break;
                        case MarkerShape.cross:
                            Font drawFontCross = new Font("CourierNew", 12);
                            Point markerPositionCross = new Point(textLocation.X - stubWidth, (int)(textLocation.Y + legendFontLineHeight / 8));
                            settings.gfxData.DrawString("+", drawFontCross, brushMarker, markerPositionCross);
                            break;
                        case MarkerShape.eks:
                            Font drawFontEks = new Font("CourierNew", 12);
                            Point markerPositionEks = new Point(textLocation.X - stubWidth, (int)(textLocation.Y));
                            settings.gfxData.DrawString("x", drawFontEks, brushMarker, markerPositionEks);
                            break;
                        case MarkerShape.filledCircle:
                            settings.gfxData.FillEllipse(brushMarker, rect);
                            break;
                        case MarkerShape.filledDiamond:
                            // Create points that define polygon.
                            PointF point1 = new PointF(center.X, center.Y + 3);
                            PointF point2 = new PointF(center.X - 3, center.Y);
                            PointF point3 = new PointF(center.X, center.Y - 3);
                            PointF point4 = new PointF(center.X + 3, center.Y);

                            PointF[] curvePoints = { point1, point2, point3, point4 };

                            //Draw polygon to screen
                            settings.gfxData.FillPolygon(brushMarker, curvePoints);
                            break;
                        case MarkerShape.filledSquare:
                            settings.gfxData.FillRectangle(brushMarker, rect);
                            break;
                        case MarkerShape.hashTag:
                            Font drawFontHashtag = new Font("CourierNew", 12);
                            Point markerPositionHashTag = new Point(textLocation.X - stubWidth, (int)(textLocation.Y + legendFontLineHeight / 8));
                            settings.gfxData.DrawString("#", drawFontHashtag, brushMarker, markerPositionHashTag);
                            break;
                        case MarkerShape.openCircle:
                            settings.gfxData.DrawEllipse(pen, rect);
                            break;
                        case MarkerShape.openDiamond:
                            // Create points that define polygon.
                            PointF point5 = new PointF(center.X, center.Y + 3);
                            PointF point6 = new PointF(center.X - 3, center.Y);
                            PointF point7 = new PointF(center.X, center.Y - 3);
                            PointF point8 = new PointF(center.X + 3, center.Y);

                            PointF[] curvePoints2 = { point5, point6, point7, point8 };

                            //Draw polygon to screen
                            settings.gfxData.DrawPolygon(pen, curvePoints2);
                            break;
                        case MarkerShape.openSquare:
                            settings.gfxData.DrawRectangle(pen, corner1.X, corner1.Y, 5, 5);
                            break;
                        case MarkerShape.triDown:
                            // Create points that define polygon.
                            PointF point14 = new PointF(center.X, center.Y + 6);
                            PointF point15 = new PointF(center.X, center.Y);
                            PointF point16 = new PointF(center.X - 6 * (float)0.866, center.Y - 6 * (float)0.5);
                            PointF point17 = new PointF(center.X, center.Y);
                            PointF point18 = new PointF(center.X + 6 * (float)0.866, center.Y - 6 * (float)0.5);


                            PointF[] curvePoints4 = { point17, point14, point15, point16, point17, point18 };

                            //Draw polygon to screen
                            settings.gfxData.DrawPolygon(pen, curvePoints4);

                            break;

                        case MarkerShape.triUp:
                            // Create points that define polygon.
                            PointF point9 = new PointF(center.X, center.Y - 6);
                            PointF point10 = new PointF(center.X, center.Y);
                            PointF point11 = new PointF(center.X - 6 * (float)0.866, center.Y + 6 * (float)0.5);
                            PointF point12 = new PointF(center.X, center.Y);
                            PointF point13 = new PointF(center.X + 6 * (float)0.866, center.Y + 6 * (float)0.5);


                            PointF[] curvePoints3 = { point12, point9, point10, point11, point12, point13 };
                            //Draw polygon to screen
                            settings.gfxData.DrawPolygon(pen, curvePoints3);
                            break;

                        case MarkerShape.verticalBar:
                            Font drawFontVertical = new Font("CourierNew", 12);
                            Point markerPositionVertical = new Point(textLocation.X - stubWidth, (int)(textLocation.Y));
                            settings.gfxData.DrawString("|", drawFontVertical, brushMarker, markerPositionVertical);
                            break;


                    }
                }
            }
        }
    }
}
