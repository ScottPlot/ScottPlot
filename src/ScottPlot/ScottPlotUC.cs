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

        public void Render(bool resizeToo = false)
        {
            if (resizeToo)
                plt.settings.Resize(pb.Width, pb.Height);
            pb.Image = plt.figure.GetBitmap();
            Application.DoEvents();
        }

        #region mouse tracking for pan and zoom

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            plt.settings.mouse.Down();
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            plt.settings.mouse.Up();
            Render();
        }

        private bool currentlyProcessingMoveEvent = false;
        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
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
            if (e.Button == MouseButtons.Middle)
            {
                plt.settings.AxisFit();
                Render();
            }
        }

        #endregion
    }
}
