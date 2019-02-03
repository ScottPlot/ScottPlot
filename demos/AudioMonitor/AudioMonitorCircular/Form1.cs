using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioMonitor
{
    public partial class Form1 : Form
    {
        private SWHearCircular swhear;
        public bool autoScaleHappened = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScanForMicrophones();
        }

        private void ScanForMicrophones()
        {
            List<string> audioDeviceProductNames = new List<string>();

            for (int devIndex = 0; devIndex < NAudio.Wave.WaveIn.DeviceCount; devIndex++)
            {
                string devName = NAudio.Wave.WaveIn.GetCapabilities(devIndex).ProductName;
                audioDeviceProductNames.Add($"Device {devIndex}: {devName}");
            }
                

            comboMicrophone.Items.Clear();
            if (audioDeviceProductNames.Count > 0)
            {
                comboMicrophone.Items.AddRange(audioDeviceProductNames.ToArray());
                comboMicrophone.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("ERROR: no recording device is plugged in!");
            }
        }

        private void MicrophoneSelected()
        {
            btnStart_Click(null, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // GUI BINDINGS

        private void btnMicScan_Click(object sender, EventArgs e)
        {
            ScanForMicrophones();
        }

        private void comboMicrophone_SelectedIndexChanged(object sender, EventArgs e)
        {
            MicrophoneSelected();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;

            swhear = new SWHearCircular(comboMicrophone.SelectedIndex);
            swhear.Open();

            scottPlotUC1.plt.data.Clear();
            scottPlotUC1.plt.data.AddSignal(swhear.CircularBuffer, swhear.SampleRate);
            scottPlotUC1.plt.settings.figureBgColor = SystemColors.Control;
            scottPlotUC1.plt.settings.title = "Audio Level (Circular Buffer)";
            scottPlotUC1.plt.settings.axisLabelX = "Time (seconds)";
            scottPlotUC1.plt.settings.axisLabelY = "Amplitude (frac max)";

            timer1.Enabled = true;
            lblStatus.Text = $"Listening to device ID {swhear.DeviceIndex} ...";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;

            swhear.Close();
            timer1.Enabled = false;
        }

        int lastProcessedBuffer = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (swhear.BuffersRead == lastProcessedBuffer)
                return;

            // set level meter
            lastProcessedBuffer = swhear.BuffersRead;
            double maxAmplitude = Math.Pow(2, 16);
            double lastBufferPeakFrac = swhear.lastBufferAmplitude / maxAmplitude;
            pbLevelMask.Height = pnlLevel.Height - (int)(lastBufferPeakFrac * pnlLevel.Height);

            // force a ScottPlot render
            scottPlotUC1.plt.data.ClearAxisLines();
            scottPlotUC1.plt.data.AddVertLine(swhear.CircularBufferNextIndexSec, lineColor: Color.Red);

            // auto-scale after a few recordings
            if (swhear.BuffersRead > 10 && autoScaleHappened == false)
            {
                scottPlotUC1.plt.settings.AxisFit(0, .5);
                autoScaleHappened = true;
            }
            
            // render the image
            scottPlotUC1.Render();
        }
    }
}
