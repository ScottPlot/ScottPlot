namespace ScottPlot.Interactivity.UserInputResponses;

public class ArrowKeyPanAndZoom : IUserInputResponse
{
    public float ShiftPanMultiplier = 5;
    public float DeltaPan { get; set; } = 20;
    public double DeltaZoomIn { get; set; } = 0.85f;
    public double DeltaZoomOut { get; set; } = 1.15f;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is UserInputs.KeyDown keyDown)
        {
            Debug.WriteLine(keys);
            if (keys.IsPressed(StandardKeys.Alt))
            {
                if (keyDown.Key is Keys.LeftKey) return ApplyZoom(plot, DeltaZoomIn, 1);
                else if (keyDown.Key is Keys.RightKey) return ApplyZoom(plot, DeltaZoomOut, 1);
                else if (keyDown.Key is Keys.DownKey) return ApplyZoom(plot, 1, DeltaZoomIn);
                else if (keyDown.Key is Keys.UpKey) return ApplyZoom(plot, 1, DeltaZoomOut);
                else return UserInputResponseResult.NoActionTaken;
            }
            else
            {
                float multiplier = keys.IsPressed(StandardKeys.Shift) ? ShiftPanMultiplier : 1;
                if (keyDown.Key is Keys.LeftKey) return ApplyPan(plot, -DeltaPan * multiplier, 0);
                else if (keyDown.Key is Keys.RightKey) return ApplyPan(plot, DeltaPan * multiplier, 0);
                else if (keyDown.Key is Keys.DownKey) return ApplyPan(plot, 0, -DeltaPan * multiplier);
                else if (keyDown.Key is Keys.UpKey) return ApplyPan(plot, 0, DeltaPan * multiplier);
                else return UserInputResponseResult.NoActionTaken;
            }
        }

        return UserInputResponseResult.NoActionTaken;
    }

    private UserInputResponseResult ApplyPan(Plot plot, float dX, float dY)
    {
        PixelOffset pxOffset = new(dX, dY);
        plot.Axes.Pan(pxOffset);

        return new UserInputResponseResult()
        {
            Summary = $"Applied pan X={dX}, y={dY}",
            RefreshRequired = true,
        };
    }

    private UserInputResponseResult ApplyZoom(Plot plot, double dX, double dY)
    {
        plot.Axes.Zoom(dX, dY);

        return new UserInputResponseResult()
        {
            Summary = $"Applied zoom X={dX}, y={dY}",
            RefreshRequired = true,
        };
    }
}
