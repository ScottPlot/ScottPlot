using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    /// <summary>
    /// Everything related to drawing on bitmaps. 
    /// Data, axis, and figure labels.
    /// Fonts, pens, colors, and all that.
    /// </summary>
    public class Draw
    {
        // load-up an axis object to keep track of the point/pixel mesh
        public Axis Axis = new Axis();

        // bitmap and graphics buffers
        public Bitmap bmpFigure, bmpData;
        public Graphics gfxFigure, gfxData;

        // details about the figure size, etc
        public int figureWidth, figureHeight;
        public int figurePadLeft = 40;
        public int figurePadRight = 10;
        public int figurePadTop = 10;
        public int figurePadBottom = 20;

        // styles
        public string bgFigure = "control";
        public string bgData = "w";

        /// <summary>
        /// combine the axis bitmap with the data bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap Render()
        {
            gfxFigure.DrawImage(bmpData, figurePadLeft, figurePadTop);
            return bmpFigure;
        }

        /// <summary>
        /// re-create the figure from scratch.
        /// re-create bitmap buffers (with new size).
        /// re-draw axis.
        /// clear data.
        /// </summary>
        public void Init(int width, int height)
        {

            // assign our new dimensions
            figureWidth = width;
            figureHeight = height;

            // figure out the data dimensions and init axis (calculates unit/pixel conversions)
            int dataWidth = width - figurePadLeft - figurePadRight;
            int dataHeight = height - figurePadTop - figurePadBottom;
            Axis.Init(dataWidth, dataHeight);

            // redraw the axis (with new ticks) and clear data
            InitAxis();
            InitData();
        }

        /// <summary>
        /// Draw a blank figure with an axis and labels.
        /// This stock figure will be reused (untlil a resize event), where the
        /// data graph will be pasted over this figure upon each render event.
        /// </summary>
        public void InitAxis()
        {

            // todo: only make new bitmap if size changed
            bmpFigure = new Bitmap(figureWidth, figureHeight);
            gfxFigure = Graphics.FromImage(bmpFigure);

            // prepare the fonts and colors
            int tickSize = 3;
            Pen tickPen = new Pen(Color.Black, 1);
            Pen borderPen = new Pen(Color.Black);
            int fontSize = 8;
            Font tickFont = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Regular);
            Brush tickFontBrush = new SolidBrush(Color.Black);

            // so much work to prepare font alignment
            StringFormat formatX = new StringFormat();
            formatX.LineAlignment = StringAlignment.Near;
            formatX.Alignment = StringAlignment.Center;
            StringFormat formatY = new StringFormat();
            formatY.LineAlignment = StringAlignment.Center;
            formatY.Alignment = StringAlignment.Far;

            // clear the figure graphics to start fresh
            gfxFigure.Clear(ColorCode(bgFigure));

            // set the graphics mode to high quality
            gfxFigure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // make a frame around the data area
            gfxFigure.DrawRectangle(borderPen, 
                new Rectangle(figurePadLeft - 1, figurePadTop - 1,
                              Axis.dataWidth+1, Axis.dataHeight +1));

            // draw axis tick lines (vertical axis)
            int pxTickX1 = figurePadLeft-1;
            int pxTickX2 = pxTickX1- tickSize;
            double[] ticksY = Axis.TickGen(Axis.Y1, Axis.Y2, Axis.dataHeight, Axis.dataHeight / 50);
            for (int i=0; i< ticksY.Length; i++)
            {
                int y = (int)((Axis.dataHeight - (ticksY[i] - Axis.Y1) * Axis.pxPerUnitY) + figurePadTop);
                string s = Axis.TickString(ticksY[i], Axis.Y2 - Axis.Y1);
                gfxFigure.DrawLine(tickPen, new Point(pxTickX1, y), new Point(pxTickX2, y));
                gfxFigure.DrawString(s, tickFont, tickFontBrush, 
                    new Point(figurePadLeft-tickSize-2, y+1), formatY);
            }

            // draw axis tick lines (horizontal axis)
            int pxTickY1 = figureHeight-figurePadBottom;
            int pxTickY2 = pxTickY1+ tickSize;
            double[] ticksX = Axis.TickGen(Axis.X1, Axis.X2, Axis.dataWidth, Axis.dataWidth / 100);
            for (int i = 0; i < ticksX.Length; i++)
            {
                int x = (int)((ticksX[i] - Axis.X1) * Axis.pxPerUnitX) + figurePadLeft;
                string s = Axis.TickString(ticksX[i], Axis.X2 - Axis.X1);
                gfxFigure.DrawLine(tickPen, new Point(x, pxTickY1), new Point(x, pxTickY2));
                gfxFigure.DrawString(s, tickFont, tickFontBrush, 
                    new Point(x, figurePadTop + Axis.dataHeight + tickSize + 3), formatX);
            }
        }

        /// <summary>
        /// create a clean-slate data bitmap
        /// </summary>
        public void InitData()
        {
            // todo: only make new bitmap if size changed
            bmpData = new Bitmap(figureWidth-figurePadLeft-figurePadRight, figureHeight-figurePadTop-figurePadBottom);
            gfxData = Graphics.FromImage(bmpData);

            // clear the data graphics to start fresh
            gfxData.Clear(ColorCode(bgData));
        }

        /// <summary>
        /// paint a grid on the data canvas
        /// </summary>
        public void Grid()
        {
            Pen gridPen = new Pen(Color.LightGray, 1);
            double[] ticksY = Axis.TickGen(Axis.Y1, Axis.Y2, Axis.dataHeight, Axis.dataHeight / 50);
            double[] ticksX = Axis.TickGen(Axis.X1, Axis.X2, Axis.dataWidth, Axis.dataWidth / 100);
            for (int i=0; i<ticksY.Length; i++)
            {
                int y = (int)((ticksY[i] - Axis.Y1) * Axis.pxPerUnitY);
                y = Axis.dataHeight - y;
                gfxData.DrawLine(gridPen, new Point(0, y), new Point(Axis.dataWidth, y));
            }
            for (int i = 0; i < ticksX.Length; i++)
            {
                int x = (int)((ticksX[i] - Axis.X1) * Axis.pxPerUnitX);
                gfxData.DrawLine(gridPen, new Point(x, 0), new Point(x, Axis.dataHeight));
            }
        }

        /// <summary>
        /// given a string colorcode, reutrn that actual color
        /// </summary>
        public Color ColorCode(string code = "b")
        {
            switch (code)
            {
                case "r":
                    return Color.Red;
                case "g":
                    return Color.Green;
                case "b":
                    return Color.Blue;
                case "k":
                    return Color.Black;
                case "w":
                    return Color.White;
                case "control":
                    return SystemColors.Control;
                default:
                    return Color.Black;
            }
        }

    }
}
