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
        static string serLastLine;
        static AdcValuesLoop values = new AdcValuesLoop();

        public Form1()
        {
            InitializeComponent();
            lblLastLine.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateSerialComboBoxes();

            scottPlotUC1.plt.PlotSignal(values.values1, 50, markerSize: 0, label: "ADC 1");
            scottPlotUC1.plt.PlotSignal(values.values2, 50, markerSize: 0, label: "ADC 2");
            scottPlotUC1.plt.PlotSignal(values.values3, 50, markerSize: 0, label: "ADC 3");
            scottPlotUC1.plt.PlotSignal(values.values4, 50, markerSize: 0, label: "ADC 4");
            scottPlotUC1.plt.Title("Sensor Data");
            scottPlotUC1.plt.YLabel("ADC Value");
            scottPlotUC1.plt.XLabel("Time (seconds)");
            scottPlotUC1.plt.Legend();
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.plt.Axis(y2: 32000);
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
                values.Clear();

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
                lblLastLine.Enabled = true;
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
                lblLastLine.Enabled = false;
            }

        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string line = (sp.ReadExisting() + sp.ReadLine()).Trim();
                serLastLine = line;
                values.ParseCsvLine(line);
            }
            catch (System.IO.IOException exc)
            {
                Console.WriteLine("IOException in serial data handler");
                Console.WriteLine(exc);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblLastLine.Text = serLastLine;
            scottPlotUC1.plt.Clear(scatterPlots: false, signalPlots: false);
            scottPlotUC1.plt.PlotVLine(values.nextIndex / 50.0, color: Color.Red, lineWidth: 2);
            scottPlotUC1.Render();
        }
    }
}
