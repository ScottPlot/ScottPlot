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
        private readonly bool InitialLegendVisibility;
        private IPlottable ClickedPlottable;

        public FormsPlotLegendViewer(FormsPlot fPlot, string windowTitle = "Detached Legend")
        {
            // Get current Legend
            Legend = fPlot.Plot.Legend();
            // Do nothing if Legend is already detached
            if (Legend.IsDetached == true) return;

            // Get current FormsPlot
            FormsPlot = fPlot;
            // Get initial legend visibility, to be restored upon detached legend closing
            InitialLegendVisibility = fPlot.Plot.Legend(null).IsVisible;
            // Set detached status
            fPlot.Plot.Legend().IsDetached = true;
            fPlot.Plot.Legend().IsVisible = false;

            // Initialize FormsPlotLegendViewer and actions
            InitializeComponent();
            // Set action that is triggered by Detached legend closing
            this.FormClosed += RestoreVisibleProperties;

            // Update the legend image based on latest status
            UpdateLegendImage();

            // If the legend is not empty, show it
            if (Legend.HasItems)
            {
                SetSizeBasedOnPictureboxImage();
                Show();
            }
            else
            {
                MessageBox.Show("Current legend has no items", "Detached Legend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        /// <summary>
        /// Updates the detached legend and the underlying FormsPlot
        /// </summary>
        private void UpdateLegendImage()
        {
            FormsPlot.Refresh();
            Legend.UpdateLegendItems(FormsPlot.Plot, includeHidden: true);
            PictureBoxLegend.Image = Legend.GetBitmap(false);
        }

        /// <summary>
        /// Sets size properties of the detached legend in order to have a reasonably sized Form
        /// </summary>
        private void SetSizeBasedOnPictureboxImage()
        {
            var widthMax = PictureBoxLegend.Image.Width + 2 * SystemInformation.VerticalScrollBarWidth;
            var heightMax = PictureBoxLegend.Image.Height + 3 * SystemInformation.HorizontalScrollBarHeight;
            MaximumSize = new(widthMax, heightMax);
            MinimumSize = new(widthMax, Math.Min(500, heightMax));
            Size = MinimumSize;
        }

        /// <summary>
        /// Handles a click on a legend item.
        /// <list type="bullet">
        /// <item>If a left click is performed, toggles the visibility of the plottable corresponding to the legend item under the mouse.</item>
        /// <item>If a right click is performed, opens a context menu.</item>
        /// </list>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBoxLegend_MouseClick(object sender, EventArgs e)
        {
            //Get the plottable 
            GetLegendItemUnderMouse(((MouseEventArgs)e).Location);
            //Act depending on which button was pressed
            switch (((MouseEventArgs)e).Button)
            {
                // Toggle plottable visibility
                case MouseButtons.Left:
                    {
                        // only allow toggling of plottables with a single legend item (blocking things like pie charts)
                        if (ClickedPlottable.GetLegendItems().Length == 1)
                        {
                            ClickedPlottable.IsVisible = !ClickedPlottable.IsVisible;
                            // Update display
                            UpdateLegendImage();
                        }
                        break;
                    }
                // Open special menu
                case MouseButtons.Right:
                    {
                        OpenRightClickMenu();
                        break;
                    }
            }
        }

        /// <summary>
        /// Toggles highlighting of the <c>ClickedPlottable</c>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBoxLegend_ToggleHighlight(object sender, EventArgs e)
        {
            // only allow toggling of plottables with a single legend item (blocking things like pie charts)
            if (ClickedPlottable is IHighlightable)
                ToggleHighlight((IHighlightable)ClickedPlottable);

            // Update display
            UpdateLegendImage();
        }

        /// <summary>
        /// Deletes the <c>ClickedPlottable</c>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBoxLegend_DeletePlottable(object sender, EventArgs e)
        {
            // Delete the plot
            FormsPlot.Plot.Remove(ClickedPlottable);

            // Update display
            UpdateLegendImage();
        }

        /// <summary>
        /// Get the plottable associated with the current cursor position on the detached legend.
        /// The plottable is stored in <c>ClickedPlottable</c> for later access.
        /// The plottable position in the list is stored because there is currently no <c>isHighlighted</c> property of <c>IPlottable</c>
        /// </summary>
        /// <param name="e">Current point under the mouse</param>
        private void GetLegendItemUnderMouse(System.Drawing.Point e)
        {
            // mouse hit logic must go here because Legend doesn't know about image stretching or display scaling
            double legendItemHeight = (double)PictureBoxLegend.Image.Height / Legend.Count;
            int clickedindex = (int)Math.Floor(e.Y / legendItemHeight);
            ClickedPlottable = Legend.GetItems()[clickedindex].Parent;
        }

        /// <summary>
        /// Restores the initial legend visibility, and detached status.
        /// </summary>
        private void RestoreLegendVisibility()
        {
            FormsPlot.Plot.Legend(InitialLegendVisibility);
            FormsPlot.Plot.Legend().IsDetached = false;
            FormsPlot.Refresh();
        }

        /// <summary>
        /// Performs highlight toggle on <c>ScatterPlots</c> and <c>SignalPlots</c>.
        /// A plottable is highlighted by multiplying its line width and marker size by 2.
        /// </summary>
        private void ToggleHighlight(IHighlightable plottable)
        {
            plottable.IsHighlighted = !plottable.IsHighlighted;
        }

        /// <summary>
        /// Loops over current legend items to remove all highlights.
        /// </summary>
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

        /// <summary>
        /// Actions performed when the detached legend is closed.
        /// All higlights are removed, and the old legend is re-displayed if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestoreVisibleProperties(object sender, EventArgs e)
        {
            RemoveHighlightFromAllPlottables();
            RestoreLegendVisibility();
        }

        /// <summary>
        /// Opens a right-click menu with customization options.
        /// Options provided are : 
        /// <list type="bullet">
        /// <item>Highlight Plottable : To highlight the plottable under the cursor</item>
        /// <item>Delete Plottable : To delete the plottable under the cursor</item>
        /// <item>Style : To set line style and marker shape of the plottable under the cursor if possible</item>
        /// </list>
        /// </summary>
        private void OpenRightClickMenu()
        {
            ContextMenuStrip customMenu = new ContextMenuStrip();
            customMenu.Items.Add(new ToolStripMenuItem("Highlight Plottable", null, new EventHandler(PictureBoxLegend_ToggleHighlight)));
            customMenu.Items.Add(new ToolStripMenuItem("Delete Plottable", null, new EventHandler(PictureBoxLegend_DeletePlottable)));

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
            if (ClickedPlottable is IHasLine)
            {
                foreach (LineStyle ls in (LineStyle[])Enum.GetValues(typeof(LineStyle)))
                    if (ls.ToString() == lsstring)
                    {
                        ((IHasLine)ClickedPlottable).LineStyle = ls;
                    }
            }
            UpdateLegendImage();
        }

        private void ChangeLineWidth(object sender, EventArgs e)
        {
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasLine)
            {
                ((IHasLine)ClickedPlottable).LineWidth *= lwcoeff;
            }
            UpdateLegendImage();
        }

        private void ChangeMarkerLineWidth(object sender, EventArgs e)
        {
            var lwcoeff = LineWidthCoefficient(((ToolStripMenuItem)sender).Text);
            if (ClickedPlottable is IHasMarker)
            {
                ((IHasMarker)ClickedPlottable).MarkerLineWidth *= lwcoeff;
            }
            UpdateLegendImage();
        }

        private float LineWidthCoefficient(string lwstring)
        {
            switch (lwstring)
            {
                case "much thinner":
                    return 1 / 2;
                case "thinner":
                    return 2 / 3;
                case "thicker":
                    return 3 / 2;
                case "much thicker":
                    return 2;
            }
            throw new NotImplementedException();
        }

        private void ChangeMarkerShape(object sender, EventArgs e)
        {
            string msstring = ((ToolStripMenuItem)sender).Text;
            if (ClickedPlottable is IHasMarker)
            {
                foreach (MarkerShape ms in (MarkerShape[])Enum.GetValues(typeof(MarkerShape)))
                    if (ms.ToString() == msstring)
                    {
                        ((IHasMarker)ClickedPlottable).MarkerShape = ms;
                    }
            }
            UpdateLegendImage();
        }

        private void ChangeMarkerSize(object sender, EventArgs e)
        {
            string lwstring = ((ToolStripMenuItem)sender).Text;
            if (ClickedPlottable is IHasMarker)
            {
                switch (((ToolStripMenuItem)sender).Text)
                {
                    case "much smaller":

                        ((IHasMarker)ClickedPlottable).MarkerSize /= (float)2;
                        break;

                    case "smaller":

                        ((IHasMarker)ClickedPlottable).MarkerSize /= (float)1.5;
                        break;

                    case "bigger":
                        ((IHasMarker)ClickedPlottable).MarkerSize *= (float)1.5;
                        break;

                    case "much bigger":
                        ((IHasMarker)ClickedPlottable).MarkerSize *= (float)2;
                        break;
                }
            }
            UpdateLegendImage();
        }
    }
}
