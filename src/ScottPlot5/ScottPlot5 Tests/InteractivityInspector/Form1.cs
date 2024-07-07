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
            IUserInput userInput = new ScottPlot.Interactivity.DefaultInputs.MouseMove(mousePixel);
            ProcessInput(userInput);
        };

        formsPlot1.MouseDown += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            IUserInput? userInput = e.Button switch
            {
                MouseButtons.Left => new ScottPlot.Interactivity.DefaultInputs.LeftMouseDown(mousePixel),
                MouseButtons.Right => new ScottPlot.Interactivity.DefaultInputs.RightMouseDown(mousePixel),
                MouseButtons.Middle => new ScottPlot.Interactivity.DefaultInputs.MiddleMouseDown(mousePixel),
                _ => null,
            };
            ProcessInput(userInput);
        };

        formsPlot1.MouseUp += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            IUserInput? userInput = e.Button switch
            {
                MouseButtons.Left => new ScottPlot.Interactivity.DefaultInputs.LeftMouseUp(mousePixel),
                MouseButtons.Right => new ScottPlot.Interactivity.DefaultInputs.RightMouseUp(mousePixel),
                MouseButtons.Middle => new ScottPlot.Interactivity.DefaultInputs.MiddleMouseUp(mousePixel),
                _ => null,
            };
            ProcessInput(userInput);
        };

        formsPlot1.MouseWheel += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);

            IUserInput userInput = e.Delta > 0
                ? new ScottPlot.Interactivity.DefaultInputs.MouseWheelUp(mousePixel)
                : new ScottPlot.Interactivity.DefaultInputs.MouseWheelDown(mousePixel);

            ProcessInput(userInput);
        };

        formsPlot1.KeyDown += (s, e) =>
        {
            IUserInput userInput = new ScottPlot.Interactivity.DefaultInputs.KeyDown(e.KeyCode.ToString());
            ProcessInput(userInput);
        };

        formsPlot1.KeyUp += (s, e) =>
        {
            IUserInput userInput = new ScottPlot.Interactivity.DefaultInputs.KeyUp(e.KeyCode.ToString());
            ProcessInput(userInput);
        };
    }


    private void ProcessInput(IUserInput? userInput)
    {
        if (userInput is null)
            return;

        UserActionResult[] actionResults = UserInputProcessor.Add(userInput);

        foreach (UserActionResult actionResult in actionResults)
        {
            Debug.WriteLine(actionResult);
        }
    }
}
