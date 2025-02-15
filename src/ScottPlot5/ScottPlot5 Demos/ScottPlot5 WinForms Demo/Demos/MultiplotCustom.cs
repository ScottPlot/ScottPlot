namespace WinForms_Demo.Demos;
public partial class MultiplotCustom : Form, IDemoWindow
{
    public string Title => "Custom Multiplot System";

    public string Description => "Although the default Multiplot is suitable for most applications, " +
        "advanced users may achieve extreme control by creating a class that implements IMultiplot.";

    public MultiplotCustom()
    {
        InitializeComponent();

        //formsPlot1.Multiplot = new CustomMultiplot();
    }
}
