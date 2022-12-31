using System.Data;
using System.Runtime.Serialization;

namespace WinForms_Demo;

public partial class MainMenuForm : Form
{
    private readonly Dictionary<string, Type> Demos = DemoWindows.GetDemoTypesByTitle();

    public MainMenuForm()
    {
        InitializeComponent();
        label2.Text = ScottPlot.Version.VersionString;
    }

    private void MainMenuForm_Load(object sender, EventArgs e)
    {
        int initialWidth = Width;

        int nextItemPositionY = 100;
        int paddingBetweenItems = 10;
        int itemHeight = 83;

        IDemoWindow[] demoForms = Demos.Values.Select(x => (IDemoWindow)FormatterServices.GetUninitializedObject(x)).ToArray();

        foreach (IDemoWindow demoForm in demoForms)
        {
            MenuItem item = new(demoForm, Demos[demoForm.Title])
            {
                Location = new Point(12, nextItemPositionY),
                Size = new Size(initialWidth - 55, itemHeight),
            };

            nextItemPositionY += itemHeight + paddingBetweenItems;

            Controls.Add(item);
        }
    }
}
