using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScottPlot
{
    public partial class ScottPlotUC : UserControl
    {
        public Plot plt = new Plot();

        public bool useDynamicAA = false;

        private bool dynamicAAFigureLow = true;  // setting for AAFigure at interactions
        private bool dynamicAADataLow = false;   // settings for AAData at interactions
        private bool dynamicAAFigureHigh = true; // setting for AAFigure at Static
        private bool dynamicAADataHigh = true;   // setting for AAData at Static

        private bool currentlyRendering = false;
        private ContextMenuStrip rightClickMenu;

        public ScottPlotUC()
        {
            InitializeComponent();
            plt.Style(ScottPlot.Style.Control);
            pbPlot.MouseWheel += PbPlot_MouseWheel;
            if (Process.GetCurrentProcess().ProcessName == "devenv")
                ScottPlot.Tools.DesignerModeDemoPlot(plt);
            PbPlot_SizeChanged(null, null);
        }

        public void Render(bool skipIfCurrentlyRendering, bool antiAliasFigure, bool antiAliasData)
        {
            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                pbPlot.Image = plt.GetBitmap(antiAliasFigure, antiAliasData);
                if (plt.mouseTracker.IsDraggingSomething())
                    Application.DoEvents();
                currentlyRendering = false;
            }
        }

        public void Render(bool skipIfCurrentlyRendering = false)
        {
            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                pbPlot.Image = plt.GetBitmap();
                if (plt.mouseTracker.IsDraggingSomething())
                    Application.DoEvents();
                currentlyRendering = false;
            }
        }

        private void PbPlot_SizeChanged(object sender, EventArgs e)
        {
            plt.Resize(Width, Height);
            if (useDynamicAA)
                Render(false, dynamicAAFigureHigh, dynamicAADataHigh); // use high settings for Resize
            else
                Render(skipIfCurrentlyRendering: false);
        }

        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseDown(e.Location);
            if (plt.mouseTracker.PlottableUnderCursor(e.Location) != null)
                OnMouseDownOnPlottable(EventArgs.Empty);
        }

        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseMove(e.Location);
            OnMouseMoved(EventArgs.Empty);

            var plottableUnderCursor = plt.mouseTracker.PlottableUnderCursor(e.Location);
            if (plottableUnderCursor != null)
            {
                if (e.Button != MouseButtons.None)
                    OnMouseDragPlottable(EventArgs.Empty);
                if (plottableUnderCursor is PlottableAxLine axLine)
                {
                    if (axLine.vertical == true)
                        pbPlot.Cursor = Cursors.SizeWE;
                    else
                        pbPlot.Cursor = Cursors.SizeNS;
                }
            }
            else
            {
                pbPlot.Cursor = Cursors.Arrow;
            }

            if (e.Button != MouseButtons.None)
            {
                if (useDynamicAA)
                    Render(true, dynamicAAFigureLow, dynamicAADataLow); // use low settings on pan or zoom
                else
                    Render(skipIfCurrentlyRendering: true);
                OnMouseDragged(EventArgs.Empty);
            }
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseUp(e.Location);
            if (useDynamicAA)
                Render(false, dynamicAAFigureHigh, dynamicAADataHigh); // use high settings then mouse up
            else
                Render(skipIfCurrentlyRendering: false);
            if (plt.mouseTracker.PlottableUnderCursor(e.Location) != null)
                OnMouseDropPlottable(EventArgs.Empty);
        }

        private void PbPlot_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                plt.AxisAuto();
                if (useDynamicAA)
                    Render(false, dynamicAAFigureHigh, dynamicAADataHigh); // use high settings on middle button (AutoAxis)
                else
                    Render(skipIfCurrentlyRendering: false);
            }
            else if (e.Button == MouseButtons.Right && plt.mouseTracker.mouseDownStopwatch.ElapsedMilliseconds < 100)
            {
                rightClickMenu = new ContextMenuStrip();
                rightClickMenu.Items.Add("Save Image");

                rightClickMenu.Items.Add("Save Data");
                ToolStripMenuItem saveDataMenuItem = (ToolStripMenuItem)rightClickMenu.Items[rightClickMenu.Items.Count - 1];
                saveDataMenuItem.Enabled = false;
                if (plt.GetPlottables().Count > 0)
                    if ((plt.GetPlottables()[0] is PlottableScatter) || (plt.GetPlottables()[0] is PlottableSignal))
                        saveDataMenuItem.Enabled = true;

                rightClickMenu.Items.Add("Auto-Axis");
                rightClickMenu.Items.Add("Clear");
                rightClickMenu.Items.Add(new ToolStripSeparator());
                rightClickMenu.Items.Add("Help");

                ToolStripMenuItem helpMenu = (ToolStripMenuItem)rightClickMenu.Items[rightClickMenu.Items.Count - 1];
                helpMenu.DropDownItems.Add("left-click-drag to pan");
                helpMenu.DropDownItems.Add("right-click-drag to zoom");
                helpMenu.DropDownItems.Add("middle-click for auto-axis");
                helpMenu.DropDownItems.Add("double-click to toggle benchmark");
                helpMenu.DropDownItems.Add(new ToolStripSeparator());
                helpMenu.DropDownItems.Add($"About ScottPlot {ScottPlot.Tools.GetVersionString()}");
                helpMenu.DropDownItems[0].Enabled = false;
                helpMenu.DropDownItems[1].Enabled = false;
                helpMenu.DropDownItems[2].Enabled = false;
                helpMenu.DropDownItems[3].Enabled = false;

                rightClickMenu.Show(pbPlot, PointToClient(Cursor.Position));
                rightClickMenu.ItemClicked += new ToolStripItemClickedEventHandler(RightClickMenuItemClicked);
                helpMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(RightClickMenuItemClicked);
            }
        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            plt.Benchmark(toggle: true);
            if (useDynamicAA)
                Render(false, dynamicAAFigureHigh, dynamicAADataHigh); // use high settings on double click?, is this render before context menu
            else
                Render(skipIfCurrentlyRendering: false);
        }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            double zoomAmount = 0.15;
            PointF zoomCenter = plt.CoordinateFromPixel(e.Location);
            if (e.Delta > 0)
                plt.AxisZoom(1 + zoomAmount, 1 + zoomAmount, zoomCenter);
            else
                plt.AxisZoom(1 - zoomAmount, 1 - zoomAmount, zoomCenter);
            // lost in performance, must call high only on last MouseWhell, but no way to check
            // may be implement on timer, because typicaly mouse wheel event called close in time
            if (useDynamicAA)
                Render(false, dynamicAAFigureHigh, dynamicAADataHigh); 
            else
                Render(skipIfCurrentlyRendering: false);
        }

        private void RightClickMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            rightClickMenu.Hide();
            Tools.RightClickMenuItemClicked(e.ClickedItem, rightClickMenu, plt);
            if (useDynamicAA)
                Render(false, dynamicAAFigureHigh, dynamicAADataHigh); // use high settings
            else
                Render(skipIfCurrentlyRendering: false);
        }

        public event EventHandler MouseDownOnPlottable;
        public event EventHandler MouseDragPlottable;
        public event EventHandler MouseMoved;
        public event EventHandler MouseDragged;
        public event EventHandler MouseDropPlottable;
        protected virtual void OnMouseDownOnPlottable(EventArgs e) { MouseDownOnPlottable?.Invoke(this, e); }
        protected virtual void OnMouseDragPlottable(EventArgs e) { MouseDragPlottable?.Invoke(this, e); }
        protected virtual void OnMouseMoved(EventArgs e) { MouseMoved?.Invoke(this, e); }
        protected virtual void OnMouseDragged(EventArgs e) { MouseDragged?.Invoke(this, e); }
        protected virtual void OnMouseDropPlottable(EventArgs e) { MouseDropPlottable?.Invoke(this, e); }
    }
}
