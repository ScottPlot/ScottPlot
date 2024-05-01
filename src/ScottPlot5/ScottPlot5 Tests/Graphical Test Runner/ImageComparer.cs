namespace GraphicalTestRunner;

public partial class ImageComparer : UserControl
{
    Bitmap? Bmp1;
    Bitmap? Bmp2;
    ScottPlot.Testing.ImageDiff? ImgDiff;
    int ImageMode = 0;

    private string Path1 = string.Empty;
    private string Path2 = string.Empty;
    Color SelectedBackgroundColor = System.Drawing.Color.FromArgb(50, SystemColors.ControlDark);

    public ImageComparer()
    {
        InitializeComponent();

        timer1.Enabled = !DesignMode;
        timer1.Tick += (s, e) => SwitchImages(1);
        timer1.Interval = 100;
        pictureBox1.MouseWheel += (s, e) => SwitchImages(e.Delta > 0 ? 1 : -1);

        checkBox1.CheckedChanged += (s, e) => timer1.Enabled = checkBox1.Checked;
        checkBox2.CheckedChanged += (s, e) => UpdateDiffBitmap();

        pictureBox1.DoubleClick += (s, e) => System.Diagnostics.Process.Start("explorer.exe", Path1!);
        pictureBox2.DoubleClick += (s, e) => System.Diagnostics.Process.Start("explorer.exe", Path2!);
    }

    private void SwitchImages(int delta)
    {
        ImageMode += delta;
        if (ImageMode % 2 == 0)
        {
            SetBitmap1();
        }
        else
        {
            SetBitmap2();
        }
    }

    private void SetBitmap1()
    {
        label1.BackColor = Bmp1 is not null ? SelectedBackgroundColor : SystemColors.Control;
        label2.BackColor = SystemColors.Control;
        pictureBox1.Image = Bmp1;
    }

    private void SetBitmap2()
    {
        label2.BackColor = Bmp2 is not null ? SelectedBackgroundColor : SystemColors.Control;
        label1.BackColor = SystemColors.Control;
        pictureBox1.Image = Bmp2;
    }

    private void UpdateDiffBitmap()
    {
        if (ImgDiff is null)
            return;

        ScottPlot.Image? diffImage = checkBox2.Checked
            ? ImgDiff.DifferenceImage!.GetAutoscaledImage()
            : ImgDiff.DifferenceImage;

        pictureBox2.Image = (diffImage is not null)
            ? ScottPlot.WinForms.FormsPlotExtensions.GetBitmap(diffImage)
            : null;
    }

    public void SetImages(string path1, string path2)
    {
        Path1 = path1;
        Path2 = path2;
        ScottPlot.Image? img1 = null;
        ScottPlot.Image? img2 = null;

        if (File.Exists(path1))
        {
            img1 = new(path1);
            Bmp1 = ScottPlot.WinForms.FormsPlotExtensions.GetBitmap(img1);
        }
        else
        {
            Bmp1 = null;
        }

        if (File.Exists(path2))
        {
            img2 = new(path2);
            Bmp2 = ScottPlot.WinForms.FormsPlotExtensions.GetBitmap(img2);
        }

        if (img1 is not null && img2 is not null)
        {
            ImgDiff = new(img1, img2);
        }
        else
        {
            ImgDiff = null;
        }

        SwitchImages(0);
        UpdateDiffBitmap();
        pictureBox1.BackColor = SystemColors.Control;
        pictureBox2.BackColor = SystemColors.Control;
    }
}
