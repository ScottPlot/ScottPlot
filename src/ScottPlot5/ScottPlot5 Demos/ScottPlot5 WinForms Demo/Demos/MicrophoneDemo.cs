namespace WinForms_Demo.Demos;

public partial class MicrophoneDemo : Form, IDemoWindow
{
    public string Title => $"Microphone Demo";
    public string Description => $"Plot real-time audio signal data from the microphone";

    // audio sampling settings
    const int SampleRate = 44100;
    const int BitDepth = 16;
    const int ChannelCount = 1;
    const int BufferMilliseconds = 20;
    int BuffersProcessed = 0;
    NAudio.Wave.WaveInEvent? Wave = null;

    // this fixed-size buffer holds the latest values from the microphone
    readonly double[] AudioBuffer = new double[SampleRate * BufferMilliseconds / 1000];

    // this timer requests frequent refreshes
    readonly System.Windows.Forms.Timer RenderTimer = new() { Interval = 20, Enabled = true };

    // use a data logger to display growing datasets
    readonly ScottPlot.Plottables.DataLogger DataLogger;

    public MicrophoneDemo()
    {
        InitializeComponent();

        // display the latest audio buffer in a fixed-length array using a signal plot
        formsPlot1.Plot.Add.Signal(AudioBuffer, period: 1000.0 / SampleRate);
        formsPlot1.Plot.Axes.MarginsX(0);
        formsPlot1.Plot.Title("Audio Buffer");
        formsPlot1.Plot.YLabel("PCM Value");
        formsPlot1.Plot.XLabel("Time (milliseconds)");

        // plot audio level over time using a datalogger
        DataLogger = formsPlot2.Plot.Add.DataLogger();
        formsPlot2.Plot.Title("Audio Volume");
        formsPlot2.Plot.YLabel("PPV");
        formsPlot2.Plot.XLabel("Time (seconds)");

        for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
        {
            var caps = NAudio.Wave.WaveIn.GetCapabilities(i);
            comboBox1.Items.Add(caps.ProductName);
        }

        comboBox1.SelectedIndexChanged += (s, e) => StartMonitoring(comboBox1.SelectedIndex);
        if (comboBox1.Items.Count > 0)
            comboBox1.SelectedIndex = 1;

        RenderTimer.Tick += (s, e) =>
        {
            formsPlot1.Plot.Axes.AutoScaleExpandY();
            formsPlot1.Refresh();

            double secondsElapsed = BuffersProcessed * BufferMilliseconds / 1000.0;
            double amplitude = AudioBuffer.Max() - AudioBuffer.Min();
            DataLogger.Add(secondsElapsed, amplitude);
            formsPlot2.Refresh();
        };
    }

    private void StartMonitoring(int deviceNumber)
    {
        if (Wave is not null)
        {
            Wave.StopRecording();
            Wave.Dispose();
        }

        Wave = new NAudio.Wave.WaveInEvent()
        {
            DeviceNumber = deviceNumber,
            WaveFormat = new NAudio.Wave.WaveFormat(SampleRate, BitDepth, ChannelCount),
            BufferMilliseconds = BufferMilliseconds
        };

        Wave.DataAvailable += (s, e) =>
        {
            // Don't request a plot refresh inside this event because
            // the thread that invokes is not the same as the UI thread.
            // However, values in the fixed length buffer may be modified.
            for (int i = 0; i < e.Buffer.Length / 2; i++)
            {
                AudioBuffer[i] = BitConverter.ToInt16(e.Buffer, i * 2);
            }
            BuffersProcessed += 1;
        };

        Wave.StartRecording();
    }
}
