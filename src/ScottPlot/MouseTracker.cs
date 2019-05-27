using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public class MouseTracker
    {
        public Stopwatch mouseDownStopwatch = new Stopwatch();

        private readonly Settings settings;
        public MouseTracker(Settings settings)
        {
            this.settings = settings;
        }

        public void MouseDown()
        {
            mouseDownStopwatch.Restart();
            if (Control.MouseButtons == MouseButtons.Left)
                settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, panning: true);
            else if (Control.MouseButtons == MouseButtons.Right)
                settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, zooming: true);
        }

        public void MouseMove()
        {
            settings.MouseMove(Cursor.Position.X, Cursor.Position.Y);
        }

        public void MouseUp()
        {
            settings.MouseMove(Cursor.Position.X, Cursor.Position.Y);
            settings.MouseUp();
        }
    }
}
