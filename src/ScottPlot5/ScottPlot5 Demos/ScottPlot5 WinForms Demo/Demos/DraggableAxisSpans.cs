using ScottPlot;
using System.Diagnostics;

namespace WinForms_Demo.Demos;

public partial class DraggableAxisSpans : Form, IDemoWindow
{
    public string Title => "Draggable Axis Spans";

    public string Description => "Demonstrates how to create a mouse-interactive " +
        "axis span that can be resized or dragged";

    ThingUnderMouse? PlottableBeingDragged = null;

    public DraggableAxisSpans()
    {
        InitializeComponent();

        // place axis spans on the plot
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());
        formsPlot1.Plot.Add.VerticalSpan(.23, .78);
        formsPlot1.Plot.Add.HorizontalSpan(23, 42);
        formsPlot1.Refresh();

        // use events for custom mouse interactivity
        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        PlottableBeingDragged = GetThingUnderMouse(e.X, e.Y);

        // disable pan/zoom while a plottable is being dragged
        if (PlottableBeingDragged is not null)
        {
            formsPlot1.Interaction.Disable();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        PlottableBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (PlottableBeingDragged is not null)
        {
            Coordinates cs = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
            PlottableBeingDragged.Update(cs);
            formsPlot1.Refresh();
        }
        else
        {
            ThingUnderMouse? thingUnderMouse = GetThingUnderMouse(e.X, e.Y);
            SetCursor(thingUnderMouse);
        }
    }

    public class ThingUnderMouse
    {
        public readonly ScottPlot.Plottables.HorizontalSpan? HSpan;
        public readonly ScottPlot.Plottables.VerticalSpan? VSpan;
        public readonly bool X1;
        public readonly bool X2;
        public readonly bool Y1;
        public readonly bool Y2;
        public readonly Coordinates MouseDownCoordiantes;
        public readonly double MouseDownX1;
        public readonly double MouseDownX2;
        public readonly double MouseDownY1;
        public readonly double MouseDownY2;
        public bool IsVerticalEdge => X1 || X2;
        public bool IsHorizontalEdge => Y1 || Y2;
        public bool IsEdge => IsVerticalEdge || IsHorizontalEdge;

        public ThingUnderMouse(ScottPlot.Plottables.HorizontalSpan hSpan, bool x1, bool x2, Coordinates cs)
        {
            HSpan = hSpan;
            X1 = x1;
            X2 = x2;
            MouseDownX1 = HSpan.X1;
            MouseDownX2 = HSpan.X2;
            MouseDownCoordiantes = cs;
        }

        public ThingUnderMouse(ScottPlot.Plottables.VerticalSpan vSpan, bool y1, bool y2, Coordinates cs)
        {
            VSpan = vSpan;
            Y1 = y1;
            Y2 = y2;
            MouseDownY1 = vSpan.Y1;
            MouseDownY2 = vSpan.Y2;
            MouseDownCoordiantes = cs;
        }

        public void Update(Coordinates mouse)
        {
            if (HSpan is not null)
            {
                if (X1) HSpan.X1 = mouse.X;
                else if (X2) HSpan.X2 = mouse.X;
                else
                {
                    double dX = mouse.X - MouseDownCoordiantes.X;
                    (HSpan.X1, HSpan.X2) = (MouseDownX1 + dX, MouseDownX2 + dX);
                }
            }
            if (VSpan is not null)
            {
                if (Y1) VSpan.Y1 = mouse.Y;
                else if (Y2) VSpan.Y2 = mouse.Y;
                else
                {
                    double dY = mouse.Y - MouseDownCoordiantes.Y;
                    (VSpan.Y1, VSpan.Y2) = (MouseDownY1 + dY, MouseDownY2 + dY);
                }
            }
        }
    }

    private void SetCursor(ThingUnderMouse? thingUnderMouse)
    {
        if (thingUnderMouse is null)
        {
            Cursor = Cursors.Default;
        }
        else if (thingUnderMouse.IsVerticalEdge)
        {
            Cursor = Cursors.SizeWE;
        }
        else if (thingUnderMouse.IsHorizontalEdge)
        {
            Cursor = Cursors.SizeNS;
        }
        else
        {
            Cursor = Cursors.SizeAll;
        }
    }

    private ThingUnderMouse? GetThingUnderMouse(float mouseX, float mouseY)
    {
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(mouseX, mouseY, radius: 10);

        foreach (var plottable in formsPlot1.Plot.GetPlottables().Reverse())
        {
            if (plottable is ScottPlot.Plottables.HorizontalSpan hSpan)
            {
                ThingUnderMouse? spanUnderMouse = CheckH(rect, hSpan);
                if (spanUnderMouse is not null)
                {
                    return spanUnderMouse;
                }
            }

            if (plottable is ScottPlot.Plottables.VerticalSpan vSpan)
            {
                ThingUnderMouse? spanUnderMouse = CheckV(rect, vSpan);
                if (spanUnderMouse is not null)
                {
                    return spanUnderMouse;
                }
            }
        }

        return null;
    }

    private ThingUnderMouse? CheckH(CoordinateRect rect, ScottPlot.Plottables.HorizontalSpan hSpan)
    {
        if (rect.ContainsX(hSpan.X1))
        {
            return new ThingUnderMouse(hSpan, true, false, rect.Center);
        }
        else if (rect.ContainsX(hSpan.X2))
        {
            return new ThingUnderMouse(hSpan, false, true, rect.Center);
        }
        else if (rect.XRange.Intersects(hSpan.XRange))
        {
            return new ThingUnderMouse(hSpan, false, false, rect.Center);
        }
        else
        {
            return null;
        }
    }

    private ThingUnderMouse? CheckV(CoordinateRect rect, ScottPlot.Plottables.VerticalSpan vSpan)
    {
        if (rect.ContainsY(vSpan.Y1))
        {
            return new ThingUnderMouse(vSpan, true, false, rect.Center);
        }
        else if (rect.ContainsY(vSpan.Y2))
        {
            return new ThingUnderMouse(vSpan, false, true, rect.Center);
        }
        else if (rect.YRange.Intersects(vSpan.YRange))
        {
            return new ThingUnderMouse(vSpan, false, false, rect.Center);
        }
        else
        {
            return null;
        }
    }
}
