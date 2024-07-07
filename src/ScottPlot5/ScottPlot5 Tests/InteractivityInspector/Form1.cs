using ScottPlot.Interactivity;
using System.Diagnostics;

namespace Interactivity_Inspector;

public partial class Form1 : Form
{
    readonly UserInputProcessor UserInputProcessor;

    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());

        // disable the default interaction system
        formsPlot1.Interaction.Disable();

        // setup an experimental interaction system
        UserInputProcessor = new(formsPlot1.Plot);

        formsPlot1.MouseMove += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            var userInput = new ScottPlot.Interactivity.DefaultInputs.MouseMove(mousePixel);
            var actionResults = UserInputProcessor.Add(userInput);
            DisplayActionResults(actionResults);
        };

        formsPlot1.MouseDown += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            var userInput = new ScottPlot.Interactivity.DefaultInputs.LeftMouseDown(mousePixel);
            var actionResults = UserInputProcessor.Add(userInput);
            DisplayActionResults(actionResults);
        };

        formsPlot1.MouseUp += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            var userInput = new ScottPlot.Interactivity.DefaultInputs.LeftMouseUp(mousePixel);
            var actionResults = UserInputProcessor.Add(userInput);
            DisplayActionResults(actionResults);
        };
    }

    private void DisplayActionResults(UserActionResult[] actionResults)
    {
        foreach (UserActionResult actionResult in actionResults)
        {
            Debug.WriteLine(actionResult);
        }
    }
}
