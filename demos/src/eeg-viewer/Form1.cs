using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eeg_viewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines(LocateDataFile("EEG.txt"));
            int slipLines = 6;
            for (int channel = 1; channel <= 8; channel++)
            {
                double[] channelData = new double[lines.Length - slipLines];
                for (int lineIndex = slipLines; lineIndex < lines.Length; lineIndex++)
                {
                    string[] valueStrings = lines[lineIndex].Split(',');
                    double value = double.Parse(valueStrings[channel]);
                    channelData[lineIndex - slipLines] = value;
                }
                //ScottPlot.Tools.ApplyBaselineSubtraction(channelData, 250, 500);
                formsPlot1.plt.PlotSignal(channelData, sampleRate: 250, label: $"Channel {channel}");
            }
            formsPlot1.plt.Legend();
            formsPlot1.plt.AxisAuto();
            formsPlot1.plt.Title("EEG Signals");
            formsPlot1.plt.YLabel("Lead (microvolts)");
            formsPlot1.plt.XLabel("Time (seconds)");
            formsPlot1.Render();
        }

        private string LocateDataFile(string fileName)
        {
            string folderHere = System.IO.Path.GetFullPath("./");
            string folderData = System.IO.Path.GetFullPath("../../../../data/");

            string pathHere = System.IO.Path.Combine(folderHere, fileName);
            if (System.IO.File.Exists(pathHere))
                return pathHere;

            string pathData = System.IO.Path.Combine(folderData, fileName);
            if (System.IO.File.Exists(pathData))
                return pathData;

            throw new ArgumentException($"could not locate {fileName}");
        }
    }
}
