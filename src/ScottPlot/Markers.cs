using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ScottPlot
{
    public enum MarkerShape
    {
        // TODO: replace this with ScottPlot.Marker in the next major version of ScottPlot
        none,
        filledCircle,
        filledSquare,
        openCircle,
        openSquare,
        filledDiamond,
        openDiamond,
        asterisk,
        hashTag,
        cross,
        eks,
        verticalBar,
        triUp,
        triDown,
    }

    public class MarkerTools
    {
        public static void DrawMarker(Graphics gfx, PointF pixelLocation, MarkerShape shape, float size, Color color)
        {
            if (size == 0 || shape == MarkerShape.none)
                return;

            Pen pen = new Pen(color);

            Brush brush = new SolidBrush(color);

            PointF corner1 = new PointF(pixelLocation.X - size / 2, pixelLocation.Y - size / 2);
            PointF corner2 = new PointF(pixelLocation.X + size / 2, pixelLocation.Y + size / 2);
            SizeF bounds = new SizeF(size, size);
            RectangleF rect = new RectangleF(corner1, bounds);

            switch (shape)
            {
                case MarkerShape.filledCircle:
                    gfx.FillEllipse(brush, rect);
                    break;
                case MarkerShape.openCircle:
                    gfx.DrawEllipse(pen, rect);
                    break;
                case MarkerShape.filledSquare:
                    gfx.FillRectangle(brush, rect);
                    break;
                case MarkerShape.openSquare:
                    gfx.DrawRectangle(pen, corner1.X, corner1.Y, size, size);
                    break;
                case MarkerShape.filledDiamond:

                    // Create points that define polygon.
                    PointF point1 = new PointF(pixelLocation.X, pixelLocation.Y + size / 2);
                    PointF point2 = new PointF(pixelLocation.X - size / 2, pixelLocation.Y);
                    PointF point3 = new PointF(pixelLocation.X, pixelLocation.Y - size / 2);
                    PointF point4 = new PointF(pixelLocation.X + size / 2, pixelLocation.Y);

                    PointF[] curvePoints = { point1, point2, point3, point4 };

                    //Draw polygon to screen
                    gfx.FillPolygon(brush, curvePoints);
                    break;
                case MarkerShape.openDiamond:

                    // Create points that define polygon.
                    PointF point5 = new PointF(pixelLocation.X, pixelLocation.Y + size / 2);
                    PointF point6 = new PointF(pixelLocation.X - size / 2, pixelLocation.Y);
                    PointF point7 = new PointF(pixelLocation.X, pixelLocation.Y - size / 2);
                    PointF point8 = new PointF(pixelLocation.X + size / 2, pixelLocation.Y);

                    PointF[] curvePoints2 = { point5, point6, point7, point8 };

                    //Draw polygon to screen
                    gfx.DrawPolygon(pen, curvePoints2);

                    break;
                case MarkerShape.asterisk:
                    Font drawFont = new Font("CourierNew", size * 3);
                    SizeF textSize = Drawing.GDI.MeasureString(gfx, "*", drawFont);
                    PointF asteriskPoint = new PointF(pixelLocation.X - textSize.Width / 2, pixelLocation.Y - textSize.Height / 4);
                    gfx.DrawString("*", drawFont, brush, asteriskPoint);

                    break;
                case MarkerShape.hashTag:
                    Font drawFont2 = new Font("CourierNew", size * 2);
                    SizeF textSize2 = Drawing.GDI.MeasureString(gfx, "#", drawFont2);
                    PointF asteriskPoint2 = new PointF(pixelLocation.X - textSize2.Width / 2, pixelLocation.Y - textSize2.Height / 3);
                    gfx.DrawString("#", drawFont2, brush, asteriskPoint2);

                    break;
                case MarkerShape.cross:
                    Font drawFont3 = new Font("CourierNew", size * 2);
                    SizeF textSize3 = Drawing.GDI.MeasureString(gfx, "+", drawFont3);
                    PointF asteriskPoint3 = new PointF(pixelLocation.X - textSize3.Width / (5 / 2), pixelLocation.Y - textSize3.Height / 2);
                    gfx.DrawString("+", drawFont3, brush, asteriskPoint3);

                    break;
                case MarkerShape.eks:
                    Font drawFont4 = new Font("CourierNew", size * 2);
                    SizeF textSize4 = Drawing.GDI.MeasureString(gfx, "x", drawFont4);
                    PointF asteriskPoint4 = new PointF(pixelLocation.X - textSize4.Width / (5 / 2), pixelLocation.Y - textSize4.Height / 2);
                    gfx.DrawString("x", drawFont4, brush, asteriskPoint4);

                    break;
                case MarkerShape.verticalBar:
                    Font drawFont5 = new Font("CourierNew", size * 2);
                    SizeF textSize5 = Drawing.GDI.MeasureString(gfx, "|", drawFont5);
                    PointF asteriskPoint5 = new PointF(pixelLocation.X - textSize5.Width / (5 / 2), pixelLocation.Y - textSize5.Height / 2);
                    gfx.DrawString("|", drawFont5, brush, asteriskPoint5);

                    break;
                case MarkerShape.triUp:
                    // Create points that define polygon.
                    PointF point9 = new PointF(pixelLocation.X, pixelLocation.Y - size);
                    PointF point10 = new PointF(pixelLocation.X, pixelLocation.Y);
                    PointF point11 = new PointF(pixelLocation.X - size * (float)0.866, pixelLocation.Y + size * (float)0.5);
                    PointF point12 = new PointF(pixelLocation.X, pixelLocation.Y);
                    PointF point13 = new PointF(pixelLocation.X + size * (float)0.866, (pixelLocation.Y + size * (float)0.5));


                    PointF[] curvePoints3 = { point12, point9, point10, point11, point12, point13 };

                    //Draw polygon to screen
                    gfx.DrawPolygon(pen, curvePoints3);

                    break;
                case MarkerShape.triDown:
                    // Create points that define polygon.
                    PointF point14 = new PointF(pixelLocation.X, pixelLocation.Y + size);
                    PointF point15 = new PointF(pixelLocation.X, pixelLocation.Y);
                    PointF point16 = new PointF(pixelLocation.X - size * (float)0.866, pixelLocation.Y - size * (float)0.5);
                    PointF point17 = new PointF(pixelLocation.X, pixelLocation.Y);
                    PointF point18 = new PointF(pixelLocation.X + size * (float)0.866, (pixelLocation.Y - size * (float)0.5));


                    PointF[] curvePoints4 = { point17, point14, point15, point16, point17, point18 };

                    //Draw polygon to screen
                    gfx.DrawPolygon(pen, curvePoints4);

                    break;
                case MarkerShape.none:
                    break;
                default:
                    throw new NotImplementedException($"unsupported marker type: {shape}");
            }
        }

        public static void DrawMarkers(Graphics gfx, ICollection<PointF> pixelLocations, MarkerShape shape, float size, Color color)
        {
            if (size == 0 || shape == MarkerShape.none)
                return;

            Pen pen = new Pen(color);

            Brush brush = new SolidBrush(color);

            float halfsize = size / 2;
            float quartersize = size / 4;
            float halfsqrt3size = (float)0.866 * size;
            float halfsqrt2size = (float)0.707 * size;
            float quartersqrt2size = halfsqrt2size / 2;
            float quartersqrt3size = halfsqrt3size / 2;

            // adjust marker offset to improve rendering on Linux and MacOS
            float markerOffsetX = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? 0 : 1;
            switch (shape)
            {
                case MarkerShape.filledCircle:
                    foreach (PointF point in pixelLocations)
                    {
                        gfx.FillEllipse(brush: brush, x: point.X - halfsize + markerOffsetX, y: point.Y - halfsize, width: size, height: size);
                    }

                    break;
                case MarkerShape.openCircle:
                    foreach (PointF point in pixelLocations)
                    {
                        gfx.DrawEllipse(pen: pen, x: point.X - halfsize + markerOffsetX, y: point.Y - halfsize, width: size, height: size);
                    }

                    break;
                case MarkerShape.filledSquare:
                    foreach (PointF point in pixelLocations)
                    {
                        gfx.FillRectangle(brush: brush, x: point.X - halfsize + markerOffsetX, y: point.Y - halfsize, width: size, height: size);
                    }

                    break;
                case MarkerShape.openSquare:
                    foreach (PointF point in pixelLocations)
                    {
                        gfx.DrawRectangle(pen: pen, x: point.X - halfsize + markerOffsetX, y: point.Y - halfsize, width: size, height: size);
                    }

                    break;
                case MarkerShape.filledDiamond:
                    foreach (PointF point in pixelLocations)
                    {
                        // Create points that define polygon.
                        PointF point1 = new PointF(point.X + markerOffsetX, point.Y + halfsize);
                        PointF point2 = new PointF(point.X + markerOffsetX - halfsize, point.Y);
                        PointF point3 = new PointF(point.X + markerOffsetX, point.Y - halfsize);
                        PointF point4 = new PointF(point.X + markerOffsetX + halfsize, point.Y);

                        PointF[] curvePoints = { point1, point2, point3, point4 };

                        //Fill polygon to screen
                        gfx.FillPolygon(brush, curvePoints);
                    }

                    break;
                case MarkerShape.openDiamond:
                    foreach (PointF point in pixelLocations)
                    {
                        // Create points that define polygon.
                        PointF point5 = new PointF(point.X + markerOffsetX, point.Y + halfsize);
                        PointF point6 = new PointF(point.X + markerOffsetX - halfsize, point.Y);
                        PointF point7 = new PointF(point.X + markerOffsetX, point.Y - halfsize);
                        PointF point8 = new PointF(point.X + markerOffsetX + halfsize, point.Y);

                        PointF[] curvePoints2 = { point5, point6, point7, point8 };

                        //Draw polygon to screen
                        gfx.DrawPolygon(pen, curvePoints2);
                    }

                    break;
                case MarkerShape.asterisk:
                    //Font drawFont = new Font("CourierNew", size * 3);
                    //SizeF textSize = Drawing.GDI.MeasureString(gfx, "*", drawFont);
                    //float halfWidth = textSize.Width / 2;
                    //float quarterHeight = textSize.Height / 4;
                    foreach (PointF point in pixelLocations)
                    {
                        //gfx.DrawString("*", drawFont, brush, point.X - halfWidth, point.Y - quarterHeight);
                        // horizontal line of the *
                        gfx.DrawLine(pen, point.X + markerOffsetX - halfsize, point.Y, point.X + markerOffsetX + halfsize, point.Y);
                        // vertical line of the *
                        gfx.DrawLine(pen, point.X + markerOffsetX, point.Y - halfsize, point.X + markerOffsetX, point.Y + halfsize);
                        // bottom-left / top-right line of the *
                        gfx.DrawLine(pen, point.X + markerOffsetX - quartersqrt2size, point.Y - quartersqrt2size, point.X + markerOffsetX + quartersqrt2size, point.Y + quartersqrt2size);
                        // top-left / bottom-right line of the *
                        gfx.DrawLine(pen, point.X + markerOffsetX - quartersqrt2size, point.Y + quartersqrt2size, point.X + markerOffsetX + quartersqrt2size, point.Y - quartersqrt2size);
                    }

                    break;
                case MarkerShape.hashTag:
                    Font drawFont2 = new Font("CourierNew", size * 2);
                    SizeF textSize2 = Drawing.GDI.MeasureString(gfx, "#", drawFont2);
                    float halfWidth2 = textSize2.Width / 2;
                    float quarterHeight2 = textSize2.Height / 4;
                    foreach (PointF point in pixelLocations)
                    {
                        gfx.DrawString("#", drawFont2, brush, point.X - halfWidth2, point.Y - quarterHeight2);
                    }

                    break;
                case MarkerShape.cross:
                    //Font drawFont3 = new Font("CourierNew", size * 2);
                    //SizeF textSize3 = Drawing.GDI.MeasureString(gfx, "+", drawFont3);
                    //float halfWidth3 = textSize3.Width / (5 / 2);
                    //float quarterHeight3 = textSize3.Height / 2;
                    foreach (PointF point in pixelLocations)
                    {
                        //gfx.DrawString("+", drawFont3, brush, point.X - halfWidth3, point.Y - quarterHeight3);
                        // horizontal line of the +
                        gfx.DrawLine(pen, point.X + markerOffsetX - halfsize, point.Y, point.X + markerOffsetX + halfsize, point.Y);
                        // vertical line of the +
                        gfx.DrawLine(pen, point.X + markerOffsetX, point.Y - halfsize, point.X + markerOffsetX, point.Y + halfsize);
                    }

                    break;
                case MarkerShape.eks:
                    //Font drawFont4 = new Font("CourierNew", size * 2);
                    //SizeF textSize4 = Drawing.GDI.MeasureString(gfx, "x", drawFont4);
                    //float halfWidth4 = textSize4.Width / (5 / 2);
                    //float quarterHeight4 = textSize4.Height / 2;
                    foreach (PointF point in pixelLocations)
                    {
                        //gfx.DrawString("x", drawFont4, brush, point.X - halfWidth4, point.Y - quarterHeight4);
                        // bottom-left / top-right line of the x
                        gfx.DrawLine(pen, point.X + markerOffsetX - quartersqrt2size, point.Y - quartersqrt2size, point.X + markerOffsetX + quartersqrt2size, point.Y + quartersqrt2size);
                        // top-left / bottom-right line of the x
                        gfx.DrawLine(pen, point.X + markerOffsetX - quartersqrt2size, point.Y + quartersqrt2size, point.X + markerOffsetX + quartersqrt2size, point.Y - quartersqrt2size);
                    }

                    break;
                case MarkerShape.verticalBar:
                    //Font drawFont5 = new Font("CourierNew", size * 2);
                    //SizeF textSize5 = Drawing.GDI.MeasureString(gfx, "|", drawFont5);
                    //float halfWidth5 = textSize5.Width / (5 / 2);
                    //float quarterHeight5 = textSize5.Height / 2;
                    foreach (PointF point in pixelLocations)
                    {
                        //gfx.DrawString("|", drawFont5, brush, point.X - halfWidth5, point.Y - quarterHeight5);
                        // vertical line of the *
                        gfx.DrawLine(pen, point.X + markerOffsetX, point.Y - halfsize, point.X + markerOffsetX, point.Y + halfsize);
                    }

                    break;
                case MarkerShape.triUp:
                    foreach (PointF point in pixelLocations)
                    {
                        // Create points that define polygon.
                        PointF point9 = new PointF(point.X + markerOffsetX, point.Y - halfsize);
                        PointF point10 = new PointF(point.X + markerOffsetX, point.Y);
                        PointF point11 = new PointF(point.X + markerOffsetX - quartersqrt3size, point.Y + quartersize);
                        PointF point12 = new PointF(point.X + markerOffsetX, point.Y);
                        PointF point13 = new PointF(point.X + markerOffsetX + quartersqrt3size, point.Y + quartersize);

                        PointF[] curvePoints3 = { point12, point9, point10, point11, point12, point13 };

                        //Draw polygon to screen
                        gfx.DrawPolygon(pen, curvePoints3);
                    }

                    break;
                case MarkerShape.triDown:
                    foreach (PointF point in pixelLocations)
                    {
                        // Create points that define polygon.
                        PointF point14 = new PointF(point.X + markerOffsetX, point.Y + halfsize);
                        PointF point15 = new PointF(point.X + markerOffsetX, point.Y);
                        PointF point16 = new PointF(point.X + markerOffsetX - quartersqrt3size, point.Y - quartersize);
                        PointF point17 = new PointF(point.X + markerOffsetX, point.Y);
                        PointF point18 = new PointF(point.X + markerOffsetX + quartersqrt3size, point.Y - quartersize);

                        PointF[] curvePoints4 = { point17, point14, point15, point16, point17, point18 };

                        //Draw polygon to screen
                        gfx.DrawPolygon(pen, curvePoints4);
                    }

                    break;
                case MarkerShape.none:
                    break;
                default:
                    throw new NotImplementedException($"unsupported marker type: {shape}");
            }
        }
    }
}
