using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ScottPlot
{
    class UserControlTools
    {
        public static void DesignerModeDemoPlot(ScottPlot.Plot plt)
        {
            int pointCount = 101;
            double pointSpacing = .01;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount, pointSpacing);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto(0);
            plt.Title("ScottPlot User Control");
            plt.YLabel("Sample Data");
        }

        public static BitmapImage bmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            ((System.Drawing.Bitmap)bmp).Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            return bmpImage;
        }

        public static ContextMenuStrip GetRightClickMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Save Image");
            menu.Items.Add("Auto-Axis");
            menu.Items.Add("Clear");
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Help");
            int i = menu.Items.Count - 1;
            (menu.Items[i] as ToolStripMenuItem).DropDownItems.Add("left-click-drag to pan");
            (menu.Items[i] as ToolStripMenuItem).DropDownItems.Add("right-click-drag to zoom");
            (menu.Items[i] as ToolStripMenuItem).DropDownItems.Add("middle-click for auto-axis");
            (menu.Items[i] as ToolStripMenuItem).DropDownItems.Add("double-click to toggle benchmark");
            (menu.Items[i] as ToolStripMenuItem).DropDownItems[0].Enabled = false;
            (menu.Items[i] as ToolStripMenuItem).DropDownItems[1].Enabled = false;
            (menu.Items[i] as ToolStripMenuItem).DropDownItems[2].Enabled = false;
            (menu.Items[i] as ToolStripMenuItem).DropDownItems[3].Enabled = false;
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("About ScottPlot");
            return menu;
        }

        public static void RightClickMenuItemClicked(ToolStripItem item, ContextMenuStrip rightClickMenu, ScottPlot.Plot plt)
        {
            switch (item.ToString())
            {
                case "Save Image":
                    rightClickMenu.Hide();
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.FileName = "ScottPlot.png";
                    savefile.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
                    if (savefile.ShowDialog() == DialogResult.OK)
                        plt.SaveFig(savefile.FileName);
                    break;
                case "Auto-Axis":
                    rightClickMenu.Hide();
                    plt.AxisAuto();
                    break;
                case "Clear":
                    rightClickMenu.Hide();
                    plt.Clear();
                    break;
                case "About ScottPlot":
                    rightClickMenu.Hide();
                    System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
                    break;
            }
        }
    }
}
