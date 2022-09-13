using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms_Demo;

namespace ScottPlot5_WinForms_Demo
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            Text = "ScottPlot Demo";
            Size = new(500, 600);
            MinimumSize = new(500, 600);
            Load += Form3_Load;
        }

        private async void Form3_Load(object? sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();
            webView21.NavigateToString(HtmlMenu.GetHtml());
            webView21.WebMessageReceived += WebView21_WebMessageReceived;
        }

        private void WebView21_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            Text = e.WebMessageAsJson.Trim('"');
        }
    }
}
