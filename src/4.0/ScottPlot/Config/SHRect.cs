using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class SHRect
    {
        public int left { get; private set; } // origin
        public int top { get; private set; } // origin
        public int right { get; private set; }
        public int bottom { get; private set; }

        public int Width { get { return right - left; } }
        public int Height { get { return bottom - top; } }
        public Point Location { get { return new Point(left, top); } }
        public Point Center { get { return new Point(CenterX, CenterY); } }
        public int CenterX { get { return (int)((right + left) / 2); } }
        public int CenterY { get { return (int)((bottom + top) / 2); } }
        public Size Size { get { return new Size(Width, Height); } }
        public Rectangle Rectangle { get { return new Rectangle(Location, Size); } }
        public bool isValid { get { return (left < right) && (top < bottom); } }

        public SHRect(int left, int right, int bottom, int top)
        {
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.top = top;
        }

        public SHRect(SHRect rect)
        {
            left = rect.left;
            right = rect.right;
            bottom = rect.bottom;
            top = rect.top;
        }

        public override string ToString()
        {
            string validString = (isValid) ? "valid" : "invalid";
            return $"{validString} rectangle [{Width}, {Height}] at ({left}, {top})";
        }

        public void ShrinkTo(int? left = null, int? right = null, int? bottom = null, int? top = null)
        {
            if (top != null)
                this.bottom = this.top + (int)top;
            if (bottom != null)
                this.top = this.bottom - (int)bottom;
            if (left != null)
                this.right = this.left + (int)left;
            if (right != null)
                this.left = this.right - (int)right;
        }

        public void ShrinkBy(int? left = null, int? right = null, int? bottom = null, int? top = null)
        {
            if (top != null)
                this.top += (int)top;
            if (bottom != null)
                this.bottom -= (int)bottom;
            if (left != null)
                this.left += (int)left;
            if (right != null)
                this.right -= (int)right;
        }

        public void Shift(int rightward = 0, int downward = 0)
        {
            left += rightward;
            right += rightward;
            top += downward;
            bottom += downward;
        }

        public void MatchVert(SHRect source)
        {
            top = source.top;
            bottom = source.bottom;
        }

        public void MatchHoriz(SHRect source)
        {
            left = source.left;
            right = source.right;
        }
    }
}
