using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    public class Text
    {
        [Test]
        public void Test_Text_Alignment()
        {
            ScottPlot.Alignment[] alignments = (ScottPlot.Alignment[])Enum.GetValues(typeof(ScottPlot.Alignment));

            var plt = new ScottPlot.Plot(400, 300);

            for (int i = 0; i < alignments.Length; i++)
            {
                double frac = (double)i / alignments.Length;
                double x = Math.Sin(frac * Math.PI * 2);
                double y = Math.Cos(frac * Math.PI * 2);

                var txt = plt.AddText(alignments[i].ToString(), x, y);
                txt.Alignment = alignments[i];
                txt.Font.Color = System.Drawing.Color.Black; ;
                txt.BackgroundColor = System.Drawing.Color.LightSteelBlue;
                txt.BackgroundFill = true;
                txt.Rotation = 5;
                txt.BorderSize = 2;
                txt.BorderColor = System.Drawing.Color.Navy;

                plt.AddPoint(x, y, System.Drawing.Color.Black);
            }

            plt.Frameless();
            plt.Margins(.5, .2);
            TestTools.SaveFig(plt);
        }
    }
}
