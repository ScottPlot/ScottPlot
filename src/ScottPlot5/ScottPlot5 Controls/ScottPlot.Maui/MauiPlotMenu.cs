using ScottPlot.Control;

namespace ScottPlot.Maui;

public class MauiPlotMenu : IPlotMenu
{
    private readonly MauiPlot MauiPlot;
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();

    public MauiPlotMenu(MauiPlot plot)
    {
        MauiPlot = plot;
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

    public async void SaveImageDialog(IPlotControl plotControl)
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
            ImageFormat imgformat = ImageFormats.FromFilename(format);


            string tempFileName = $"Plot_{DateTime.Now:yyyyMMdd_HHmmss}";
            var name = await page.DisplayPromptAsync("File name", "Enter the file name", placeholder: tempFileName);
            if (string.Equals(name, "Cancel")) return;
            if (string.IsNullOrEmpty(name)) name = tempFileName;


            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
            plotControl.Plot.Save(Path.Combine(folder, $"{name}{format}"), (int)lastRenderSize.Width, (int)lastRenderSize.Height, imgformat);

            await page.DisplayAlert("Success", $"Image saved to {folder}", "OK");
        }

        catch (Exception)
        {
            await page.DisplayAlert("Error", $"Image save failed", "OK");
            return;
        }

    }

    public void Autoscale(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.AutoScale();
        plotControl.Refresh();
    }

    public void ZoomIn(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.Zoom(1.5, 1.5);
        plotControl.Refresh();
    }

    public void ZoomOut(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.Zoom(0.5, 0.5);
        plotControl.Refresh();
    }

    public MenuFlyout GetContextMenu(IPlotControl plotControl)
    {
        MenuFlyout flyout = new();

        foreach (var item in ContextMenuItems)
        {
            if (item.IsSeparator)
            {
                flyout.Add(new MenuFlyoutSeparator());
            }
            else
            {
                var menuItem = new MenuFlyoutItem { Text = item.Label };
                menuItem.Clicked += (s, e) => item.OnInvoke(plotControl);
                flyout.Add(menuItem);
            }
        }

        return flyout;
    }

    public void ShowContextMenu(Pixel pixel)
    {
        MenuFlyout flyout = GetContextMenu(MauiPlot);
        FlyoutBase.SetContextFlyout(MauiPlot, flyout);
    }

    public void Add(string Label, Action<IPlotControl> action)
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
