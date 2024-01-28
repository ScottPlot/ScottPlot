namespace ScottPlot;

public class DraggedSpan
{
    public readonly Plottables.HorizontalSpan? HSpan;
    public readonly Plottables.VerticalSpan? VSpan;
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

    public DraggedSpan(Plottables.HorizontalSpan hSpan, bool x1, bool x2, Coordinates cs)
    {
        HSpan = hSpan;
        X1 = x1;
        X2 = x2;
        MouseDownX1 = HSpan.X1;
        MouseDownX2 = HSpan.X2;
        MouseDownCoordiantes = cs;
    }

    public DraggedSpan(Plottables.VerticalSpan vSpan, bool y1, bool y2, Coordinates cs)
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

    public static DraggedSpan? GetThingUnderMouse(IPlotControl control, float mouseX, float mouseY)
    {
        CoordinateRect rect = control.Plot.GetCoordinateRect(mouseX, mouseY, radius: 10);

        foreach (var plottable in control.Plot.GetPlottables().Reverse())
        {
            if (plottable is Plottables.HorizontalSpan hSpan)
            {
                DraggedSpan? spanUnderMouse = CheckH(rect, hSpan);
                if (spanUnderMouse is not null)
                {
                    return spanUnderMouse;
                }
            }

            if (plottable is Plottables.VerticalSpan vSpan)
            {
                DraggedSpan? spanUnderMouse = CheckV(rect, vSpan);
                if (spanUnderMouse is not null)
                {
                    return spanUnderMouse;
                }
            }
        }

        return null;
    }

    private static DraggedSpan? CheckH(CoordinateRect rect, Plottables.HorizontalSpan hSpan)
    {
        if (rect.ContainsX(hSpan.X1))
        {
            return new DraggedSpan(hSpan, true, false, rect.Center);
        }
        else if (rect.ContainsX(hSpan.X2))
        {
            return new DraggedSpan(hSpan, false, true, rect.Center);
        }
        else if (rect.XRange.Intersects(hSpan.XRange))
        {
            return new DraggedSpan(hSpan, false, false, rect.Center);
        }
        else
        {
            return null;
        }
    }

    private static DraggedSpan? CheckV(CoordinateRect rect, Plottables.VerticalSpan vSpan)
    {
        if (rect.ContainsY(vSpan.Y1))
        {
            return new DraggedSpan(vSpan, true, false, rect.Center);
        }
        else if (rect.ContainsY(vSpan.Y2))
        {
            return new DraggedSpan(vSpan, false, true, rect.Center);
        }
        else if (rect.YRange.Intersects(vSpan.YRange))
        {
            return new DraggedSpan(vSpan, false, false, rect.Center);
        }
        else
        {
            return null;
        }
    }
}
