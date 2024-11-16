using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ScottPlot.Plottables;
using ScottPlot.WinForms;


namespace ScottPlot
{
    public partial class FormsPlotLegendViewer : Form
    {
        private readonly IPlotControl FormsPlot;
        private readonly Legend Legend;
        private readonly int nLegend;
        private IPlottable? ClickedPlottable;
        private readonly bool LegendWasInitiallyVisible;


        public FormsPlotLegendViewer(IPlotControl formsPlot, string windowTitle = "Detached Legend")
        {
            LegendWasInitiallyVisible = formsPlot.Plot.Legend.IsVisible;

            FormsPlot = formsPlot;
            Legend = FormsPlot.Plot.Legend;// Legend(enable: true, location: null);
            nLegend = Legend.GetItems().Length;
            Text = windowTitle;

            Legend.IsDetached = true;
            Legend.IsVisible = false;

            InitializeComponent();
            this.Visible = true;
            this.Show();
            FormClosed += OnClosed;
            //PictureBoxLegend.MouseClick += PictureBoxLegend_MouseClick;
            RefreshLegendImage();
            ResizeWindowToFitLegendImage();
        }

        public void RefreshLegendImage()
        {
            FormsPlot.Refresh();
            ScottPlot.Image legendImage = FormsPlot.Plot.GetLegendImage();

            var originalLegendOrientation = Legend.Orientation;
            Legend.Orientation = Orientation.Vertical;
            byte[] legendBitmapBytes = legendImage.GetImageBytes(ImageFormat.Bmp);
            MemoryStream ms = new(legendBitmapBytes);
            Bitmap bmp = new(ms);
            PictureBoxLegend.Image = bmp;
            PictureBoxLegend.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            //RemoveHighlightFromAllPlottables();
            Legend.IsDetached = false;
            FormsPlot.Plot.Legend.IsVisible = LegendWasInitiallyVisible;
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

        private void PictureBoxLegend_MouseClick(object? sender, EventArgs ee)
        {
            var e = (MouseEventArgs)ee;

            GetLegendItemUnderMouse(e.Location);

            if (e.Button == MouseButtons.Left)
            {
                // only allow toggling of plottables with a single legend item
                // (blocking things like pie charts)
                if (ClickedPlottable != null)
                {
                    ClickedPlottable.IsVisible = !ClickedPlottable.IsVisible;
                    ClickedPlottable.LegendItems.ToList().ForEach(x => x.IsToggled = !x.IsToggled);
                    RefreshLegendImage();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                OpenRightClickMenu();
            }
        }

        private void PictureBoxLegend_ToggleHighlight(object sender, EventArgs e)
        {
            //if (ClickedPlottable is IHighlightable plottable)
            //{
            //    plottable.IsHighlighted = !plottable.IsHighlighted;
            //    RefreshLegendImage();
            //}
        }

        private void PictureBoxLegend_DeletePlottable(object sender, EventArgs e)
        {
            if (ClickedPlottable != null)
            {
                FormsPlot.Plot.Remove(ClickedPlottable);
                RefreshLegendImage();
            }
        }

        private void GetLegendItemUnderMouse(Point e)
        {
            if (nLegend == 0)
                return;

            // mouse hit logic must go here because Legend doesn't know about image stretching or display scaling
            double legendItemHeight = (double)(PictureBoxLegend.Image.Height / nLegend);
            int clickedindex = (int)Math.Floor(e.Y / legendItemHeight);
            ClickedPlottable = Legend.GetItems()[clickedindex].Parent;
        }

        //private void RemoveHighlightFromAllPlottables()
        //{
        //    //IHighlightable[] highlightables = FormsPlot.Plot.GetPlottables()
        //    //    .Where(x => x is IHighlightable)
        //    //    .Cast<IHighlightable>()
        //    //    .ToArray();

        //    //foreach (IHighlightable highlightable in highlightables)
        //    //{
        //    //    highlightable.IsHighlighted = false;
        //    //}
        //}

        private void OpenRightClickMenu()
        {
            ContextMenuStrip customMenu = new();

            var highlightMenuItem = new ToolStripMenuItem("Highlight", null,
                new EventHandler(PictureBoxLegend_ToggleHighlight));
            //if (ClickedPlottable is IHighlightable h)
            //    highlightMenuItem.Checked = h.IsHighlighted;
            //customMenu.Items.Add(highlightMenuItem);

            customMenu.Items.Add(new ToolStripMenuItem("Delete", null,
                new EventHandler(PictureBoxLegend_DeletePlottable)));

            if (ClickedPlottable is IHasLine)
            {
                var LineMenu = new ToolStripMenuItem("Line");
                var LinePatternMenu = new ToolStripMenuItem("Pattern");
                foreach (LinePattern lp in LinePattern.GetAllPatterns())
                {
                    LinePatternMenu.DropDownItems.Add(new ToolStripMenuItem(lp.Name, null, new EventHandler(ChangeLinePattern)));
                }
                LineMenu.DropDownItems.Add(LinePatternMenu);
                var LineWidthMenu = new ToolStripMenuItem("Thickness");
                foreach (string lw in new string[] { "much thinner", "thinner", "thicker", "much thicker" })
                {
                    LineWidthMenu.DropDownItems.Add(new ToolStripMenuItem(lw, null, new EventHandler(ChangeLineWidth)));
                }
                LineMenu.DropDownItems.Add(LineWidthMenu);
                var LineColorMenu = new ToolStripMenuItem("Color", null, new EventHandler(ChangeLineColor));
                LineMenu.DropDownItems.Add(LineColorMenu);
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
                var MarkerColorMenu = new ToolStripMenuItem("Marker Color", null, new EventHandler(ChangeMarkerColor));
                MarkerMenu.DropDownItems.Add(MarkerColorMenu);
                var MarkerLineColorMenu = new ToolStripMenuItem("Marker Line Color", null, new EventHandler(ChangeMarkerLineColor));
                MarkerMenu.DropDownItems.Add(MarkerLineColorMenu);
                customMenu.Items.Add(MarkerMenu);
            }

            customMenu.Show(System.Windows.Forms.Cursor.Position);
        }

        private void ChangeLinePattern(object sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }
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
                RefreshLegendImage();
            }
        }

        private void ChangeLineColor(object sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasLine line)
            {
                if (ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    line.LineColor = ScottPlot.Color.FromColor(ColorDialog.Color);
                }
                RefreshLegendImage();
            }
        }

        private void ChangeLineWidth(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasLine line)
            {
                line.LineWidth *= lwcoeff;
                RefreshLegendImage();
            }
        }

        private void ChangeMarkerLineWidth(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasMarker marker)
            {
                marker.MarkerLineWidth *= lwcoeff;
                RefreshLegendImage();
            }
        }

        private void ChangeMarkerShape(object? sender, EventArgs e)
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
                    RefreshLegendImage();
                }
            }
        }

        private void ChangeMarkerSize(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasMarker marker)
            {
                float coeff = MarkerSizeCoefficient(((ToolStripMenuItem)sender).Text);
                marker.MarkerSize *= coeff;
                RefreshLegendImage();
            }
        }

        private void ChangeMarkerColor(object sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasMarker marker)
            {
                if (ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    marker.MarkerColor = ScottPlot.Color.FromColor(ColorDialog.Color);
                }
                RefreshLegendImage();
            }
        }

        private void ChangeMarkerLineColor(object sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }
            if (ClickedPlottable is IHasMarker marker)
            {
                if (ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    marker.MarkerLineColor = ScottPlot.Color.FromColor(ColorDialog.Color);
                }
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
