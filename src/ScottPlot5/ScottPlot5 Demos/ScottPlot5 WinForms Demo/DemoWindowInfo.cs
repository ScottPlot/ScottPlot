using System.Runtime.InteropServices;

using WinForms_Demo.Properties;

namespace WinForms_Demo;

public partial class DemoWindowInfo : UserControl
{
    [DllImport("user32.dll")]
    static extern bool HideCaret(IntPtr hWnd);

    readonly Bitmap ImageOriginal;
    readonly Bitmap ImageFaded;
    readonly Type FormType;

    public DemoWindowInfo(IDemoWindow demoForm, Type formType)
    {
        InitializeComponent();
        FormType = formType;
        richTextBox1.Click += (s, e) => HideCaret(richTextBox1.Handle);
        groupBox1.Text = demoForm.Title;
        richTextBox1.Text = demoForm.Description;
        ImageOriginal = Resources.github_24;
        ImageFaded = GetFadedButtonImage();
        pictureBox1.Image = ImageFaded;
        pictureBox1.MouseEnter += (s, e) => pictureBox1.Image = ImageOriginal;
        pictureBox1.MouseLeave += (s, e) => pictureBox1.Image = ImageFaded;
        pictureBox1.MouseClick += (s, e) => LaunchSourceBrowser(formType);
        button1.Click += (s, e) => LaunchDemoWindow(demoForm.Title);
    }

    private void LaunchSourceBrowser(Type formType)
    {
        string folder = "https://github.com/ScottPlot/ScottPlot/tree/main/src/ScottPlot5/ScottPlot5%20Demos/ScottPlot5%20WinForms%20Demo/Demos/";
        string filename = formType.Name + ".cs";
        string url = folder + filename;
        ScottPlot.Platform.LaunchWebBrowser(url);
    }

    private Bitmap GetFadedButtonImage()
    {
        Rectangle rect = new(0, 0, pictureBox1.Width, pictureBox1.Height);
        Bitmap bmp = new(rect.Width, rect.Height);
        using Graphics gfx = Graphics.FromImage(bmp);
        gfx.Clear(SystemColors.Control);
        gfx.DrawImage(Resources.github_24, rect);
        using SolidBrush brush = new(Color.FromArgb(200, BackColor));
        gfx.FillRectangle(brush, rect);
        return bmp;
    }

    private void LaunchDemoWindow(string title)
    {
        Form form = (Form)Activator.CreateInstance(FormType)!;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Text = title;
        form.Width = 800;
        form.Height = 500;
        form.ShowDialog();
    }
}
