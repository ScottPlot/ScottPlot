using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using NAudio.Wave; // installed with nuget
//using NAudio.CoreAudioApi;

namespace _18._01._03_live_data
{
    public partial class Form1 : Form
    {

        // audio stuff
        public WaveIn wi;
        public BufferedWaveProvider bwp;
        public Int32 envelopeMax;
        private int RATE = 8000; // sample rate of the sound card
        //private int BUFFERSIZE = (int)Math.Pow(2, 13); // must be a multiple of 2
        private int BUFFERSIZE = (int)Math.Pow(2, 9); // must be a multiple of 2
        private bool listening = false;

        // graph and data stuff
        private const int GRAPHSIZE = 8000 * 5 / 2; // rate time seconds, /2 for 16-bit
        private double[] Xs = new double[GRAPHSIZE];
        private double[] Ys = new double[GRAPHSIZE];
        private Random rand = new Random();

        public Form1()
        {
            InitializeComponent();


            // fill the data window with something random
            for (int i = 0; i < GRAPHSIZE; i++)
            {
                Xs[i] = (double)i/RATE*2; // time in seconds
                Ys[i] = rand.NextDouble() * 10; // start with random data
            }

            System.Console.WriteLine("BUFFER SIZE: {0}", BUFFERSIZE);

        }

        /// <summary>
        /// update the interactive graph with the new Xs and Ys
        /// </summary>
        private void update_graph()
        {
            if (scottPlotUC1.Xs.Length != Xs.Length)
            {
                // if the first run, reset the axis limits
                scottPlotUC1.SetData(Xs, Ys);
            }
            else
            {
                // if not the first run, respect current axis limits
                scottPlotUC1.Xs = Xs;
                scottPlotUC1.Ys = Ys;
                scottPlotUC1.UpdateGraph();
            }
            
        }

        /// <summary>
        /// continuously graph PCM data coming in on the microphone
        /// </summary>
        private void listen()
        {
            listening = true;

            // see what audio devices are available
            int devcount = WaveIn.DeviceCount;
            Console.Out.WriteLine("Device Count: {0}.", devcount);

            // get the WaveIn class started
            WaveIn wi = new WaveIn();
            wi.DeviceNumber = 0;
            wi.WaveFormat = new NAudio.Wave.WaveFormat(RATE, 1);

            // create a wave buffer and start the recording
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.BufferLength = BUFFERSIZE * 2;

            bwp.DiscardOnBufferOverflow = true;
            wi.StartRecording();

        }

        // adds data to the audio recording buffer
        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            // record some new audio
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);

            // dont know how this works
            var frames = new byte[BUFFERSIZE];
            bwp.Read(frames, 0, BUFFERSIZE);

            // convert to double
            int SAMPLE_RESOLUTION = 16; // this is for 16-bit integers in a byte array
            int BYTES_PER_POINT = SAMPLE_RESOLUTION / 8;
            Int32[] vals = new Int32[frames.Length / BYTES_PER_POINT];
            for (int i = 0; i < vals.Length; i++)
            {
                byte hByte = frames[i * 2 + 1];
                byte lByte = frames[i * 2 + 0];
                vals[i] = (int)(short)((hByte << 8) | lByte);
            }

            // roll the data left by copying it in place
            Array.Copy(Ys, vals.Length, Ys, 0, Ys.Length - vals.Length);

            // set the last data point to something new
            Array.Copy(vals, 0, Ys, Ys.Length - vals.Length, vals.Length);

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listening==false) listen();
            update_graph();
        }

        private void btn_autoscale_Click(object sender, EventArgs e)
        {
            // forces scaling with auto axis
            scottPlotUC1.SetData(Xs, Ys);
        }
        
    }
}
