using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class Ticks : IRenderable
    {
        public Edge Edge;

        public Color MajorTickColor = Color.Black;
        public float MajorTickLength = 5;
        public double[] MajorTickPositions;
        public string[] MajorTickLabels;

        public Color MinorTickColor = Color.Black;
        public float MinorTickLength = 2;
        public double[] MinorTickPositions;

        public Color FontColor = Color.Black;
        public float FontSize = 9;
        public string FontName = Fonts.GetDefaultFontName();

        public void Render(Settings settings)
        {
            using (Graphics gfx = Graphics.FromImage(settings.Bitmap))
            using (Font font = new Font(FontName, FontSize))
            using (StringFormat sf = new StringFormat())
            using (Brush fontBrush = new SolidBrush(FontColor))
            using (Pen majorPen = new Pen(MajorTickColor))
            using (Pen minorPen = new Pen(MinorTickColor))
            {
                // RENDER MAJOR TICKS AND TICK LABELS

                if (MajorTickPositions?.Length != MajorTickLabels?.Length)
                    throw new InvalidOperationException("major tick length must equal tick label length");

                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                for (int i = 0; i < MajorTickPositions?.Length; i++)
                {
                    PointF pt1, pt2;
                    if (Edge == Edge.Bottom)
                    {
                        float xPx = (float)settings.PixelX(MajorTickPositions[i]);
                        pt1 = new PointF(xPx, settings.DataB);
                        pt2 = new PointF(xPx, settings.DataB + MajorTickLength);
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        gfx.DrawString(MajorTickLabels[i], font, fontBrush, pt2, sf);
                    }
                    else if (Edge == Edge.Left)
                    {
                        float yPx = (float)settings.PixelY(MajorTickPositions[i]);
                        pt1 = new PointF(settings.DataL, yPx);
                        pt2 = new PointF(settings.DataL - MajorTickLength, yPx);
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
                        gfx.DrawString(MajorTickLabels[i], font, fontBrush, pt2, sf);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    gfx.DrawLine(majorPen, pt1, pt2);
                }

                // RENDER MINOR TICKS
                for (int i = 0; i < MinorTickPositions?.Length; i++)
                {
                    PointF pt1, pt2;
                    if (Edge == Edge.Bottom)
                    {
                        float xPx = (float)settings.PixelX(MinorTickPositions[i]);
                        pt1 = new PointF(xPx, settings.DataB);
                        pt2 = new PointF(xPx, settings.DataB + MinorTickLength);
                    }
                    else if (Edge == Edge.Left)
                    {
                        float yPx = (float)settings.PixelY(MinorTickPositions[i]);
                        pt1 = new PointF(settings.DataL, yPx);
                        pt2 = new PointF(settings.DataL - MinorTickLength, yPx);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    gfx.DrawLine(minorPen, pt1, pt2);
                }
            }
        }
    }
}
