namespace ScottPlot.Maui;

public class MauiPlotMenu : IPlotMenu
{
    private readonly MauiPlot ThisControl;
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();

    public MauiPlotMenu(MauiPlot plot)
    {
        ThisControl = plot;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {

        ContextMenuItem saveImage = new()
        {
            Label = "Save Image",
            OnInvoke = SaveImageDialog,
        };

        // TODO: ContextMenuItem copyImage

        ContextMenuItem autoscale = new()
        {
            Label = "Autoscale",
            OnInvoke = Autoscale,
        };

        ContextMenuItem zoomIn = new()
        {
            Label = "Zoom In",
            OnInvoke = ZoomIn,
        };

        ContextMenuItem zoomOut = new()
        {
            Label = "Zoom Out",
            OnInvoke = ZoomOut,
        };


        return new ContextMenuItem[] {
            saveImage,
            new() { IsSeparator = true },
            autoscale,
            zoomIn,
            zoomOut,
        };
    }

    public async void SaveImageDialog(Plot plot)
    {
        Page? page = Application.Current?.MainPage;
        if (page == null) return;

        string[] formats = [
            "PNG Files .png",
            "JPEG Files .jpg",
            "BMP Files .bmp",
            "WebP Files .webp",
            "SVG Files .svg"
        ];

        try
        {
            var format = await page.DisplayActionSheet("Select a format", null, null, formats);
            if (string.IsNullOrEmpty(format)) return;
            ImageFormat imageFormat = ImageFormats.FromFilename(format);

            string tempFileName = $"Plot_{DateTime.Now:yyyyMMdd_HHmmss}";
            var name = await page.DisplayPromptAsync("File name", "Enter the file name", placeholder: tempFileName);
            if (string.Equals(name, "Cancel")) return;
            if (string.IsNullOrEmpty(name)) name = tempFileName;

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
            plot.Save(Path.Combine(folder, $"{name}{format}"), (int)lastRenderSize.Width, (int)lastRenderSize.Height, imageFormat);

            await page.DisplayAlert("Success", $"Image saved to {folder}", "OK");
        }

        catch (Exception)
        {
            await page.DisplayAlert("Error", $"Image save failed", "OK");
            return;
        }

    }

    public void Autoscale(Plot plot)
    {
        plot.Axes.AutoScale();
        ThisControl.Refresh();
    }

    public void ZoomIn(Plot plot)
    {
        plot.Axes.Zoom(1.5, 1.5);
        ThisControl.Refresh();
    }

    public void ZoomOut(Plot plot)
    {
        plot.Axes.Zoom(0.5, 0.5);
        ThisControl.Refresh();
    }

    public MenuFlyout GetContextMenu(Plot plot)
    {
        MenuFlyout flyout = [];

        foreach (var item in ContextMenuItems)
        {
            if (item.IsSeparator)
            {
                flyout.Add(new MenuFlyoutSeparator());
            }
            else
            {
                var menuItem = new MenuFlyoutItem { Text = item.Label };
                menuItem.Clicked += (s, e) => item.OnInvoke(plot);
                flyout.Add(menuItem);
            }
        }

        return flyout;
    }

    public void ShowContextMenu(Pixel pixel)
    {
        Plot? plot = ThisControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;
        MenuFlyout flyout = GetContextMenu(plot);
        FlyoutBase.SetContextFlyout(ThisControl, flyout);
    }

    public void Add(string Label, Action<Plot> action)
    {
        ContextMenuItems.Add(new ContextMenuItem() { Label = Label, OnInvoke = action });
    }

    public void AddSeparator()
    {
        ContextMenuItems.Add(new ContextMenuItem() { IsSeparator = true });
    }

    public void Clear()
    {
        ContextMenuItems.Clear();
    }

    public void Reset()
    {
        Clear();
        ContextMenuItems.AddRange(GetDefaultContextMenuItems());
    }
}
