using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotAudioMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StylePcm();
            ScanSoundCards();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ScanSoundCards()
        {
            cbDevice.Items.Clear();
            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
                cbDevice.Items.Add(NAudio.Wave.WaveIn.GetCapabilities(i).ProductName);
            if (cbDevice.Items.Count > 0)
                cbDevice.SelectedIndex = 0;
            else
                MessageBox.Show("ERROR: no recording devices available");
        }

        private void StylePcm()
        {
            scottPlotUC1.plt.Title("");
            scottPlotUC1.plt.YLabel("PCM (amplitude)", fontSize: 12);
            scottPlotUC1.plt.XLabel("Time (millisec)", fontSize: 12);
            scottPlotUC1.Render();

            scottPlotUC2.plt.Title("");
            scottPlotUC2.plt.YLabel("FFT (power)", fontSize: 12);
            scottPlotUC2.plt.XLabel("Frequency (kHz)", fontSize: 12);
            scottPlotUC2.Render();
        }

        private void updateFFT()
        {
            if (dataPcm == null)
                return;

            int fftPoints = 2;
            while (fftPoints * 2 <= dataPcm.Length)
                fftPoints *= 2;

            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for (int i = 0; i < fftPoints; i++)
                fftFull[i].X = (float)(dataPcm[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));

            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

            if (dataFft == null)
                dataFft = new double[fftPoints / 2];
            for (int i = 0; i < fftPoints / 2; i++)
            {
                double fftLeft = Math.Abs(fftFull[i].X + fftFull[i].Y);
                double fftRight = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
                dataFft[i] = fftLeft + fftRight;
            }
        }

        private NAudio.Wave.WaveInEvent wvin;

        double[] dataPcm;
        double[] dataFft;

        private void OnDataAvailable(object sender, NAudio.Wave.WaveInEventArgs args)
        {
            int bytesPerSample = wvin.WaveFormat.BitsPerSample / 8;
            int samplesRecorded = args.BytesRecorded / bytesPerSample;
            Int16[] lastBuffer = new Int16[samplesRecorded];
            if (dataPcm == null)
                dataPcm = new double[samplesRecorded];
            for (int i = 0; i < samplesRecorded; i++)
            {
                lastBuffer[i] = BitConverter.ToInt16(args.Buffer, i * bytesPerSample);
                dataPcm[i] = lastBuffer[i];
            }
            updateFFT();
        }

        private void AudioMonitorInitialize(int DeviceIndex, int sampleRate = 24000, int bitRate = 16,
            int channels = 1, int bufferMilliseconds = 50, bool start = true)
        {
            if (wvin == null)
            {
                wvin = new NAudio.Wave.WaveInEvent();
                wvin.DeviceNumber = DeviceIndex;
                wvin.WaveFormat = new NAudio.Wave.WaveFormat(sampleRate, bitRate, channels);
                wvin.DataAvailable += OnDataAvailable;
                wvin.BufferMilliseconds = bufferMilliseconds;
                if (start)
                    wvin.StartRecording();
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            AudioMonitorInitialize(cbDevice.SelectedIndex);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (wvin != null)
            {
                wvin.StopRecording();
                wvin = null;
                dataPcm = null;
            }
        }

        private void TimerPlot_Tick(object sender, EventArgs e)
        {
            if (dataPcm == null || dataFft == null)
            {
                if (scottPlotUC1.plt.GetPlottables().Count > 0)
                    scottPlotUC1.plt.Clear();
                if (scottPlotUC2.plt.GetPlottables().Count > 0)
                    scottPlotUC2.plt.Clear();
                return;
            }
            else
            {
                if (scottPlotUC1.plt.GetPlottables().Count == 0)
                {
                    // plot the PCM (raw signal)
                    scottPlotUC1.plt.PlotSignal(dataPcm, wvin.WaveFormat.SampleRate / 1000.0, markerSize: 0);
                    scottPlotUC1.plt.AxisAuto(0, .5);

                    // plot the FFT (frequency power)
                    double fftSampleRate = dataFft.Length / (double)wvin.WaveFormat.SampleRate * 2;
                    scottPlotUC2.plt.PlotSignal(dataFft, fftSampleRate * 1000.0, markerSize: 0);
                    scottPlotUC2.plt.AxisAuto(0);
                }

                // update vertical line at peak frequency
                double peakFrequency = getPeakFrequency();
                Console.WriteLine($"Peak frequency: {peakFrequency} Hz");
                scottPlotUC2.plt.Clear(signalPlots: false, axisLines: true);
                scottPlotUC2.plt.PlotVLine(peakFrequency / 1000.0, color: Color.Red, 
                    label: string.Format("peak {0:0} Hz", peakFrequency));
                scottPlotUC2.plt.Legend(location: ScottPlot.legendLocation.upperRight);

                scottPlotUC1.Render();
                scottPlotUC2.Render();
            }
        }

        double getPeakFrequency(double ignoreBelowHz = 200)
        {
            double pointSpacingHz = (double)wvin.WaveFormat.SampleRate / dataFft.Length / 2;

            double peakAmplitude = 0;
            double peakIndex = 0;
            int lowestIndex = (int)(ignoreBelowHz / pointSpacingHz);
            for (int i = lowestIndex; i < dataFft.Length; i++)
            {
                if (dataFft[i] > peakAmplitude)
                {
                    peakAmplitude = dataFft[i];
                    peakIndex = i;
                }
            }

            double peakFrequency = (peakIndex) * pointSpacingHz;
            return peakFrequency;
        }
    }
}
