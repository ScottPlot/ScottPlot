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
using OpenTK.Graphics;

namespace ScottPlotSkia
{
    public partial class FormsPlotSkia : FormsPlot
    {
        SKColorType colorType = SKColorType.Rgba8888;
        SkiaBackend figureBackend;
        SkiaBackend skiaBackend;
        SkiaBackend legendBackend;
        GRBackendRenderTarget renderTarget;
        SKSurface surface;
        GRContext context;
        OpenTK.GLControl glControl1;

        private void OnDispose(Object sender, EventArgs e)
        {
            renderTarget?.Dispose();
            surface?.Dispose();
            context?.Dispose();
        }
        public FormsPlotSkia()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                Tools.DesignerModeDemoPlot(plt);
                plt.Style(ScottPlot.Style.Control);
                base.Render();
                return;
            }
            Disposed += OnDispose;

            glControl1 = new OpenTK.GLControl(new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8, 4));
            glControl1.BackColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.glControl1.Location = new Point(77, 35);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new Size(403, 293);
            this.glControl1.Dock = DockStyle.Fill;
            this.glControl1.TabIndex = 1;

            glControl1.VSync = false;
            glControl1.Paint += new PaintEventHandler(this.GlControl1_Paint);

            glControl1.MouseClick += new MouseEventHandler(this.glControl_MouseClick);
            glControl1.MouseDoubleClick += new MouseEventHandler(this.glControl_MouseDoubleClick);
            glControl1.MouseDown += new MouseEventHandler(this.glControl_MouseDown);
            glControl1.MouseMove += new MouseEventHandler(this.glControl_MouseMove);
            glControl1.MouseUp += new MouseEventHandler(this.PbPlot_MouseUp);
            glControl1.MouseWheel += PbPlot_MouseWheel;
            this.Controls.Add(this.glControl1);
            glControl1.BringToFront();

            skiaBackend = new SkiaBackend();
            legendBackend = new SkiaBackend();
            figureBackend = new SkiaBackend();
            plt = new Plot(backendFigure: figureBackend, backendData: skiaBackend, backendLegend: legendBackend);
            plt.Style(ScottPlot.Style.Control);
            PbPlot_SizeChanged(null, null);
        }

        public void glControl_MouseClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs pbplotE = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
            PbPlot_MouseClick(sender, pbplotE);
        }

        public void glControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs pbplotE = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
            PbPlot_MouseDoubleClick(sender, pbplotE);
        }

        public void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs pbplotE = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
            PbPlot_MouseDown(sender, pbplotE);
        }

        public void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs pbplotE = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
            PbPlot_MouseMove(sender, pbplotE);
        }

        public void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs pbplotE = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
            PbPlot_MouseUp(sender, pbplotE);
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
            figureBackend.canvas = surface.Canvas;
            skiaBackend.canvas = surface.Canvas;
            legendBackend.canvas = surface.Canvas;
            pbPlot.Image = plt.GetBitmap(true, !skiaBackend.AA);

            surface.Canvas.Flush();
            glControl1.SwapBuffers();
            //if (glControl1.Size != plt.GetSettings().dataSize || glControl1.Location != plt.GetSettings().dataOrigin)
            //{
            //    glControl1.SetBounds(plt.GetSettings().dataOrigin.X, plt.GetSettings().dataOrigin.Y, plt.GetSettings().dataSize.Width, plt.GetSettings().dataSize.Height);
            //    glControl1.Invalidate();
            //}
        }
    }
}
