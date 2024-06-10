namespace WinForms_Demo.Demos;

public partial class PersistingPlot : Form, IDemoWindow
{
    public string Title => "Persisting Plot";

    public string Description => "Manipulations to a Plot " +
        "on another Form persist through Close() events";

    private readonly ExamplePlotForm PersistentForm = new();

    public PersistingPlot()
    {
        InitializeComponent();

        PersistentForm.FormsPlot1.Plot.Add.Signal(ScottPlot.Generate.RandomWalk(100));

        button1.Click += (s, e) => PersistentForm.ShowDialog();
    }
}
