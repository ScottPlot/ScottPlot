using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ScottPlot.Control;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace ScottPlot.WPF;

[System.ComponentModel.ToolboxItem(true)] 
[System.ComponentModel.DesignTimeVisible(true)]
public partial class WpfPlot : UserControl, IPlotControl
{
    public Plot Plot { get; } = new();

    public Interaction Interaction { get; private set; }

    public GRContext GRContext => SKElement.GRContext;

    public WpfPlot()
    {
        InitializeComponent();
        Interaction = new(this)
        {
            ContextMenuItems = GetDefaultContextMenuItems()
        };
    }

    private ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new() { Label = "Save Image", OnInvoke = OpenSaveImageDialog };
        ContextMenuItem copyImage = new() { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

        return new ContextMenuItem[] { saveImage, copyImage };
    }

    private ContextMenu GetContextMenu()
    {
        ContextMenu menu = new();
        foreach (var curr in Interaction.ContextMenuItems)
        {
            var menuItem = new MenuItem { Header = curr.Label };
            menuItem.Click += (s, e) => curr.OnInvoke();

            menu.Items.Add(menuItem);
        }

        return menu;
    }

    public void Replace(Interaction interaction)
    {
        Interaction = interaction;
    }

    public void Refresh()
    {
        SKElement.InvalidateVisual();
    }

    public void ShowContextMenu(Pixel position)
    {
        var menu = GetContextMenu();
        menu.PlacementTarget = this;
        menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
        menu.IsOpen = true;
    }

    private void OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        Plot.Render(e.Surface);
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Keyboard.Focus(this);

        Interaction.MouseDown(e.Pixel(this), e.ToButton());

        (sender as UIElement)?.CaptureMouse();

        if (e.ClickCount == 2)
        {
            Interaction.DoubleClick();
        }

        base.OnMouseDown(e);
    }

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(this), e.ToButton());
        (sender as UIElement)?.ReleaseMouseCapture();
        base.OnMouseUp(e);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel(this));
        base.OnMouseMove(e);
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(this), e.Delta);
        base.OnMouseWheel(e);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.Key());
        base.OnKeyDown(e);
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.Key());
        base.OnKeyUp(e);
    }

    private void OpenSaveImageDialog()
    {
        SaveFileDialog dialog = new()
        {
            FileName = Interaction.DefaultSaveImageFilename,
            Filter = "PNG Files (*.png)|*.png" +
                     "|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                     "|BMP Files (*.bmp)|*.bmp" +
                     "|WebP Files (*.webp)|*.webp" +
                     "|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() is true)
        {
            // TODO: launch a pop-up window indicating if extension is invalid or save failed
            ImageFormat format = ImageFormatLookup.FromFilePath(dialog.FileName);
            Plot.Save(dialog.FileName, (int)ActualWidth, (int)ActualHeight, format);
        }
    }

    private void CopyImageToClipboard()
    {
        BitmapImage bmp = new();
        bmp.BeginInit();
        byte[] bytes = Plot.GetImage((int)ActualWidth, (int)ActualHeight).GetImageBytes();
        using MemoryStream ms = new(bytes);
        bmp.StreamSource = ms;
        bmp.EndInit();
        bmp.Freeze();

        Clipboard.SetImage(bmp);
    }
}
