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

        // disable the default interaction system and use the new one
        formsPlot1.Interaction.Disable();
        formsPlot1.UserInputProcessor.IsEnabled = true;
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
