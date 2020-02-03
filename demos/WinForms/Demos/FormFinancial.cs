using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormFinancial : Form
    {
        public FormFinancial()
        {
            InitializeComponent();
            Button1_Click(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        ScottPlot.PlottableOHLC plottedOHLCs;
        ScottPlot.PlottableText plottedText;
        ScottPlot.PlottableVLine plottedLine;

        private void GenerateNewData()
        {
            Random rand = new Random();

            int pointCount = 100;

            ScottPlot.OHLC[] ohlcs = ScottPlot.DataGen.RandomStockPrices(rand, pointCount);

            double[] volumes = ScottPlot.DataGen.Random(rand, pointCount, 500, 1000);
            double[] timestamps = new double[ohlcs.Length];
            for (int i = 0; i < timestamps.Length; i++)
                timestamps[i] = ohlcs[i].time;

            // stock price chart
            formsPlot1.plt.Clear();
            formsPlot1.plt.YLabel("Share Price");
            formsPlot1.plt.Title("ScottPlot Candlestick Demo");
            plottedLine = formsPlot1.plt.PlotVLine(timestamps[0], color: Color.Gray, lineStyle: ScottPlot.LineStyle.Dash);
            plottedLine.visible = false;
            if (rbCandle.Checked)
                plottedOHLCs = formsPlot1.plt.PlotCandlestick(ohlcs);
            else
                plottedOHLCs = formsPlot1.plt.PlotOHLC(ohlcs);
            plottedText = formsPlot1.plt.PlotText("", timestamps[0], ohlcs[0].low,
                bold: true, fontSize: 10, color: Color.Black,
                frame: true, frameColor: Color.DarkGray);
            formsPlot1.plt.Ticks(dateTimeX: true);
            formsPlot1.plt.AxisAuto();

            // volume chart
            formsPlot2.plt.Clear();
            formsPlot2.plt.YLabel("Volume");
            formsPlot2.plt.PlotBar(timestamps, volumes, barWidth: .5);
            formsPlot2.plt.AxisAuto(.01, .1);
            formsPlot2.plt.Axis(null, null, 0, null);
            formsPlot2.plt.Ticks(dateTimeX: true);

            formsPlot1.plt.MatchLayout(formsPlot2.plt, horizontal: true, vertical: false);
            formsPlot1.Render();
            formsPlot2.Render();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GenerateNewData();
        }

        private void RbCandle_CheckedChanged(object sender, EventArgs e)
        {
            GenerateNewData();
        }

        private void RbOHLC_CheckedChanged(object sender, EventArgs e)
        {
            GenerateNewData();
        }

        private void formsPlot1_AxesChanged(object sender, EventArgs e)
        {
            formsPlot2.plt.MatchAxis(formsPlot1.plt, horizontal: true, vertical: false);
            formsPlot2.Render();
        }

        private void formsPlot2_AxesChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.MatchAxis(formsPlot2.plt, horizontal: true, vertical: false);
            formsPlot1.Render();
        }

        private void formsPlot1_MouseMoved(object sender, EventArgs e)
        {
            Point mouseLoc = new Point(Cursor.Position.X, Cursor.Position.Y);
            mouseLoc.X -= this.PointToScreen(formsPlot1.Location).X;
            mouseLoc.Y -= this.PointToScreen(formsPlot1.Location).Y;

            PointF mouseCoordinate = formsPlot1.plt.CoordinateFromPixel(mouseLoc);
            DateTime dt = DateTime.FromOADate(mouseCoordinate.X);

            // determine which OHLC is closest to the mouse
            int closestIndex = 0;
            double closestDistance = double.PositiveInfinity;
            for (int i = 0; i < plottedOHLCs.pointCount; i++)
            {
                //Added all 4 points to make sure tooltip covers all the size of the candle
                //@ High
                double dx = mouseLoc.X - formsPlot1.plt.CoordinateToPixel(plottedOHLCs.ohlcs[i].time, 0).X;
                double dy = mouseLoc.Y - formsPlot1.plt.CoordinateToPixel(0, plottedOHLCs.ohlcs[i].high).Y;
                double distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (closestIndex < 0)
                {
                    closestDistance = distance;
                }
                else if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }

                //@ Low
                dx = mouseLoc.X - formsPlot1.plt.CoordinateToPixel(plottedOHLCs.ohlcs[i].time, 0).X;
                dy = mouseLoc.Y - formsPlot1.plt.CoordinateToPixel(0, plottedOHLCs.ohlcs[i].low).Y;
                distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (closestIndex < 0)
                {
                    closestDistance = distance;
                }
                else if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }

                //@ Open
                dx = mouseLoc.X - formsPlot1.plt.CoordinateToPixel(plottedOHLCs.ohlcs[i].time, 0).X;
                dy = mouseLoc.Y - formsPlot1.plt.CoordinateToPixel(0, plottedOHLCs.ohlcs[i].open).Y;
                distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (closestIndex < 0)
                {
                    closestDistance = distance;
                }
                else if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }

                //@ Close
                dx = mouseLoc.X - formsPlot1.plt.CoordinateToPixel(plottedOHLCs.ohlcs[i].time, 0).X;
                dy = mouseLoc.Y - formsPlot1.plt.CoordinateToPixel(0, plottedOHLCs.ohlcs[i].low).Y;
                distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (closestIndex < 0)
                {
                    closestDistance = distance;
                }
                else if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            if (closestDistance < 20)
            {
                // we are close enough to a OHLC to label it
                plottedText.x = plottedOHLCs.ohlcs[closestIndex].time + .5;
                plottedText.y = plottedOHLCs.ohlcs[closestIndex].close;
                plottedText.text = string.Format(
                        " {0} \n High : {1}\n Open : {2}\n Close: {3}\n Low  : {4}",
                        dt.ToShortDateString(),
                        Math.Round(plottedOHLCs.ohlcs[closestIndex].high, 2),
                        Math.Round(plottedOHLCs.ohlcs[closestIndex].open, 2),
                        Math.Round(plottedOHLCs.ohlcs[closestIndex].close, 2),
                        Math.Round(plottedOHLCs.ohlcs[closestIndex].low, 2)
                    );
                plottedLine.position = plottedOHLCs.ohlcs[closestIndex].time;
                plottedLine.visible = true;
            }
            else
            {
                // the mouse isnt close to an OHLC
                plottedText.text = "";
                plottedLine.visible = false;
            }

            formsPlot1.Render(skipIfCurrentlyRendering: true);
        }
    }
}
