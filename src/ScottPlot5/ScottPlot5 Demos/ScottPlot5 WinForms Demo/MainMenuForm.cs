namespace WinForms_Demo;

public partial class MainMenuForm : Form
{

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
        Text = $"{ScottPlot.Version.LongString} Demo";

        tbSearch.TextChanged += (s, e) => demoWindowScrollList1.Update(tbSearch.Text, null);
        Shown += (s, e) => demoWindowScrollList1.Update(tbSearch.Text, null);
    }
}
