using ScottPlot;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace WinForms_Demo.Demos
{
    public partial class DetachedLegend : Form, IDemoWindow
    {
        public string Title => "Detachable Legend";

        public string Description => "Add an option to the right-click menu to display the legend in a pop-up window";

        public DetachedLegend()
        {
            InitializeComponent();

            int count = 20;
            for (int i = 0; i < 20; i++)
            {
                double[] ys = Generate.Sin(100, phase: i / (2.0 * count));
                var sig = formsPlot1.Plot.Add.Signal(ys);
                sig.Color = Colors.Category20[i];
                sig.LineWidth = 2;
                sig.LegendText = $"Line #{i + 1}";
            }

            formsPlot1.Menu?.Add("Detach Legend", LaunchDetachedLegend);
        }

        private void LaunchDetachedLegend(IPlotControl plotControl)
        {
            Form form = new()
            {
                Text = "Detached Legend",
                StartPosition = FormStartPosition.CenterScreen,
            };

            SKControl skControl = new()
            {
                Dock = DockStyle.Fill
            };
            skControl.PaintSurface += (s, e) => { PaintDetachedLegend((SKControl)s, (SKPaintSurfaceEventArgs)e); };
            skControl.MouseClick += (s, e) => {LegendControl_MouseClick((SKControl)s, e); };
            form.Controls.Add(skControl);
            formsPlot1.Plot.Legend.IsVisible = formsPlot1.Plot.Legend.IsVisible && false;
            form.Show();
        }

        private void PaintDetachedLegend(SKControl skControl, SKPaintSurfaceEventArgs e)
        {
            PixelSize size = new(skControl.Width, skControl.Height);
            PixelRect rect = new(Pixel.Zero, size);
            SKCanvas canvas = e.Surface.Canvas;
            formsPlot1.Plot.Legend.Render(canvas, rect, Alignment.UpperLeft, false);
        }

        private void LegendControl_MouseClick(SKControl sender, EventArgs ee)
        {
            var e = (MouseEventArgs)ee;

            var item = GetLegendItemUnderMouse(sender, e.Location);
            var ClickedPlottable = item != null ? item.Plottable: null;

            if (e.Button == MouseButtons.Left)
            {
                if (ClickedPlottable != null)
                {
                    ClickedPlottable.IsVisible = !ClickedPlottable.IsVisible;

                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (ClickedPlottable != null)
                {
                    OpenRightClickMenu(ClickedPlottable);
                }
            }
            formsPlot1.Refresh();
            // TODO : How to trigger legend repaint ?
        }


        private LegendItem? GetLegendItemUnderMouse(SKControl sender, Point e)
        {
            PixelSize size = new(sender.Width, sender.Height);
            LegendItem[] items = formsPlot1.Plot.Legend.GetItems(false);
            LegendLayout layout = formsPlot1.Plot.Legend.GetLayout(size, false);
            if (items.Count() == 0)
                return null;

            // mouse hit logic must go here because Legend doesn't know about image stretching or display scaling
            var itemslayout = Enumerable.Zip(items, layout.LabelRects, layout.SymbolRects);
            foreach (var il in itemslayout)
                {
                var item = il.First;
                var lrect = il.Second;
                var srect = il.Third;
                if (lrect.Contains(e.X, e.Y) || srect.Contains(e.X, e.Y))
                {
                        return item;
                }
            }
            return null;
        }

        private void OpenRightClickMenu(IPlottable ClickedPlottable)
        {
            ContextMenuStrip customMenu = new();

            customMenu.Items.Add(new ToolStripMenuItem("Delete", null, new EventHandler((s,e) => DeletePlottable(ClickedPlottable))));

            if (ClickedPlottable is IHasLine)
            {
                var LineMenu = new ToolStripMenuItem("Line");
                var LinePatternMenu = new ToolStripMenuItem("Pattern");
                foreach (LinePattern lp in LinePattern.GetAllPatterns())
                {
                    LinePatternMenu.DropDownItems.Add(new ToolStripMenuItem(lp.Name, null, new EventHandler((s, e) => ChangeLinePattern(s, ClickedPlottable))));
                }
                LineMenu.DropDownItems.Add(LinePatternMenu);
                var LineWidthMenu = new ToolStripMenuItem("Thickness");
                foreach (string lw in new string[] { "much thinner", "thinner", "thicker", "much thicker" })
                {
                    LineWidthMenu.DropDownItems.Add(new ToolStripMenuItem(lw, null, new EventHandler((s, e) => ChangeLineWidth(s, ClickedPlottable))));
                }
                LineMenu.DropDownItems.Add(LineWidthMenu);
                var LineColorMenu = new ToolStripMenuItem("Color", null, new EventHandler((s, e) => ChangeLineColor(s, ClickedPlottable)));
                LineMenu.DropDownItems.Add(LineColorMenu);
                customMenu.Items.Add(LineMenu);
            }

            if (ClickedPlottable is IHasMarker)
            {
                var MarkerMenu = new ToolStripMenuItem("Marker");
                var MarkerShapeMenu = new ToolStripMenuItem("Shape");
                foreach (MarkerShape ms in (MarkerShape[])Enum.GetValues(typeof(MarkerShape)))
                {
                    MarkerShapeMenu.DropDownItems.Add(new ToolStripMenuItem(ms.ToString(), null, new EventHandler((s, e) => ChangeMarkerShape(s, ClickedPlottable))));
                }
                MarkerMenu.DropDownItems.Add(MarkerShapeMenu);
                var MarkerSizeMenu = new ToolStripMenuItem("Size");
                foreach (string ms in new string[] { "much smaller", "smaller", "bigger", "much bigger" })
                {
                    MarkerSizeMenu.DropDownItems.Add(new ToolStripMenuItem(ms.ToString(), null, new EventHandler((s, e) => ChangeMarkerSize(s, ClickedPlottable))));
                }
                MarkerMenu.DropDownItems.Add(MarkerSizeMenu);
                customMenu.Items.Add(MarkerMenu);
                var MarkerLineWidthMenu = new ToolStripMenuItem("Thickness");
                foreach (string lw in new string[] { "much thinner", "thinner", "thicker", "much thicker" })
                {
                    MarkerLineWidthMenu.DropDownItems.Add(new ToolStripMenuItem(lw, null, new EventHandler((s, e) => ChangeMarkerLineWidth(s, ClickedPlottable))));
                }
                MarkerMenu.DropDownItems.Add(MarkerLineWidthMenu);
                var MarkerColorMenu = new ToolStripMenuItem("Marker Color", null, new EventHandler((s, e) => ChangeMarkerColor(s, ClickedPlottable)));
                MarkerMenu.DropDownItems.Add(MarkerColorMenu);
                var MarkerLineColorMenu = new ToolStripMenuItem("Marker Line Color", null, new EventHandler((s, e) => ChangeMarkerLineColor(s, ClickedPlottable)));
                MarkerMenu.DropDownItems.Add(MarkerLineColorMenu);
                customMenu.Items.Add(MarkerMenu);
            }

            customMenu.Show(Cursor.Position);
        }

        private void DeletePlottable(IPlottable ClickedPlottable)
        {
            formsPlot1.Plot.Remove(ClickedPlottable);
            formsPlot1.Refresh();
        }

        private void ChangeLinePattern(object sender, IPlottable ClickedPlottable)
        {
            string lpstring = ((ToolStripMenuItem)sender).Text;
            if (ClickedPlottable is IHasLine line)
            {
                // TODO: streamline this
                foreach (LinePattern lp in LinePattern.GetAllPatterns())
                {
                    if (lp.Name == lpstring)
                    {
                        line.LinePattern = lp;
                    }
                }
            }
        }

        private void ChangeLineColor(object sender, IPlottable ClickedPlottable)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasLine line)
            {
                var colorDialog = new ColorDialog();
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    line.LineColor = ScottPlot.Color.FromColor(colorDialog.Color);
                }
            }
        }

        private void ChangeLineWidth(object sender, IPlottable ClickedPlottable)
        {
            if (sender is null)
            {
                return;
            }
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasLine line)
            {
                line.LineWidth *= lwcoeff;
            }
        }

        private void ChangeMarkerLineWidth(object sender, IPlottable ClickedPlottable)
        {
            if (sender is null)
            {
                return;
            }
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasMarker marker)
            {
                marker.MarkerLineWidth *= lwcoeff;
            }
        }

        private void ChangeMarkerShape(object sender, IPlottable ClickedPlottable)
        {
            if (sender is null)
            {
                return;
            }
            string msstring = ((ToolStripMenuItem)sender).Text;
            if (ClickedPlottable is IHasMarker marker)
            {
                if (msstring != "None")
                {
                    if (marker.MarkerSize == 0)
                    {
                        marker.MarkerSize = 1;
                    }
                    foreach (MarkerShape ms in (MarkerShape[])Enum.GetValues(typeof(MarkerShape)))
                    {
                        if (ms.ToString() == msstring)
                        {
                            marker.MarkerShape = ms;
                        }
                    }
                }
            }
        }

        private void ChangeMarkerSize(object sender, IPlottable ClickedPlottable)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasMarker marker)
            {
                float coeff = MarkerSizeCoefficient(((ToolStripMenuItem)sender).Text);
                marker.MarkerSize *= coeff;
            }
        }

        private void ChangeMarkerColor(object sender, IPlottable ClickedPlottable)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasMarker marker)
            {
                var colorDialog = new ColorDialog();
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    marker.MarkerColor = ScottPlot.Color.FromColor(colorDialog.Color);
                }
            }
        }

        private void ChangeMarkerLineColor(object sender, IPlottable ClickedPlottable)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasMarker marker)
            {
                var colorDialog = new ColorDialog();
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    marker.MarkerLineColor = ScottPlot.Color.FromColor(colorDialog.Color);
                }
            }
        }


private float LineWidthCoefficient(string lwstring)
{
    return lwstring switch
    {
        "much thinner" => 1 / 2f,
        "thinner" => 2 / 3f,
        "thicker" => 3 / 2f,
        "much thicker" => 2,
        _ => throw new NotImplementedException(),
    };
}

private float MarkerSizeCoefficient(string lwstring)
{
    return lwstring switch
    {
        "much smaller" => 1 / 2f,
        "smaller" => 1 / 1.5f,
        "bigger" => 1.5f,
        "much bigger" => 2,
        _ => throw new NotImplementedException(),
    };
    }

}
}
