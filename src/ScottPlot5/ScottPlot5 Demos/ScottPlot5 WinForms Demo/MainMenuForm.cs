using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace WinForms_Demo;

public partial class MainMenuForm : Form
{
    private readonly Dictionary<string, Type> Demos = DemoWindows.GetDemoTypesByTitle();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
    private enum ScrollBarDirection { HORZ = 0, VERT = 1, CTL = 2, BOTH = 3 }

    public MainMenuForm()
    {
        InitializeComponent();
        label2.Text = ScottPlot.Version.VersionString;
        ShowScrollBar(tableLayoutPanel1.Handle, (int)ScrollBarDirection.VERT, true);
    }

    private void MainMenuForm_Load(object sender, EventArgs e)
    {
        IDemoWindow[] demos = Demos.Values.Select(x => (IDemoWindow)FormatterServices.GetUninitializedObject(x)).ToArray();

        foreach (IDemoWindow demo in demos)
            AddLauncherRow(demo);

        AddBlankRow();
    }

    private void AddBlankRow(int height = 300)
    {
        int rowIndex = tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Panel panel = new() { Height = height };
        panel.Dock = DockStyle.Fill;
        tableLayoutPanel1.Controls.Add(panel, 0, rowIndex);
    }

    private void AddLauncherRow(IDemoWindow demoForm)
    {
        int rowIndex = tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        DemoWindowInfo info = new(demoForm, Demos[demoForm.Title]);
        info.Margin = new(10, 10, 10, 10);
        info.Dock = DockStyle.Fill;
        tableLayoutPanel1.Controls.Add(info, 0, rowIndex);
    }
}
