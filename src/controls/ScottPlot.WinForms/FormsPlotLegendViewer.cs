using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class FormsPlotLegendViewer : Form
    {
        private readonly FormsPlot FormsPlot;
        private readonly Renderable.Legend Legend;

        public FormsPlotLegendViewer(FormsPlot fPlot, string windowTitle = "Detached Legend")
        {
            FormsPlot = fPlot;
            Legend = fPlot.Plot.Legend(null);

            InitializeComponent();
            PictureBoxLegend.Click += PictureBoxLegend_Click;

            UpdateLegendImage();
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

        private void UpdateLegendImage()
        {
            var allPlottables = FormsPlot.Plot.GetPlottables();
            Legend.UpdateLegendItems(allPlottables, true);
            FormsPlot.Refresh();
            PictureBoxLegend.Image = Legend.GetBitmap(false);
        }

        private void SetSizeBasedOnPictureboxImage()
        {
            var widthMax = PictureBoxLegend.Image.Width + 2 * SystemInformation.VerticalScrollBarWidth;
            var heightMax = PictureBoxLegend.Image.Height + 3 * SystemInformation.HorizontalScrollBarHeight;
            MaximumSize = new(widthMax, heightMax);
            MinimumSize = new(widthMax, Math.Min(500, heightMax));
            Size = MinimumSize;
        }

        private void PictureBoxLegend_Click(object sender, EventArgs e)
        {
            MouseEventArgs e2 = (MouseEventArgs)e;

            // TODO: move this logic inside the Legend class if possible
            // public IPlottable Legend.GetPlottableUnderMouse(float xPixel, float yPixel) { }

            // TODO: I don't think this logic is sound...
            // some plottables have multiple items in their legend (e.g., coxcomb)

            // TODO: maybe plottables with multiple legend items should not have this click-to-toggle-visibility feature

            double legendItemHeight = (double)PictureBoxLegend.Image.Height / Legend.Count;
            int clickedItemIndex = (int)Math.Floor(e2.Y / legendItemHeight);
            var clickedPlottable = FormsPlot.Plot.GetPlottables()[clickedItemIndex];

            clickedPlottable.IsVisible = !clickedPlottable.IsVisible;

            UpdateLegendImage();
        }
    }
}
