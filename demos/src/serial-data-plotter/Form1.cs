using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace serial_data_plotter
{
    public partial class Form1 : Form
    {
        SerialPort ser;
        static List<string> lines = new List<string>();

        public Form1()
        {
            InitializeComponent();
            lblLastLine.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateSerialComboBoxes();
        }

        private void PopulateSerialComboBoxes()
        {
            cbPort.Items.Clear();
            cbPort.Items.AddRange(SerialPort.GetPortNames());
            cbPort.SelectedItem = cbPort.Items[cbPort.Items.Count - 1];

            cbBaud.Items.Clear();
            cbBaud.Items.Add("115200");
            cbBaud.SelectedItem = cbBaud.Items[cbBaud.Items.Count - 1];

            cbFlow.Items.Clear();
            cbFlow.Items.Add("None");
            cbFlow.Items.Add("RTS");
            cbFlow.SelectedItem = cbFlow.Items[cbFlow.Items.Count - 1];
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (ser == null || !ser.IsOpen)
            {
                string com = cbPort.SelectedItem.ToString();
                int baud = int.Parse(cbBaud.SelectedItem.ToString());

                ser = new SerialPort(com, baud);
                ser.Open();
                ser.ReadLine();
                ser.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                btnConnect.Text = "disconnect";
                lblStatus.Text = "connected";
                cbPort.Enabled = false;
                cbBaud.Enabled = false;
                cbFlow.Enabled = false;
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                ser.Close();
                btnConnect.Text = "connect";
                lblStatus.Text = "disconnected";
                cbPort.Enabled = true;
                cbBaud.Enabled = true;
                cbFlow.Enabled = true;
            }

        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string line = (sp.ReadExisting() + sp.ReadLine()).Trim();
                lines.Add(line);
            }
            catch (System.IO.IOException exc)
            {
                Console.WriteLine("IOException in serial data handler");
                Console.WriteLine(exc);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (lines.Count > 0)
            {
                lblLastLine.Text = lines.Last();
                lines.Clear();
            }
        }
    }
}
