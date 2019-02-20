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
        private SwhEar swhear;

        public Form1()
        {
            InitializeComponent();
            scottPlotUC1.plt.settings.figureBgColor = SystemColors.Control;
            scottPlotUC1.plt.settings.title = "Audio Level (Circular Buffer)";
            scottPlotUC1.plt.settings.axisLabelX = "Time (seconds)";
            scottPlotUC1.plt.settings.axisLabelY = "Amplitude (frac max)";
            scottPlotUC1.plt.settings.SetDataPadding(75, null, null, null);
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
            //MicrophoneSelected();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;

            lblStatus.Text = $"Listening to device ID {comboMicrophone.SelectedIndex} ...";
            swhear = new SwhEar(comboMicrophone.SelectedIndex);
            scottPlotUC1.plt.data.Clear();
            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;

            if (swhear!=null)
                swhear.Stop();
            timer1.Enabled = false;
        }

        double maxAmplitude = Math.Pow(2, 16);
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (swhear.values == null)
                return;

            if (scottPlotUC1.plt.data.Count() == 0)
            {
                scottPlotUC1.plt.data.AddSignal(swhear.values, swhear.SAMPLERATE);
                scottPlotUC1.plt.settings.AxisFit(0, .1);
                scottPlotUC1.plt.settings.axisY.Set(-maxAmplitude / 2, maxAmplitude / 2);
            }

            // set level meter
            double lastBufferPeakFrac = (double)(swhear.lastAmplitude) / maxAmplitude;
            pbLevelMask.Height = pnlLevel.Height - (int)(lastBufferPeakFrac * pnlLevel.Height);

            // force a ScottPlot render
            scottPlotUC1.plt.data.ClearAxisLines();
            scottPlotUC1.plt.data.AddVertLine((double)swhear.lastPointUpdated / swhear.SAMPLERATE, lineColor: Color.Red);
            
            // render the image
            scottPlotUC1.Render();
        }
    }
}
