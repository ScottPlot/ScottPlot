using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ScottPlotABF
{
    public partial class Form1 : Form
    {
        public ABF Abf;

        public Form1()
        {
            InitializeComponent();
            Abf = new ABF();
        }

        void PopulateABFList()
        {
            // scan the ABF folder for ABFs and add them to the listbox.
            lbABFs.Items.Clear();
            string[] files = Directory.GetFiles(tbPath.Text, "*.abf");
            Array.Sort(files);
            foreach (string file in files) lbABFs.Items.Add(Path.GetFileName(file));
            if (lbABFs.Items.Count > 0) lbABFs.SelectedIndex = 0;
        }

        void ReadABFAndPlot()
        {
            
            // read the content of the ABF into a byte array
            string abfPath = System.IO.Path.Combine(tbPath.Text, (string)lbABFs.SelectedItem);
            byte[] array = File.ReadAllBytes(abfPath);

            // automatically detect the header and trailer size
            int headerBytes = Abf.GetHeaderLength(array);
            int trailerBytes = Abf.GetTrailerLength(array);

            // look up the protocol
            lblProtocol.Text = Abf.GetSWHProtocol(array);

            // determine if we need to adjust our data to select certain sweeps
            int RATE = 20000;
            int sweepSize = (int)(RATE * (double)nudSweepLength.Value);
            int sweepOffset = (int)nudSweep.Value * sweepSize;

            // default to the entire reading frame
            int nSweeps = (int)(array.Length / sweepSize);
            int firstByte = headerBytes + sweepOffset;
            int lastByte = firstByte + sweepSize;
            lastByte = Math.Min(lastByte, array.Length - headerBytes - trailerBytes);

            // perhaps ignore the sweeps
            if (cbShowAllSweeps.Checked == true)
            {
                firstByte = headerBytes;
                lastByte = array.Length - headerBytes - trailerBytes;
            }

            // error checking if manually defining sweep number and size
            if (firstByte >= lastByte)
            {
                nudSweepLength.BackColor = Color.Red;
                nudSweep.BackColor = Color.Red;
                return;
            }

            // 16-bit ADC data needs to be scaled by some value.
            //double scale = 5 / Math.Pow(2, 8);
            double scale = (double)nudScale.Value;

            // create our array of doubles to hold the data to be plotted
            int nSamples = (lastByte-firstByte)/2;
            double[] Ys = new double[nSamples];
            for (int i = 0; i < nSamples; i++)
            {
                byte hByte = array[firstByte + i * 2 + 1];
                byte lByte = array[firstByte + i * 2 + 0];
                Ys[i] = (double)(short)((hByte << 8) | lByte) * scale;
            }

            // load the data into the ScottPlot
            scottPlotUC1.SetData(ScottPlot.Generate.Sequence(Ys.Length, 1 / 20000.0), Ys);
            this.Refresh();
            
        }

        private void btnPathScan_Click(object sender, EventArgs e)
        {
            PopulateABFList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnPathBrowse_Click(object sender, EventArgs e)
        {
            // pop up a dialog box to ask what path to scan for ABFs
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowDialog();
            tbPath.Text = fb.SelectedPath;
            PopulateABFList();
        }

        private void lbABFs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSettingsFromProtocol();
            ReadABFAndPlot();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadABFAndPlot();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            PopulateABFList();
        }

        private void nudSweep_ValueChanged(object sender, EventArgs e)
        {
            ReadABFAndPlot();
        }

        private void nudSweepLength_ValueChanged(object sender, EventArgs e)
        {
            ReadABFAndPlot();
        }

        private void cbShowAllSweeps_CheckedChanged(object sender, EventArgs e)
        {
            ReadABFAndPlot();
        }

        public void LoadSettingsFromProtocol()
        {
            ReadABFAndPlot();
            string[] words = lblProtocol.Text.Split(' ');
            string protoID = words[0];
            System.Console.WriteLine($"protocol ID: {protoID}");
            nudSweepLength.BackColor = Color.Lime;
            nudSweep.BackColor = Color.Lime;
            nudScale.BackColor = Color.Lime;
            nudSweep.Value = 0;
            cbShowAllSweeps.Checked = false;
            lblProtocol.ForeColor = Color.Green;
            switch (protoID)
            {
                case "0111": // ramp
                    cbShowAllSweeps.Checked = true;
                    break;
                case "0112": // ap steps
                    nudSweepLength.Value = 6;
                    break;
                case "0113": // ap steps
                    nudSweepLength.Value = 6;
                    break;
                case "0201": // vc memtest
                    nudSweepLength.Value = 1;
                    break;
                case "0202": // vc step dual
                    nudSweepLength.Value = 7;
                    break;
                case "0203": // vc step fast
                    nudSweepLength.Value = 1;
                    break;
                case "phase":
                    nudSweepLength.Value = 5;
                    break;
                default:
                    nudSweepLength.BackColor = SystemColors.Window;
                    nudSweep.BackColor = SystemColors.Window;
                    nudScale.BackColor = SystemColors.Window;
                    lblProtocol.ForeColor = Color.Red;
                    nudSweepLength.Value = 10;
                    break;
            }
            this.Refresh();
            Application.DoEvents();
        }

        private void btnFromProto_Click(object sender, EventArgs e)
        {
            LoadSettingsFromProtocol();
        }

        private void tbPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbABFs_KeyPress(object sender, KeyPressEventArgs e)
        {
            LoadSettingsFromProtocol();
        }

        private void nudScale_ValueChanged(object sender, EventArgs e)
        {
            ReadABFAndPlot();
        }

        private void scottPlotUC1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                ReadABFAndPlot();
            }
            catch { }
        }
    }
}
