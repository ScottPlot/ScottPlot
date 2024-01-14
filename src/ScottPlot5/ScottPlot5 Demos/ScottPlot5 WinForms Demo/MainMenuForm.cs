using System.Runtime.InteropServices;

namespace WinForms_Demo;

public partial class MainMenuForm : Form
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
    private enum ScrollBarDirection { HORZ = 0, VERT = 1, CTL = 2, BOTH = 3 }

    public MainMenuForm()
    {
        InitializeComponent();
        label1.Text = "ScottPlot Demo";
        label2.Text = "ScottPlot.WinForms " + ScottPlot.Version.VersionString;
        ShowScrollBar(tableLayoutPanel1.Handle, (int)ScrollBarDirection.VERT, true);
        DemoWindows.GetDemoWindows().ForEach(AddLauncherRow);
        AddBlankRow();
        Text = $"{ScottPlot.Version.LongString} Demo";
    }

    private void AddLauncherRow(IDemoWindow demoForm)
    {
        int rowIndex = tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        DemoWindowInfo info = new(demoForm, demoForm.GetType())
        {
            Margin = new(10, 10, 10, 10),
            Dock = DockStyle.Fill
        };
        tableLayoutPanel1.Controls.Add(info, 0, rowIndex);
    }

    private void AddBlankRow(int height = 300)
    {
        int rowIndex = tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Panel panel = new()
        {
            Height = height,
            Dock = DockStyle.Fill
        };
        tableLayoutPanel1.Controls.Add(panel, 0, rowIndex);
    }
}
