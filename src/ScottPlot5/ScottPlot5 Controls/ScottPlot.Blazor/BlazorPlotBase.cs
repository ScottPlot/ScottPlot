using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ScottPlot.Control;
using SkiaSharp;

namespace ScottPlot.Blazor;

public abstract class BlazorPlotBase : ComponentBase, IPlotControl
{
    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public bool EnableRenderLoop { get; set; } = false;

    public Plot Plot { get; private set; } = new();

    public IPlotInteraction Interaction { get; set; }

    public IPlotMenu Menu { get; set; }

    public BlazorPlotBase()
    {
        HandlerPointerMoved += OnPointerMoved;
        HandlerPointerPressed += OnPointerPressed;
        HandlerPointerReleased += OnPointerReleased;
        HandlerDoubleTapped += OnDoubleTapped;
        HandlerPointerWheelChanged += OnPointerWheelChanged;
        HandlerKeyDown += OnKeyDown;
        HandlerKeyUp += OnKeyUp;

        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        Menu = new BlazorPlotMenu();
    }

    public GRContext? GRContext => null;

    public float DisplayScale { get; set; }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }

    public void Reset()
    {
        Plot plot = new();
        Reset(plot);
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        oldPlot?.Dispose();
    }

    public virtual void Refresh() { }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    public event EventHandler<PointerEventArgs> HandlerPointerMoved;

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        Interaction.OnMouseMove(CoordinateToPixel(e));
    }

    public void OnPointerMoved(PointerEventArgs e)
    {
        HandlerPointerMoved.Invoke(this, e);
    }

    public event EventHandler<PointerEventArgs> HandlerPointerPressed;

    private void OnPointerPressed(object? sender, PointerEventArgs e)
    {
        Interaction.MouseDown(CoordinateToPixel(e), MapToScottMouseButton(e));
    }

    public void OnPointerPressed(PointerEventArgs e)
    {
        HandlerPointerPressed.Invoke(this, e);
    }

    public event EventHandler<PointerEventArgs> HandlerPointerReleased;

    private void OnPointerReleased(object? sender, PointerEventArgs e)
    {
        Interaction.MouseUp(CoordinateToPixel(e), MapToScottMouseButton(e));
    }

    public void OnPointerReleased(PointerEventArgs e)
    {
        HandlerPointerReleased.Invoke(this, e);
    }

    public event EventHandler<MouseEventArgs> HandlerDoubleTapped;

    private void OnDoubleTapped(object? sender, MouseEventArgs e)
    {
        Interaction.DoubleClick();
    }
    public void OnDoubleTapped(MouseEventArgs e)
    {
        HandlerDoubleTapped.Invoke(this, e);
    }

    public event EventHandler<WheelEventArgs> HandlerPointerWheelChanged;

    public void OnPointerWheelChanged(object? sender, WheelEventArgs e)
    {
        Interaction.MouseWheelVertical(CoordinateToPixel(e), -(float)e.DeltaY);
    }

    public void OnPointerWheelChanged(WheelEventArgs e)
    {
        HandlerPointerWheelChanged.Invoke(this, e);
    }


    public event EventHandler<KeyboardEventArgs> HandlerKeyDown;

    public void OnKeyDown(object? sender, KeyboardEventArgs e)
    {
        Interaction.KeyDown(MapToScottKey(e.Key));

    }

    public void OnKeyDown(KeyboardEventArgs e)
    {
        HandlerKeyDown.Invoke(this, e);
    }

    public event EventHandler<KeyboardEventArgs> HandlerKeyUp;

    public void OnKeyUp(object? sender, KeyboardEventArgs e)
    {
        Interaction.KeyUp(MapToScottKey(e.Key));
    }

    public void OnKeyUp(KeyboardEventArgs e)
    {
        HandlerKeyUp.Invoke(this, e);
    }

    public Pixel CoordinateToPixel(WheelEventArgs args)
    {
        return new Pixel((float)args.OffsetX, (float)args.OffsetY);
    }

    public Pixel CoordinateToPixel(PointerEventArgs args)
    {
        return new Pixel((float)args.OffsetX, (float)args.OffsetY);
    }

    public MouseButton MapToScottMouseButton(MouseEventArgs args)
    {
        if (args.Button == 0)
        {
            return MouseButton.Left;
        }
        else if (args.Button == 1)
        {
            return MouseButton.Middle;
        }
        else if (args.Button == 2)
        {
            return MouseButton.Right;
        }
        else
        {
            return MouseButton.Unknown;
        }
    }

    public static Key MapToScottKey(string key)
    {
        switch (key)
        {
            case "Control":
                return Key.Ctrl;
            case "Alt":
                return Key.Alt;
            case "Shift":
                return Key.Shift;
            default:
                return Key.Unknown;
        }
    }
}
