using Microsoft.Maui.Controls;
using ScottPlot.Control;
using System.Text;

namespace ScottPlot.Maui;

public class MauiPlotMenu : IPlotMenu
{
    private readonly MauiPlot MauiPlot;
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();

    public MauiPlotMenu(MauiPlot plot)
    {
        this.MauiPlot = plot;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {
        /*
        ContextMenuItem saveImage = new()
        {
            Label = "Save Image",
            OnInvoke = OpenSaveImageDialog,
        };
       

        TODO: Unable to assign an image to the clipboard using Clipboard.SetImage
        ContextMenuItem copyImage = new()
        {
            Label = "Copy to Clipboard",
            OnInvoke = CopyImageToClipboard,
        };
         */
        ContextMenuItem autoscale = new()
        {
            Label = "Autoscale",
            OnInvoke = Autoscale,
        };

        ContextMenuItem zoomIn = new()
        {
            Label = "ZoomIn",
            OnInvoke = ZoomIn,
        };

        ContextMenuItem zoomOut = new()
        {
            Label = "ZoomOut",
            OnInvoke = ZoomOut,
        };


        return new ContextMenuItem[] {
            //saveImage,
            //copyImage,
            autoscale,
            zoomIn,
            zoomOut,
        };
    }

    private void ZoomOut(IPlotControl plotControl)
    {
        plotControl.Interaction.MouseWheelVertical(new Pixel(MauiPlot.Width / 2, MauiPlot.Height / 2), -1);
    }

    private void ZoomIn(IPlotControl plotControl)
    {
        plotControl.Interaction.MouseWheelVertical(new Pixel(MauiPlot.Width / 2, MauiPlot.Height / 2), 1);

    }

    public MenuFlyout GetContextMenu(IPlotControl plotControl)
    {
        MenuFlyout flyout = new();

        foreach (var curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                flyout.Add(new MenuFlyoutSeparator());
            }
            else
            {
                var menuItem = new MenuFlyoutItem { Text = curr.Label };
                menuItem.Clicked += (s, e) => curr.OnInvoke(plotControl);
                flyout.Add(menuItem);
            }
        }

        return flyout;
    }

    public void OpenSaveImageDialog(IPlotControl plotControl)
    {

    }

    public void CopyImageToClipboard(IPlotControl plotControl)
    {
        /* TODO
        PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
        Image bmp = plotControl.Plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height);
        byte[] bmpBytes = bmp.GetImageBytes();

        Clipboard.SetTextAsync(Encoding.UTF8.GetString(bmpBytes, 0, bmpBytes.Length));
        */
    }

    public void Autoscale(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.AutoScale();
        plotControl.Refresh();
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
