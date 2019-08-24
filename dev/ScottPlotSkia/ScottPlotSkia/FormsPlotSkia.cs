using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;
using OpenGL;
using System.Diagnostics;

namespace ScottPlotSkia
{
    public partial class FormsPlotSkia : FormsPlot
    {
        DeviceContext device;
        INativePBuffer pbuff;
        IntPtr ctx;

        private void OnDispose(object sender, EventArgs e)
        {
            (plt.GetSettings() as SettingsSkia).surface?.Dispose();
            (plt.GetSettings() as SettingsSkia).context?.Dispose();
            device.DeleteContext(ctx);
            device?.Dispose();
            pbuff?.Dispose();
        }
        public FormsPlotSkia()
        {
            if (Process.GetCurrentProcess().ProcessName == "devenv")
            {
                return;
            }
                InitializeComponent();

            OpenGL.Egl.IsRequired = true;
            string version1 = Gl.GetString(StringName.Version);
            pbuff = DeviceContext.CreatePBuffer(new DevicePixelFormat(24), 10000, 10000);
            device = DeviceContext.Create(pbuff);
            ctx = device.CreateContext(IntPtr.Zero);
            if (device.MakeCurrent(ctx) == false)
                throw new InvalidOperationException();
            // ForDebug
            var t = OpenGL.Egl.IsAvailable;
            string version = Gl.GetString(StringName.Version);

            this.Disposed += OnDispose;

            Renderer.DataBackground = RendererSkia.DataBackgroundSkia;
            Renderer.DataGrid = RendererSkia.DataGridSkia;
            Renderer.PlaceDataOntoFigure = RendererSkia.PlaceDataOntoFigureSkia;

            plt = new PlotSkia();
            SetupMenu();
            SetupTimers();
            plt.Style(ScottPlot.Style.Control);
            pbPlot.MouseWheel += PbPlot_MouseWheel;
            if (Process.GetCurrentProcess().ProcessName == "devenv")
            {
                
                Tools.DesignerModeDemoPlot(plt);
                plt.Title("ScottPlotSkia User Control");
            }
            PbPlot_SizeChanged(null, null);
        }
    }
}
