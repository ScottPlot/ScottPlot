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
using SkiaSharp;
using OpenTK.Graphics.ES30;

namespace ScottPlotSkia
{
    public partial class FormsPlotSkia : FormsPlot
    {
        private bool designMode;
        SKColorType colorType = SKColorType.Rgba8888;
        SkiaBackend skiaBackend;
        GRBackendRenderTarget renderTarget;
        SKSurface surface;
        GRContext context;

        private void OnDispose(Object sender, EventArgs e)
        {
            renderTarget?.Dispose();
            surface?.Dispose();
            context?.Dispose();
        }
        public FormsPlotSkia()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            InitializeComponent();
            
            Disposed += OnDispose;
            
            glControl1.MouseClick += new MouseEventHandler(this.PbPlot_MouseClick);
            glControl1.MouseDoubleClick += new MouseEventHandler(this.PbPlot_MouseDoubleClick);
            glControl1.MouseDown += new MouseEventHandler(this.PbPlot_MouseDown);
            glControl1.MouseMove += new MouseEventHandler(this.PbPlot_MouseMove);
            glControl1.MouseUp += new MouseEventHandler(this.PbPlot_MouseUp);
            glControl1.MouseWheel += PbPlot_MouseWheel;

            skiaBackend = new SkiaBackend();
            plt = new Plot(backendData: skiaBackend);
            pbPlot.SizeChanged += (o, e) =>
            {
                glControl1.SetBounds(plt.GetSettings().dataOrigin.X, plt.GetSettings().dataOrigin.Y, plt.GetSettings().dataSize.Width, plt.GetSettings().dataSize.Height);
            };
            PbPlot_SizeChanged(null, null);
        }

        public override void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false)
        {
            if (lastInteractionTimer.Enabled)
                lastInteractionTimer.Stop();
            
            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                
                currentlyRendering = true;
                skiaBackend?.SetAntiAlias(!lowQuality);
                glControl1?.Invalidate();
                if (plt.mouseTracker.IsDraggingSomething())
                    Application.DoEvents();
                currentlyRendering = false;
            }
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            Control senderControl = (Control)sender;
            // create the contexts if not done already
            if (context == null)
            {
                var glInterface = GRGlInterface.CreateNativeGlInterface();
                context = GRContext.Create(GRBackend.OpenGL, glInterface);
            }

            if (renderTarget == null || surface == null || renderTarget.Width != senderControl.Width || renderTarget.Height != senderControl.Height)
            {
                renderTarget?.Dispose();


                GL.GetInteger(GetPName.FramebufferBinding, out var framebuffer);
                GL.GetInteger(GetPName.StencilBits, out var stencil);
                var glInfo = new GRGlFramebufferInfo((uint)framebuffer, colorType.ToGlSizedFormat());
                renderTarget = new GRBackendRenderTarget(senderControl.Width, senderControl.Height, context.GetMaxSurfaceSampleCount(colorType), stencil, glInfo);
               
               
                surface?.Dispose();
                surface = SKSurface.Create(context, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            }


            skiaBackend.canvas = surface.Canvas;
            pbPlot.Image = plt.GetBitmap();

            surface.Canvas.Flush();
            glControl1.SwapBuffers();
        }
    }
}
