using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Visuals.Media.Imaging;
using Avalonia.VisualTree;
using ScottPlot.Control;
using SkiaSharp;

namespace ScottPlot.Avalonia
{
    public partial class AvaPlot : UserControl, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Interaction Interaction { get; private set; }

        private readonly List<FileDialogFilter> fileDialogFilters = new()
        {
            new() { Name = "PNG Files", Extensions = new List<string> { "png" } },
            new() { Name = "JPEG Files", Extensions = new List<string> { "jpg", "jpeg" } },
            // new() { Name = "BMP Files", Extensions = new List<string> { "bmp" } }, // TODO: BMP support
            new() { Name = "WebP Files", Extensions = new List<string> { "webp" } },
            new() { Name = "All Files", Extensions = new List<string> { "*" } },
        };

        public AvaPlot()
        {
            InitializeComponent();
            Interaction = new(this);
            Interaction.ContextMenuItems = GetDefaultContextMenuItems();

            Refresh();
        }

        private ContextMenuItem[] GetDefaultContextMenuItems()
        {
            ContextMenuItem saveImage = new() { Header = "Save Image", OnInvoke = OpenSaveImageDialog };
            // TODO: Copying images to the clipboard is still difficult in Avalonia https://github.com/AvaloniaUI/Avalonia/issues/3588

            return new ContextMenuItem[] { saveImage };
        }

        private ContextMenu GetContextMenu()
        {
            List<MenuItem> items = new();
            foreach (var curr in Interaction.ContextMenuItems)
            {
                var menuItem = new MenuItem { Header = curr.Header };
                menuItem.Click += (s, e) => curr.OnInvoke();

                items.Add(menuItem);
            }


            return new()
            {
                Items = items
            };
        }

        private async void OpenSaveImageDialog()
        {
            SaveFileDialog dialog = new() { InitialFileName = "ScottPlot.png", Filters = fileDialogFilters };
            Task<string?> filenameTask = dialog.ShowAsync((Window)this.GetVisualRoot());
            var filename = await filenameTask;

            if (filenameTask.IsFaulted || string.IsNullOrEmpty(filename))
                return;

            var format = ImageFormatHelpers.FromFilePath(filename);
            if (!format.HasValue)
                return;

            try
            {
                Plot.GetImage().Save(filename, format.Value);
            }
            catch (Exception _)
            {
                // TODO: Not sure if we can meaningfully do anything except perhaps show an error dialog?
            }
        }

        public void Replace(Interaction interaction)
        {
            Interaction = interaction;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            SKImageInfo imageInfo = new((int)Bounds.Width, (int)Bounds.Height);

            using var surface = SKSurface.Create(imageInfo);
            if (surface is null)
                return;

            Plot.Render(surface);

            SKImage img = surface.Snapshot();
            SKPixmap pixels = img.ToRasterImage().PeekPixels();
            byte[] bytes = pixels.GetPixelSpan().ToArray();

            using WriteableBitmap bmp = new(
                size: new global::Avalonia.PixelSize((int)Bounds.Width, (int)Bounds.Height),
                dpi: new Vector(1, 1),
                format: PixelFormat.Bgra8888,
                alphaFormat: AlphaFormat.Unpremul);

            using ILockedFramebuffer buf = bmp.Lock();
            {
                Marshal.Copy(bytes, 0, buf.Address, pixels.BytesSize);
            }

            Rect rect = new(0, 0, Bounds.Width, Bounds.Height);

            context.DrawImage(bmp, rect, rect, BitmapInterpolationMode.HighQuality);
        }

        public void Refresh()
        {
            InvalidateVisual();
        }

        public void RequestContextMenu(Pixel position)
        {
            var manualContextMenu = GetContextMenu();

            // I am fully aware of how janky it is to place the menu in a 1x1 rect, unfortunately the Avalonia docs were down when I wrote this
            manualContextMenu.PlacementRect = new(position.X, position.Y, 1, 1);
            manualContextMenu.Open(this);
        }

        private void OnMouseDown(object sender, PointerPressedEventArgs e)
        {
            Interaction.MouseDown(
                position: e.ToPixel(this),
                button: e.GetCurrentPoint(this).Properties.PointerUpdateKind.ToButton());

            e.Pointer.Capture(this);

            if (e.ClickCount == 2)
            {
                Interaction.DoubleClick();
            }
        }

        private void OnMouseUp(object sender, PointerReleasedEventArgs e)
        {
            Interaction.MouseUp(
                position: e.ToPixel(this),
                button: e.GetCurrentPoint(this).Properties.PointerUpdateKind.ToButton());

            e.Pointer.Capture(null);
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            Interaction.OnMouseMove(e.ToPixel(this));
        }

        private void OnMouseWheel(object sender, PointerWheelEventArgs e)
        {
            // Avalonia flips the delta vector when shift is held. This is seemingly intentional: https://github.com/AvaloniaUI/Avalonia/pull/7520
            float delta = (float)(e.KeyModifiers.HasFlag(KeyModifiers.Shift) ? e.Delta.X : e.Delta.Y);

            if (delta != 0)
            {
                Interaction.MouseWheelVertical(e.ToPixel(this), delta);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Interaction.KeyDown(e.ToKey());
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Interaction.KeyUp(e.ToKey());
        }
    }
}
