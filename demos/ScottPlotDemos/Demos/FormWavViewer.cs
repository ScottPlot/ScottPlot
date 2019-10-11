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
    public partial class FormWavViewer : Form
    {
        public FormWavViewer()
        {
            InitializeComponent();
        }

        private void FormWavViewer_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("mozart.wav"))
                LoadFile("mozart.wav");
            else if (System.IO.File.Exists("../../mozart.wav"))
                LoadFile("../../mozart.wav");
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "WAV files (*.wav)|*.wav";
            if (diag.ShowDialog() == DialogResult.OK)
                LoadFile(diag.FileName);
        }

        private void LoadFile(string wavFilePath)
        {
            wavFilePath = System.IO.Path.GetFullPath(wavFilePath);
            tbCurrentFile.Text = wavFilePath;

            double[] wavData = ReadWavFile(wavFilePath);

            // here PlotSignalConst() is used over PlotSignal() because it's faster and the source data does not change.

            formsPlot1.plt.PlotSignalConst(wavData, sampleRate: 8000, color: Color.Red);
            formsPlot1.plt.AxisAuto();
            formsPlot1.plt.YLabel("Amplitue");
            formsPlot1.plt.XLabel("Time (seconds)");
            formsPlot1.plt.Title(System.IO.Path.GetFileName(wavFilePath));
            formsPlot1.Render();
        }

        private double[] ReadWavFile(string wavFilePath)
        {
            // quick and drity WAV file reader (16-bit signed format)
            byte[] bytes = System.IO.File.ReadAllBytes(wavFilePath);
            double[] pcm = new double[bytes.Length / 2];
            int firstDataByte = 44;
            for (int i = firstDataByte; i < bytes.Length - 2; i += 2)
                pcm[i / 2] = BitConverter.ToInt16(bytes, i);
            return pcm;
        }
    }
}
