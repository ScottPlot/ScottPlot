using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms_Demo;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScottPlot5_WinForms_Demo
{
    public partial class LauncherForm : Form
    {
        private readonly Dictionary<string, Type> Demos = DemoWindows.GetDemoTypesByTitle();

        public LauncherForm()
        {
            InitializeComponent();
            Text = "ScottPlot Demo";
            Size = new(500, 600);
            MinimumSize = new(500, 600);
            Load += Form3_Load;
            webView21.WebMessageReceived += WebView21_WebMessageReceived;
        }

        private async void Form3_Load(object? sender, EventArgs e)
        {
            DemoLauncher.HtmlMenu menu = new(ScottPlot.Version.VersionString);
            foreach (var demoType in Demos.Values)
            {
                IDemoForm demo = (IDemoForm)FormatterServices.GetUninitializedObject(demoType);
                menu.Add(demo.Title, demo.Description);
            }

            await webView21.EnsureCoreWebView2Async();
            webView21.NavigateToString(menu.ToString());
        }

        private void WebView21_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            string title = e.WebMessageAsJson.Trim('"');
            Type formType = Demos[title];
            Form form = (Form)Activator.CreateInstance(formType)!;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = title;
            form.Width = 800;
            form.Height = 500;
            form.ShowDialog();
        }
    }
}
