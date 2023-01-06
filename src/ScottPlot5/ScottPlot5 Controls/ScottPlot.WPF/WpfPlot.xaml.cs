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
using SkiaSharp.Views.Desktop;

namespace ScottPlot.WPF
{
    /// <summary>
    /// Interaction logic for WpfPlot.xaml
    /// </summary>
    public partial class WpfPlot : UserControl, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Interaction Interaction { get; private set; }

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
            ContextMenuItem saveImage = new() { Header = "Save Image", OnInvoke = OpenSaveImageDialog };
            ContextMenuItem copyImage = new() { Header = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

            return new ContextMenuItem[] { saveImage, copyImage };
        }

        private ContextMenu GetContextMenu()
        {
            ContextMenu menu = new();
            foreach (var curr in Interaction.ContextMenuItems)
            {
                var menuItem = new MenuItem { Header = curr.Header };
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

        public void RequestContextMenu(Pixel position)
        {
            var menu = GetContextMenu();
            menu.PlacementTarget = this;
            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            menu.IsOpen = true;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
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
                FileName = "ScottPlot.png",
                Filter = "PNG Files (*.png)|*.png" +
                         "|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                         // "|BMP Files (*.bmp)|*.bmp" + // TODO: BMP support
                         "|WebP Files (*.webp)|*.webp" +
                         "|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() is true)
            {
                var format = ImageFormatHelpers.FromFilePath(dialog.FileName);
                if (!format.HasValue)
                    return;

                try
                {
                    Plot.Save(dialog.FileName, format: format.Value);
                }
                catch (Exception)
                {
                    // TODO: Not sure if we can meaningfully do anything except perhaps show an error dialog?
                }
            }
        }

        private void CopyImageToClipboard()
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = new MemoryStream(Plot.GetImage().GetImageBytes());
            bmp.EndInit();
            bmp.Freeze();

            Clipboard.SetImage(bmp);
        }
    }
}
