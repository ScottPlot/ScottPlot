using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;

namespace ScottPlotTests.Axis
{
    class ImageLabel
    {
        [Test]
        public void Test_ImageLabel_Default()
        {
            var plt = new ScottPlot.Plot();
            plt.Style(Style.Blue1);

            plt.YAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"));
            plt.XAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"));
            plt.YAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"));
            plt.XAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"));

            plt.YAxis2.Ticks(true);
            plt.XAxis2.Ticks(true);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_ImageLabel_MuchPadding()
        {
            var plt = new ScottPlot.Plot();
            plt.Style(Style.Blue1);

            plt.YAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 50);
            plt.XAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 50);
            plt.YAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 50);
            plt.XAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 50);

            plt.YAxis2.Ticks(true);
            plt.XAxis2.Ticks(true);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_ImageLabel_MuchPaddingOutside()
        {
            var plt = new ScottPlot.Plot();
            plt.Style(Style.Blue1);

            plt.YAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 0, 50);
            plt.XAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 0, 50);
            plt.YAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 0, 50);
            plt.XAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 0, 50);

            plt.YAxis2.Ticks(true);
            plt.XAxis2.Ticks(true);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_ImageLabel_MuchPaddingInside()
        {
            var plt = new ScottPlot.Plot();
            plt.Style(Style.Blue1);

            plt.YAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 5);
            plt.XAxis.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 5);
            plt.YAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 5);
            plt.XAxis2.ImageLabel(new System.Drawing.Bitmap("Images/theta.jpg"), 50, 5);

            plt.YAxis2.Ticks(true);
            plt.XAxis2.Ticks(true);

            TestTools.SaveFig(plt);
        }
    }
}
