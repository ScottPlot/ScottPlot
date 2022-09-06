namespace ScottPlot5_WinForms_Demo.Sandbox;

public partial class DeveloperTesting : Form, IDemoForm
{
    public string Title => "Basic Sandbox";

    public string Description => "This demo demonstrates how to create a simple demo";

    public DeveloperTesting()
    {
        InitializeComponent();
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin(51));
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos(51));
    }
}
