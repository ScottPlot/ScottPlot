using System;
using System.Linq;
using System.Windows.Forms;
using ScottPlot.Plottable;

namespace ScottPlot
{
    public partial class FormsPlotLegendViewer : Form
    {
        private readonly FormsPlot FormsPlot;
        private readonly Renderable.Legend Legend;
        private IPlottable ClickedPlottable;
        private readonly bool LegendWasInitiallyVisible;

        public FormsPlotLegendViewer(FormsPlot formsPlot, string windowTitle = "Detached Legend")
        {
            FormsPlot = formsPlot;
            LegendWasInitiallyVisible = formsPlot.Plot.GetSettings(false).CornerLegend.IsVisible;

            Legend = formsPlot.Plot.Legend(enable: true, location: null);
            Text = windowTitle;

            Legend.IsDetached = true;
            Legend.IsVisible = false;

            InitializeComponent();
            this.FormClosed += OnClosed;
            RefreshLegendImage();
            ResizeWindowToFitLegendImage();
        }

        public void RefreshLegendImage()
        {
            FormsPlot.Refresh();
            Legend.UpdateLegendItems(FormsPlot.Plot, includeHidden: true);

            System.Drawing.Image originalImage = PictureBoxLegend.Image;

            var originalLegendOrientation = Legend.Orientation;
            Legend.Orientation = Orientation.Vertical;
            PictureBoxLegend.Image = Legend.GetBitmap(false);
            Legend.Orientation = originalLegendOrientation;

            if (originalImage is not null)
                originalImage.Dispose();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            RemoveHighlightFromAllPlottables();
            Legend.IsDetached = false;
            FormsPlot.Plot.Legend(enable: LegendWasInitiallyVisible, location: null);
            FormsPlot.Refresh();
        }

        private void ResizeWindowToFitLegendImage()
        {
            var widthMax = PictureBoxLegend.Image.Width + 2 * SystemInformation.VerticalScrollBarWidth;
            var heightMax = PictureBoxLegend.Image.Height + 3 * SystemInformation.HorizontalScrollBarHeight;
            MaximumSize = new(widthMax, heightMax);
            MinimumSize = new(widthMax, Math.Min(500, heightMax));
            Size = MinimumSize;
        }

        private void PictureBoxLegend_MouseClick(object sender, EventArgs e)
        {
            MouseEventArgs ee = (MouseEventArgs)e;

            GetLegendItemUnderMouse(ee.Location);

            if (ee.Button == MouseButtons.Left)
            {
                // only allow toggling of plottables with a single legend item
                // (blocking things like pie charts)
                if (ClickedPlottable.GetLegendItems().Length == 1)
                {
                    ClickedPlottable.IsVisible = !ClickedPlottable.IsVisible;
                    RefreshLegendImage();
                }
            }
            else if (ee.Button == MouseButtons.Right)
            {
                OpenRightClickMenu();
            }
        }

        private void PictureBoxLegend_ToggleHighlight(object sender, EventArgs e)
        {
            if (ClickedPlottable is IHighlightable plottable)
            {
                plottable.IsHighlighted = !plottable.IsHighlighted;
                RefreshLegendImage();
            }
        }

        private void PictureBoxLegend_DeletePlottable(object sender, EventArgs e)
        {
            FormsPlot.Plot.Remove(ClickedPlottable);
            RefreshLegendImage();
        }

        private void GetLegendItemUnderMouse(System.Drawing.Point e)
        {
            // mouse hit logic must go here because Legend doesn't know about image stretching or display scaling
            double legendItemHeight = (double)PictureBoxLegend.Image.Height / Legend.Count;
            int clickedindex = (int)Math.Floor(e.Y / legendItemHeight);
            ClickedPlottable = Legend.GetItems()[clickedindex].Parent;
        }

        private void RemoveHighlightFromAllPlottables()
        {
            IHighlightable[] highlightables = FormsPlot.Plot.GetPlottables()
                .Where(x => x is IHighlightable)
                .Cast<IHighlightable>()
                .ToArray();

            foreach (IHighlightable highlightable in highlightables)
            {
                highlightable.IsHighlighted = false;
            }
        }

