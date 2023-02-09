using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;

public partial class DataLogger : Form
{
    readonly FakeDataGenerator Data = new(3);

    public DataLogger()
    {
        InitializeComponent();
    }

    private void newDataTimer_Tick(object sender, EventArgs e)
    {
        if (!cbRun.Checked)
            return;

        Data.AddData();
        lblCh1.Text = $"{Data.GetLatest(0):N2}";
        lblCh2.Text = $"{Data.GetLatest(1):N2}";
        lblCh3.Text = $"{Data.GetLatest(2):N2}";
        lblReads.Text = $"Reads: {Data.Reads:N0}";
    }

    private void plotUpdateTimer_Tick(object sender, EventArgs e)
    {
        double sampleRate = 100;
        formsPlot1.Plot.Clear();
        formsPlot1.Plot.AddSignal(Data.GetData(0), sampleRate, Color.Red);
        formsPlot1.Plot.AddSignal(Data.GetData(1), sampleRate, Color.Green);
        formsPlot1.Plot.AddSignal(Data.GetData(2), sampleRate, Color.Blue);

        if (cbAutoscale.Checked)
        {
            formsPlot1.Plot.AxisAuto();
        }

        formsPlot1.Refresh();
    }
}

public class FakeDataGenerator
{
    readonly List<double>[] DataChannels;

    readonly Random Rand = new();

    public int Reads { get; private set; } = 0;

    public FakeDataGenerator(int channels)
    {
        double[] initialValues = Enumerable
            .Range(0, channels)
            .Select(x => Rand.NextDouble() * 100)
            .ToArray();

        DataChannels = initialValues
            .Select(x => new List<double>() { x })
            .ToArray();
    }

    public void AddData(int maxCount = 10)
    {
        int count = (int)(Rand.NextDouble() * maxCount);

        for (int i = 0; i < count; i++)
        {
            foreach (List<double> channel in DataChannels)
            {
                channel.Add(channel.Last() + Rand.NextDouble() - .5);
            }
            Reads += 1;
        }
    }

    public double GetLatest(int channel)
    {
        return DataChannels[channel].Last();
    }

    public double[] GetData(int channel)
    {
        return DataChannels[channel].ToArray();
    }
}
