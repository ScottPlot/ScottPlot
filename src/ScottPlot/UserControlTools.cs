using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
