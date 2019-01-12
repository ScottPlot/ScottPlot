using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    /*
     * This class helps track mouse movements and use them to adjust existing axis  objects.
     *   left-click-drag pan
     *   right-click-drag zoom
     *   TODO: scroll-wheel zoom
     *   TODO: middle-click auscale
     *   TODO: right-click context menu
     *   TODO: double-click benchmarking
     */
    public class MouseTracker
    {
        Point posDown;
        Point posUp;
        Point posMove;
        Point dragDelta;
        Point dropDelta;
        bool panning = false;
        bool zooming = false;
        private bool moveRequiresRender { get { return panning || zooming; } }
        Axis axisX;
        Axis axisY;
        Axis axisXatMouseDown;
        Axis axisYatMouseDown;

        public MouseTracker(ref Axis axisX, ref Axis axisY)
        {
            this.axisX = axisX;
            this.axisY = axisY;
        }

        public void Down()
        {

            // create a copy of the axes the way they were before we start moving or zooming
            axisXatMouseDown = new Axis(axisX);
            axisYatMouseDown = new Axis(axisY);

            posDown = new Point(Cursor.Position.X, Cursor.Position.Y);
            if (Control.MouseButtons == MouseButtons.Left)
                panning = true;
            else if (Control.MouseButtons == MouseButtons.Right)
                zooming = true;
        }

        public void Up()
        {
            posUp = new Point(Cursor.Position.X, Cursor.Position.Y);
            dropDelta = dragDelta;
            if (panning)
                panning = false;
            if (zooming)
                zooming = false;
        }

        public void Move()
        {
            posMove = new Point(Cursor.Position.X, Cursor.Position.Y);
            if (panning || zooming)
                Drag();
        }

        public bool MoveRequiresRender()
        {
            Move();
            return moveRequiresRender;
        }

        private void Drag()
        {
            dragDelta = new Point(posMove.X - posDown.X, posMove.Y - posDown.Y);
            if (panning)
            {
                axisX.Set(axisXatMouseDown.x1, axisXatMouseDown.x2);
                axisY.Set(axisYatMouseDown.x1, axisYatMouseDown.x2);
                axisX.PanPx(-dragDelta.X);
                axisY.PanPx(dragDelta.Y);
            }
            else if (zooming)
            {
                axisX.Set(axisXatMouseDown.x1, axisXatMouseDown.x2);
                axisY.Set(axisYatMouseDown.x1, axisYatMouseDown.x2);
                axisX.ZoomPx(dragDelta.X);
                axisY.ZoomPx(-dragDelta.Y);
            }
        }
    }
}
