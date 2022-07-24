using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ScottPlot.Control;
using SkiaSharp;

namespace ScottPlot.Avalonia
{
    public partial class AvaPlot : UserControl, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Backend Backend { get; private set; }

        public InputMap InputMap
        {
            get => Backend.Interactions.InputMap;
            set => Backend.Interactions.InputMap = value;
        }

        private readonly Image image;

        public AvaPlot()
        {
            InitializeComponent();
            image = this.Find<Image>("image");
            Backend = new(this);

            Refresh();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Refresh()
        {
            UpdateBounds();

            SKImageInfo imageInfo = new((int)image.Width, (int)image.Height);
            using var surface = SKSurface.Create(imageInfo);
            if (surface is null)
                return;

            Plot.Render(surface);

            // TODO: can this sequence be improved to reduce copying?
            SKImage img = surface.Snapshot();
            SKPixmap pixels = img.ToRasterImage().PeekPixels();
            byte[] bytes = pixels.GetPixelSpan().ToArray();

            using WriteableBitmap bmp = new(
                size: new global::Avalonia.PixelSize((int)image.Width, (int)image.Height),
                dpi: new Vector(1, 1),
                format: PixelFormat.Bgra8888,
                alphaFormat: AlphaFormat.Unpremul);

            ILockedFramebuffer buf = bmp.Lock();
            Marshal.Copy(bytes, 0, buf.Address, pixels.BytesSize);

            // You can't set image.Source to a WritableBitmap
            using MemoryStream stream = new();
            bmp.Save(stream);
            stream.Position = 0;
            Bitmap bmpShow = new(stream);

            image.Source = bmpShow;
        }

        // TODO: should every On event call its base event???
        private void OnMouseDown(object sender, PointerPressedEventArgs e)
        {
            Backend.MouseDown(
                position: e.ToPixel(this),
                button: e.GetCurrentPoint(this).Properties.PointerUpdateKind.ToButton());

            e.Pointer.Capture(this);

            if (e.ClickCount == 2)
            {
                Backend.DoubleClick();
            }
        }

        private void OnMouseUp(object sender, PointerReleasedEventArgs e)
        {
            Backend.MouseUp(
                position: e.ToPixel(this),
                button: e.GetCurrentPoint(this).Properties.PointerUpdateKind.ToButton());

            e.Pointer.Capture(null);
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            Backend.MouseMove(e.ToPixel(this));
        }

        private void OnMouseWheel(object sender, PointerWheelEventArgs e)
        {
            Backend.MouseWheel(e.ToPixel(this), (float)e.Delta.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // This will fire many times while the key is held, causing performance issues.
            // Avalonia doesn't seem to offer a solution?
            // TODO: maybe we can detect and gate renders on key changes
            Backend.KeyDown(e.ToKey());
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.KeyUp(e.ToKey());
        }

        private void OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Bounds")
            {
                Refresh();
            }
        }

        private void UpdateBounds()
        {
            image.Width = Bounds.Width;
            image.Height = Bounds.Height;
        }
    }
}
