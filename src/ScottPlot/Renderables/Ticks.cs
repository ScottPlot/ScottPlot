using ScottPlot.Config;
using ScottPlot.Experimental;
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
        public float FontSize = 10;
        public string FontName = Fonts.GetDefaultFontName();

        public void Render(Bitmap bmp, FigureInfo fig)
        {
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (Font font = new Font(FontName, FontSize))
            using (StringFormat sf = new StringFormat())
            using (Brush fontBrush = new SolidBrush(FontColor))
            using (Pen majorPen = new Pen(MajorTickColor))
            using (Pen minorPen = new Pen(MinorTickColor))
            {
                // RENDER MAJOR TICKS AND TICK LABELS

                if (MajorTickPositions.Length != MajorTickLabels.Length)
                    throw new InvalidOperationException("major tick length must equal tick label length");

                for (int i = 0; i < MajorTickPositions.Length; i++)
                {
                    PointF pt1, pt2;
                    if (Edge == Edge.Bottom)
                    {
                        float xPx = (float)fig.PixelX(MajorTickPositions[i]);
                        pt1 = new PointF(xPx, fig.DataB);
                        pt2 = new PointF(xPx, fig.DataB + MajorTickLength);
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        gfx.DrawString(MajorTickLabels[i], font, fontBrush, pt2, sf);
                    }
                    else if (Edge == Edge.Left)
                    {
                        float yPx = (float)fig.PixelY(MajorTickPositions[i]);
                        pt1 = new PointF(fig.DataL, yPx);
                        pt2 = new PointF(fig.DataL - MajorTickLength, yPx);
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
                for (int i = 0; i < MinorTickPositions.Length; i++)
                {
                    PointF pt1, pt2;
                    if (Edge == Edge.Bottom)
                    {
                        float xPx = (float)fig.PixelX(MinorTickPositions[i]);
                        pt1 = new PointF(xPx, fig.DataB);
                        pt2 = new PointF(xPx, fig.DataB + MinorTickLength);
                    }
                    else if (Edge == Edge.Left)
                    {
                        float yPx = (float)fig.PixelY(MinorTickPositions[i]);
                        pt1 = new PointF(fig.DataL, yPx);
                        pt2 = new PointF(fig.DataL - MinorTickLength, yPx);
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
