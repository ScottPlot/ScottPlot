using ScottPlot.Interactivity;
using ScottPlot.WinForms;

namespace Interactivity_Inspector;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());
        formsPlot1.Plot.Title("IPlotControl.UserInputProcessor (default)");

        formsPlot2.Plot.Add.Signal(ScottPlot.Generate.Sin());
        formsPlot2.Plot.Add.Signal(ScottPlot.Generate.Cos());
        formsPlot2.Plot.Title("IPlotControl.Interaction (old, opt-in)");

#pragma warning disable CS0618
        formsPlot2.Interaction.Enable();
#pragma warning restore CS0618
    }

    /// <summary>
    /// Required because arrow key presses do not invoke KeyDown
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        Key key = keyData.GetKey();
        if (StandardKeys.IsArrowKey(key))
        {
            IUserAction keyDownAction = new ScottPlot.Interactivity.UserActions.KeyDown(key);
            formsPlot1.UserInputProcessor.Process(keyDownAction);
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}
