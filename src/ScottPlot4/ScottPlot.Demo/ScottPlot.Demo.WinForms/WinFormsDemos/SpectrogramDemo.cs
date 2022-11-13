using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class SpectrogramDemo : Form
    {
        public SpectrogramDemo()
        {
            InitializeComponent();
        }

        private void SpectrogramDemo_Load(object sender, EventArgs e)
        {
            comboBoxMoveDirection.Items.Add(new NameValue<SpecPlotMoveOrientation>(SpecPlotMoveOrientation.Right));
            comboBoxMoveDirection.Items.Add(new NameValue<SpecPlotMoveOrientation>(SpecPlotMoveOrientation.Left));
            comboBoxMoveDirection.Items.Add(new NameValue<SpecPlotMoveOrientation>(SpecPlotMoveOrientation.Down));
            comboBoxMoveDirection.Items.Add(new NameValue<SpecPlotMoveOrientation>(SpecPlotMoveOrientation.Up));
            comboBoxMoveDirection.SelectedIndex = 0;

            comboBoxMoveMode.Items.Add(new NameValue<SpecPlotMoveMode>(SpecPlotMoveMode.Full));
            comboBoxMoveMode.Items.Add(new NameValue<SpecPlotMoveMode>(SpecPlotMoveMode.Always));
            comboBoxMoveMode.SelectedIndex = 0;
        }

        private void btnChioceDemoDataFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtDemoDataFilePath.Text = ofd.FileName;
            }
        }


        private Spectrogram _specPlot = null;
        private SpectrogramData _data = null;
        private CancellationTokenSource _cts = null;
        private string _filePath = null;
        private long _timeTicks = -1;
        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._cts != null)
                {
                    this._cts.Cancel();
                    this._cts.Dispose();
                    this._cts = null;
                    return;
                }

                this._filePath = txtDemoDataFilePath.Text.Trim();
                if (string.IsNullOrWhiteSpace(this._filePath) || !System.IO.File.Exists(this._filePath))
                {
                    MessageBox.Show("文件路径为空或不存在(filepath is null or not exist)");
                    return;
                }

                int dataHeigth, dataWidth;
                var moveOrientation = ((NameValue<SpecPlotMoveOrientation>)comboBoxMoveDirection.SelectedItem).Value;
                var moveMode = ((NameValue<SpecPlotMoveMode>)comboBoxMoveMode.SelectedItem).Value;
                switch (moveOrientation)
                {
                    case SpecPlotMoveOrientation.Left:
                        dataHeigth = 1024;
                        dataWidth = 500;
                        formsPlot1.Plot.XLabel("Time");
                        formsPlot1.Plot.YLabel("BW");
                        formsPlot1.Plot.XAxis.TickLabelFormat(this.SpecPlotAxisYTimePositionFormatter);
                        formsPlot1.Plot.YAxis.TickLabelFormat(this.SpecPlotAxisXFreqPositionFormatter);
                        break;
                    case SpecPlotMoveOrientation.Right:
                        dataHeigth = 1024;
                        dataWidth = 500;
                        formsPlot1.Plot.XLabel("Time");
                        formsPlot1.Plot.YLabel("BW");
                        formsPlot1.Plot.XAxis.TickLabelFormat(this.SpecPlotAxisYTimePositionFormatter);
                        formsPlot1.Plot.YAxis.TickLabelFormat(this.SpecPlotAxisXFreqPositionFormatter);
                        break;
                    case SpecPlotMoveOrientation.Up:
                        dataHeigth = 100;
                        dataWidth = 1024;
                        formsPlot1.Plot.XLabel("BW");
                        formsPlot1.Plot.YLabel("Time");
                        formsPlot1.Plot.YAxis.TickLabelFormat(this.SpecPlotAxisYTimePositionFormatter);
                        formsPlot1.Plot.XAxis.TickLabelFormat(this.SpecPlotAxisXFreqPositionFormatter);
                        break;
                    case SpecPlotMoveOrientation.Down:
                        dataHeigth = 100;
                        dataWidth = 1024;
                        formsPlot1.Plot.XLabel("BW");
                        formsPlot1.Plot.YLabel("Time");
                        formsPlot1.Plot.YAxis.TickLabelFormat(this.SpecPlotAxisYTimePositionFormatter);
                        formsPlot1.Plot.XAxis.TickLabelFormat(this.SpecPlotAxisXFreqPositionFormatter);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                this._data = new SpectrogramData(dataHeigth, dataWidth, -70, moveOrientation, moveMode);
                this._timeTicks = DateTime.Now.Ticks;

                if (this._specPlot != null)
                {
                    formsPlot1.Plot.Remove(this._specPlot);
                    this._specPlot.Dispose();
                }

                formsPlot1.Plot.Clear();
                this._specPlot = formsPlot1.Plot.AddSpectrogram(this._data, ScottPlot.Drawing.Colormap.Turbo);
                formsPlot1.Plot.AddColorbar(this._specPlot);


                //设置Y轴时间
                //plot.Plot.SetAxisLimitsY(minTime, maxTime);

                ////设置X轴频率范围
                //plot.Plot.SetAxisLimitsX(freqMin, freqMax);

                formsPlot1.Render();

                this._cts = new CancellationTokenSource();
                Task.Factory.StartNew(this.TestThreadMethod, this._cts, TaskCreationOptions.LongRunning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TestThreadMethod(object state)
        {
            try
            {
                var cts = (CancellationTokenSource)state;
                var token = cts.Token;
                this._data.Clear();

                int fftCount;

                if (this._data.MoveOrientation == SpecPlotMoveOrientation.Up ||
                    this._data.MoveOrientation == SpecPlotMoveOrientation.Down)
                {
                    fftCount = this._data.DataWidth;
                }
                else
                {
                    fftCount = this._data.DataHeight;
                }

                int fs = 48000;
                double timeInterval = (double)fftCount / fs;
                double noise = 0;

                Action rendrAction = new Action(() =>
                {
                    try
                    {
                        var specPlot = this._specPlot;
                        if (specPlot != null)
                        {
                            specPlot.Update();
                        }
                        formsPlot1.Render();
                    }
                    catch (Exception)
                    { }
                });


                using (var stream = new System.IO.FileStream(this._filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                {
                    var br = new System.IO.BinaryReader(stream);
                    Complex[] fftData = new Complex[fftCount];
                    double iData, qData;
                    var minDataLeng = fftCount * 4;
                    int index;
                    int midIndex = fftCount / 2;
                    int halfFFTCount = midIndex;
                    double[] values = new double[fftCount];

                    while (!token.IsCancellationRequested &&
                        stream.Length - stream.Position >= minDataLeng)
                    {
                        for (int i = 0; i < fftCount; i++)
                        {
                            iData = br.ReadInt16();
                            qData = br.ReadInt16();
                            fftData[i] = new Complex(iData, qData);
                        }

                        //IQ缓存数据做FFT
                        MathNet.Numerics.IntegralTransforms.Fourier.Forward(fftData);

                        index = 0;
                        this.UpdatePowerSpecData(values, fftData, ref index, midIndex, fftCount, noise, fs, halfFFTCount, timeInterval);
                        this.UpdatePowerSpecData(values, fftData, ref index, 0, midIndex, noise, fs, halfFFTCount, timeInterval);
                        this._data.Append(values);
                        this._timeTicks = DateTime.Now.Ticks;
                        this.Invoke(rendrAction);

                        try
                        {
                            token.WaitHandle.WaitOne(50);
                        }
                        catch (ObjectDisposedException)
                        { }
                    }
                }
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void UpdatePowerSpecData(double[] values, Complex[] fftData, ref int index, int startIndex, int endIndex, double noise, int sample, int halfFFTCount, double timeInterval)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                double value = Math.Sqrt(Math.Pow(fftData[i].Real, 2) + Math.Pow(fftData[i].Imaginary, 2));
                value = Math.Pow(value / halfFFTCount, 2) / timeInterval;
                value = 10 * Math.Log10(value);
                value += noise;
                values[index] = value;
                index++;
            }
        }


        private string SpecPlotAxisXFreqPositionFormatter(double value)
        {
            try
            {
                const double FREQ_MIN = 100d;
                const double FREQ_MAX = 200d;

                int fftCount = this._data.DataWidth;
                var ret = FREQ_MIN + (FREQ_MAX - FREQ_MIN) * value / fftCount;
                return $"{Math.Round(ret, 3)}MHz";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message},{value}");
                return $"{value}MHz";
            }
        }

        private string SpecPlotAxisYTimePositionFormatter(double value)
        {
            try
            {
                const double DURATION_SECONDS = 10;
                var time = new DateTime(this._timeTicks);
                var refTime = time.Subtract(TimeSpan.FromSeconds(DURATION_SECONDS));

                int frams = this._data.DataHeight;
                var timeOffset = DURATION_SECONDS * value / frams;

                SpectrogramData data = this._data;
                string timeStr;

                if (data == null)
                {
                    timeStr = refTime.AddSeconds(DURATION_SECONDS - timeOffset).ToString("HH:mm:ss");
                }
                else
                {
                    switch (data.MoveOrientation)
                    {
                        case SpecPlotMoveOrientation.Right:
                            timeStr = refTime.AddSeconds(DURATION_SECONDS - timeOffset).ToString("HH:mm:ss");
                            break;
                        case SpecPlotMoveOrientation.Left:
                            timeStr = refTime.AddSeconds(timeOffset).ToString("HH:mm:ss");
                            break;
                        case SpecPlotMoveOrientation.Up:
                            timeStr = refTime.AddSeconds(DURATION_SECONDS - timeOffset).ToString("HH:mm:ss");
                            break;
                        case SpecPlotMoveOrientation.Down:
                            timeStr = refTime.AddSeconds(timeOffset).ToString("HH:mm:ss");
                            break;
                        default:
                            timeStr = $"{value}s";
                            break;
                    }
                }

                return timeStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message},{value}");
                return $"{value}s";
            }
        }

        private void SpectrogramDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._cts != null)
            {
                this._cts.Cancel();
                this._cts.Dispose();
                this._cts = null;
            }

            if (this._specPlot != null)
            {
                formsPlot1.Plot.Remove(this._specPlot);
                this._specPlot.Dispose();
                this._specPlot = null;
            }
        }
    }

    public class NameValue<TValue>
    {
        public NameValue(string name, TValue value)
        {
            this.Name = name;
            this.Value = value;
        }

        public NameValue(TValue value)
        {
            this.Name = value.ToString();
            this.Value = value;
        }

        public string Name { get; private set; }

        public TValue Value { get; private set; }


        public override string ToString()
        {
            return this.Name;
        }
    }
}
