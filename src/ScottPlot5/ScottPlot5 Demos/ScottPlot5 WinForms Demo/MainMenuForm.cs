using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinForms_Demo;

public partial class MainMenuForm : Form
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
    private enum ScrollBarDirection { HORZ = 0, VERT = 1, CTL = 2, BOTH = 3 }

    private readonly HashSet<Keys> PressedKeys = new();

    public MainMenuForm(Type? launchFormType)
    {
        InitializeComponent();

        // launch a test window if CTRL+D is pressed at startup
        KeyPreview = true;
        KeyUp += (s, e) => PressedKeys.Remove(e.KeyCode);
        KeyDown += (s, e) =>
        {
            PressedKeys.Add(e.KeyCode);
            if (launchFormType is not null && PressedKeys.Contains(Keys.ControlKey) && PressedKeys.Contains(Keys.D))
            {
                Hide();
                Form form = (Form)Activator.CreateInstance(launchFormType)!;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowDialog();
                Close();
            }
        };

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
