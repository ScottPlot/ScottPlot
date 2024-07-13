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
            IUserAction userInput = new ScottPlot.Interactivity.UserActions.MouseMove(mousePixel);
            ProcessInput(userInput);
        };

        formsPlot1.MouseDown += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            IUserAction? userInput = e.Button switch
            {
                MouseButtons.Left => new ScottPlot.Interactivity.UserActions.LeftMouseDown(mousePixel),
                MouseButtons.Right => new ScottPlot.Interactivity.UserActions.RightMouseDown(mousePixel),
                MouseButtons.Middle => new ScottPlot.Interactivity.UserActions.MiddleMouseDown(mousePixel),
                _ => null,
            };
            ProcessInput(userInput);
        };

        formsPlot1.MouseUp += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);
            IUserAction? userInput = e.Button switch
            {
                MouseButtons.Left => new ScottPlot.Interactivity.UserActions.LeftMouseUp(mousePixel),
                MouseButtons.Right => new ScottPlot.Interactivity.UserActions.RightMouseUp(mousePixel),
                MouseButtons.Middle => new ScottPlot.Interactivity.UserActions.MiddleMouseUp(mousePixel),
                _ => null,
            };
            ProcessInput(userInput);
        };

        formsPlot1.MouseWheel += (s, e) =>
        {
            ScottPlot.Pixel mousePixel = new(e.X, e.Y);

            IUserAction userInput = e.Delta > 0
                ? new ScottPlot.Interactivity.UserActions.MouseWheelUp(mousePixel)
                : new ScottPlot.Interactivity.UserActions.MouseWheelDown(mousePixel);

            ProcessInput(userInput);
        };

        formsPlot1.KeyDown += (s, e) =>
        {
            Key key = GetKeyCode(e.KeyCode);
            IUserAction userInput = new ScottPlot.Interactivity.UserActions.KeyDown(key);
            ProcessInput(userInput);
        };

        formsPlot1.KeyUp += (s, e) =>
        {
            Key key = GetKeyCode(e.KeyCode);
            IUserAction userInput = new ScottPlot.Interactivity.UserActions.KeyUp(key);
            ProcessInput(userInput);
        };
    }

    private Key GetKeyCode(Keys keyCode)
    {
        // strip modifiers
        keyCode = keyCode & ~Keys.Modifiers;

        Key key = keyCode switch
        {
            Keys.Alt => StandardKeys.Alt,
            Keys.Menu => StandardKeys.Alt,
            Keys.Shift => StandardKeys.Shift,
            Keys.ShiftKey => StandardKeys.Shift,
            Keys.LShiftKey => StandardKeys.Shift,
            Keys.RShiftKey => StandardKeys.Shift,
            Keys.Control => StandardKeys.Control,
            Keys.ControlKey => StandardKeys.Control,
            Keys.Down => StandardKeys.Down,
            Keys.Up => StandardKeys.Up,
            Keys.Left => StandardKeys.Left,
            Keys.Right => StandardKeys.Right,
            _ => StandardKeys.Unknown,
        };

        if (key != StandardKeys.Unknown)
            return key;

        string keyName = keyCode.ToString();
        return (keyName.Length == 1)
            ? new Key(keyName)
            : new Key($"Unknown modifier key {keyName}");
    }

    /// <summary>
    /// Required because arrow key presses do not invoke KeyDown
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        Key key = GetKeyCode(keyData);
        if (StandardKeys.IsArrowKey(key))
        {
            IUserAction userInput = new ScottPlot.Interactivity.UserActions.KeyDown(key);
            ProcessInput(userInput);
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void ProcessInput(IUserAction? userInput)
    {
        if (userInput is null)
            return;

        var results = UserInputProcessor.Process(userInput);

        foreach (PlotResponseResult result in results)
        {
            if (result.Summary is null)
                continue;

            if (result.Summary.StartsWith("left click drag pan in progress")) continue;
            if (result.Summary.StartsWith("right click drag zoom in progress")) continue;
            if (result.Summary.StartsWith("left click drag pan ignored KeyDown")) continue;
            if (result.Summary.StartsWith("left click drag pan ignored KeyUp")) continue;
            Debug.WriteLine($"[{DateTime.Now}] {result}");
        }
    }
}
