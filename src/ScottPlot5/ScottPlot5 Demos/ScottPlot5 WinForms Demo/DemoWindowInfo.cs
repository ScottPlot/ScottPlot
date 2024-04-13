using System.Runtime.InteropServices;

namespace WinForms_Demo;

public partial class DemoWindowInfo : UserControl
{
    [DllImport("user32.dll")]
    static extern bool HideCaret(IntPtr hWnd);

    readonly Type FormType;

    public DemoWindowInfo(IDemoWindow demoForm, Type formType)
    {
        InitializeComponent();
        FormType = formType;
        richTextBox1.Click += (s, e) => HideCaret(richTextBox1.Handle);
        groupBox1.Text = demoForm.Title;
        richTextBox1.Text = demoForm.Description;
        button1.Click += (s, e) => LaunchDemoWindow(demoForm.Title);
        label1.Cursor = Cursors.Hand;
        label1.MouseEnter += (s, e) => { label1.ForeColor = System.Drawing.Color.Blue; };
        label1.MouseLeave += (s, e) => { label1.ForeColor = SystemColors.ControlDark; };
        label1.Click += (s, e) => LaunchSourceBrowser();
        HideCaret(richTextBox1.Handle);
        richTextBox1.Enabled = false;
    }

    private void LaunchSourceBrowser()
    {
        string folder = "https://github.com/ScottPlot/ScottPlot/tree/main/src/ScottPlot5/ScottPlot5%20Demos/ScottPlot5%20WinForms%20Demo/Demos/";
        string filename = FormType.Name + ".cs";
        string url = folder + filename;
        ScottPlot.Platform.LaunchWebBrowser(url);
    }

    private void LaunchDemoWindow(string title)
    {
        Form form = (Form)Activator.CreateInstance(FormType)!;
        form.Icon = Properties.Resources.scottplot_icon_rounded_border;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Text = title;
        ParentForm.Hide();
        form.ShowDialog();
        ParentForm.Show();
    }
}
