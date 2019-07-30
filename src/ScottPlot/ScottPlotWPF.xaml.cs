using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScottPlot
{
    /// <summary>
    /// Interaction logic for ScottPlotWPF.xaml
    /// </summary>
    public partial class ScottPlotWPF : UserControl
    {
        public Plot plt = new Plot();
        private bool currentlyRendering = false;

        public ScottPlotWPF()
        {
            InitializeComponent();
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                UserControlTools.DesignerModeDemoPlot(plt);
            CanvasPlot_SizeChanged(null, null);
        }

        public void Render(bool skipIfCurrentlyRendering = false)
        {
            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                imagePlot.Source = UserControlTools.bmpImageFromBmp(plt.GetBitmap());
                currentlyRendering = false;
            }
        }

        private void CanvasPlot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            plt.Resize((int)canvasPlot.ActualWidth, (int)canvasPlot.ActualHeight);
            Render(skipIfCurrentlyRendering: false);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            plt.mouseTracker.MouseDown(e.GetPosition(this));
            CaptureMouse();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseMove(e.GetPosition(this));
            if ((Mouse.LeftButton == MouseButtonState.Pressed) || (Mouse.RightButton == MouseButtonState.Pressed))
                Render(skipIfCurrentlyRendering: true);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            plt.mouseTracker.MouseUp(e.GetPosition(this));
            Render(skipIfCurrentlyRendering: false);
            ReleaseMouseCapture();
        }
    }
}
