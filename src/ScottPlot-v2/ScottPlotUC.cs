using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDev2
{
    public partial class ScottPlotUC : UserControl
    {
        public ScottPlot.Plot plt;

        public ScottPlotUC()
        {
            InitializeComponent();
            Reset();
        }

        public void Reset()
        {
            plt = new ScottPlot.Plot();
        }

        private void pb_Layout(object sender, LayoutEventArgs e)
        {
            try
            {
                Render(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nException:\n{ex.Message}");
                Console.WriteLine($"\n\nTraceback:\n{ex.StackTrace}");
            }
        }

        private void ScottPlotUC_Load(object sender, EventArgs e)
        {
            try
            {
                Render(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nException:\n{ex.Message}");
                Console.WriteLine($"\n\nTraceback:\n{ex.StackTrace}");
            }
        }

        public ScottPlot.Plot plt2;
        public void Render(bool resizeToo = false)
        {
            if (resizeToo)
            {
                plt.settings.Resize(pb.Width, pb.Height);
                if (plt2 != null)
                {
                    plt2.settings.Resize(plt.settings.width, plt.settings.height);
                }
            }

            if (plt2 == null)
            {
                pb.Image = plt.figure.GetBitmap();
            }
            else
            {
                // if plt2 contains a ScottPlot, match its X axis to the user control then overlay it
                plt2.settings.axisX.Set(plt.settings.axisX.x1, plt.settings.axisX.x2);
                Bitmap bmp1 = plt.figure.GetBitmap();
                Bitmap bmp2 = plt2.figure.GetBitmap();
                Bitmap bmpMerged = new Bitmap(bmp1);
                using (Graphics gfx = Graphics.FromImage(bmpMerged))
                    gfx.DrawImage(bmp2, new Rectangle(0, 0, bmp2.Width, bmp2.Height));
                pb.Image = bmpMerged;
            }
            Application.DoEvents();
        }

        #region mouse tracking for pan and zoom

        public bool disableMouse = false;
        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (disableMouse) return;
            plt.settings.mouse.Down();
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            if (disableMouse) return;
            plt.settings.mouse.Up();
            Render();
        }

        private bool currentlyProcessingMoveEvent = false;
        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            if (disableMouse) return;
            if (currentlyProcessingMoveEvent)
            {
                return;
            }
            else
            {
                currentlyProcessingMoveEvent = true;
                if (plt.settings.mouse.MoveRequiresRender())
                    Render();
                currentlyProcessingMoveEvent = false;
            }
        }

        private void pb_MouseClick(object sender, MouseEventArgs e)
        {
            if (disableMouse) return;
            if (e.Button == MouseButtons.Middle)
            {
                plt.settings.AxisFit();
                Render();
            }
        }

        #endregion
    }
}
