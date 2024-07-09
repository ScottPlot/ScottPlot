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
            IUserInput userInput = new ScottPlot.Interactivity.UserInputs.MouseMove(mousePixel);
            ProcessInput(userInput);
        };

        formsPlot1.MouseDown += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            IUserInput? userInput = e.Button switch
            {
                MouseButtons.Left => new ScottPlot.Interactivity.UserInputs.LeftMouseDown(mousePixel),
                MouseButtons.Right => new ScottPlot.Interactivity.UserInputs.RightMouseDown(mousePixel),
                MouseButtons.Middle => new ScottPlot.Interactivity.UserInputs.MiddleMouseDown(mousePixel),
                _ => null,
            };
            ProcessInput(userInput);
        };

        formsPlot1.MouseUp += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            IUserInput? userInput = e.Button switch
            {
                MouseButtons.Left => new ScottPlot.Interactivity.UserInputs.LeftMouseUp(mousePixel),
                MouseButtons.Right => new ScottPlot.Interactivity.UserInputs.RightMouseUp(mousePixel),
                MouseButtons.Middle => new ScottPlot.Interactivity.UserInputs.MiddleMouseUp(mousePixel),
                _ => null,
            };
            ProcessInput(userInput);
        };

        formsPlot1.MouseWheel += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);

            IUserInput userInput = e.Delta > 0
                ? new ScottPlot.Interactivity.UserInputs.MouseWheelUp(mousePixel)
                : new ScottPlot.Interactivity.UserInputs.MouseWheelDown(mousePixel);

            ProcessInput(userInput);
        };

        formsPlot1.KeyDown += (s, e) =>
        {
            IKey key = GetKey(e.KeyCode);
            IUserInput userInput = new ScottPlot.Interactivity.UserInputs.KeyDown(key);
            ProcessInput(userInput);
        };

        formsPlot1.KeyUp += (s, e) =>
        {
            IKey key = GetKey(e.KeyCode);
            IUserInput userInput = new ScottPlot.Interactivity.UserInputs.KeyUp(key);
            ProcessInput(userInput);
        };
    }

    private IKey GetKey(Keys keyCode)
    {
        IKey? key = keyCode switch
        {
            Keys.Alt => StandardKeys.Alt,
            Keys.Menu => StandardKeys.Alt,
            Keys.Shift => StandardKeys.Shift,
            Keys.ShiftKey => StandardKeys.Shift,
            Keys.Control => StandardKeys.Control,
            Keys.ControlKey => StandardKeys.Control,
            Keys.Down => StandardKeys.Down,
            Keys.Up => StandardKeys.Up,
            Keys.Left => StandardKeys.Left,
            Keys.Right => StandardKeys.Right,
            _ => null,
        };

        if (key is not null)
            return key;

        string keyName = keyCode.ToString();
        if (keyName.Length == 1)
        {
            return new ScottPlot.Interactivity.Keys.CharacterKey()
            {
                Character = keyName[0]
            };
        }

        return new ScottPlot.Interactivity.Keys.CustomKey()
        {
            Name = $"Unknown key {keyName}",
        };
    }

    private void ProcessInput(IUserInput? userInput)
    {
        if (userInput is null)
            return;

        UserInputResponseResult[] results = UserInputProcessor.Process(userInput);

        foreach (UserInputResponseResult result in results)
        {
            if (result.Summary.StartsWith("left click drag pan in progress")) continue;
            if (result.Summary.StartsWith("right click drag zoom in progress")) continue;
            if (result.Summary.StartsWith("left click drag pan ignored KeyDown")) continue;
            if (result.Summary.StartsWith("left click drag pan ignored KeyUp")) continue;
            Debug.WriteLine(result);
        }
    }
}