        private void OpenRightClickMenu()
        {
            ContextMenuStrip customMenu = new();

            var highlightMenuItem = new ToolStripMenuItem("Highlight", null,
                new EventHandler(PictureBoxLegend_ToggleHighlight));
            if (ClickedPlottable is IHighlightable h)
                highlightMenuItem.Checked = h.IsHighlighted;
            customMenu.Items.Add(highlightMenuItem);

            customMenu.Items.Add(new ToolStripMenuItem("Delete", null,
                new EventHandler(PictureBoxLegend_DeletePlottable)));

            if (ClickedPlottable is IHasLine)
            {
                var LineMenu = new ToolStripMenuItem("Line");
                var LineStyleMenu = new ToolStripMenuItem("Style");
                foreach (LineStyle ls in (LineStyle[])Enum.GetValues(typeof(LineStyle)))
                {
                    LineStyleMenu.DropDownItems.Add(new ToolStripMenuItem(ls.ToString(), null, new EventHandler(ChangeLineStyle)));
                }
                LineMenu.DropDownItems.Add(LineStyleMenu);
                var LineWidthMenu = new ToolStripMenuItem("Thickness");
                foreach (string lw in new string[] { "much thinner", "thinner", "thicker", "much thicker" })
                {
                    LineWidthMenu.DropDownItems.Add(new ToolStripMenuItem(lw, null, new EventHandler(ChangeLineWidth)));
                }
                LineMenu.DropDownItems.Add(LineWidthMenu);
                customMenu.Items.Add(LineMenu);
            }

            if (ClickedPlottable is IHasMarker)
            {
                var MarkerMenu = new ToolStripMenuItem("Marker");
                var MarkerShapeMenu = new ToolStripMenuItem("Shape");
                foreach (MarkerShape ms in (MarkerShape[])Enum.GetValues(typeof(MarkerShape)))
                {
                    MarkerShapeMenu.DropDownItems.Add(new ToolStripMenuItem(ms.ToString(), null, new EventHandler(ChangeMarkerShape)));
                }
                MarkerMenu.DropDownItems.Add(MarkerShapeMenu);
                var MarkerSizeMenu = new ToolStripMenuItem("Size");
                foreach (string ms in new string[] { "much smaller", "smaller", "bigger", "much bigger" })
                {
                    MarkerSizeMenu.DropDownItems.Add(new ToolStripMenuItem(ms.ToString(), null, new EventHandler(ChangeMarkerSize)));
                }
                MarkerMenu.DropDownItems.Add(MarkerSizeMenu);
                customMenu.Items.Add(MarkerMenu);
                var MarkerLineWidthMenu = new ToolStripMenuItem("Thickness");
                foreach (string lw in new string[] { "much thinner", "thinner", "thicker", "much thicker" })
                {
                    MarkerLineWidthMenu.DropDownItems.Add(new ToolStripMenuItem(lw, null, new EventHandler(ChangeMarkerLineWidth)));
                }
                MarkerMenu.DropDownItems.Add(MarkerLineWidthMenu);
                customMenu.Items.Add(MarkerMenu);
            }

            customMenu.Show(System.Windows.Forms.Cursor.Position);
        }

        private void ChangeLineStyle(object sender, EventArgs e)
        {
            string lsstring = ((ToolStripMenuItem)sender).Text;
            if (ClickedPlottable is IHasLine line)
            {
                // TODO: streamline this
                foreach (LineStyle ls in (LineStyle[])Enum.GetValues(typeof(LineStyle)))
                {
                    if (ls.ToString() == lsstring)
                    {
                        line.LineStyle = ls;
                    }
                }
            }
            RefreshLegendImage();
        }

        private void ChangeLineWidth(object sender, EventArgs e)
        {
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasLine line)
            {
                line.LineWidth *= lwcoeff;
            }
            RefreshLegendImage();
        }

        private void ChangeMarkerLineWidth(object sender, EventArgs e)
        {
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasMarker marker)
            {
                marker.MarkerLineWidth *= lwcoeff;
            }
            RefreshLegendImage();
        }

        private void ChangeMarkerShape(object sender, EventArgs e)
        {
            string msstring = ((ToolStripMenuItem)sender).Text;
            if (ClickedPlottable is IHasMarker marker)
            {
                foreach (MarkerShape ms in (MarkerShape[])Enum.GetValues(typeof(MarkerShape)))
                {
                    if (ms.ToString() == msstring)
                    {
                        marker.MarkerShape = ms;
                    }
                }
            }
            RefreshLegendImage();
        }

        private void ChangeMarkerSize(object sender, EventArgs e)
        {
            if (ClickedPlottable is IHasMarker marker)
            {
                float coeff = MarkerSizeCoefficient(((ToolStripMenuItem)sender).Text);
                marker.MarkerSize *= coeff;
                RefreshLegendImage();
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
