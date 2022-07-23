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
using Key = ScottPlot.Control.Key;
using MouseButton = ScottPlot.Control.MouseButton;
using Ava = global::Avalonia;

namespace ScottPlot.Avalonia
{
    public partial class AvaPlot : UserControl, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Backend Backend { get; private set; }

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
            using var surface = SKSurface.Create(new SKImageInfo((int)image.Width, (int)image.Height));
            if (surface is not null)
            {
                Plot.Render(surface);

                var img = surface.Snapshot();
                var pixels = img.ToRasterImage().PeekPixels();

                var bmp = new WriteableBitmap(new global::Avalonia.PixelSize((int)image.Width, (int)image.Height), new(1, 1), PixelFormat.Rgba8888, AlphaFormat.Unpremul);
                var buf = bmp.Lock();

                Marshal.Copy(pixels.GetPixelSpan().ToArray(), 0, buf.Address, pixels.BytesSize); // Not happy with the .ToArray()

                using var stream = new MemoryStream();
                bmp.Save(stream);
                stream.Position = 0;
                image.Source = new Bitmap(stream); // You can't set image.Source to a WritableBitmap, even though it implements IImage
            }
        }

        private Pixel GetPointerPosition(PointerEventArgs e)
        {
            return new(
                    x: (float)(e.GetPosition(this).X),
                    y: (float)(e.GetPosition(this).Y));
        }

        private Key GetKey(KeyEventArgs e)
        {
            if (e.Key == Ava.Input.Key.LeftAlt || e.Key == Ava.Input.Key.RightAlt)
            {
                return Key.Alt;
            }
            else if (e.Key == Ava.Input.Key.LeftShift || e.Key == Ava.Input.Key.RightShift)
            {
                return Key.Shift;
            }
            else if (e.Key == Ava.Input.Key.LeftCtrl || e.Key == Ava.Input.Key.RightCtrl)
            {
                return Key.Ctrl;
            }

            return Key.UNKNOWN;
        }

        // TODO: should every On event call its base event???
        private void OnMouseDown(object sender, PointerPressedEventArgs e)
        {
            var position = GetPointerPosition(e);

            switch (e.GetCurrentPoint(this).Properties.PointerUpdateKind)
            {
                case PointerUpdateKind.LeftButtonPressed:
                    Backend.MouseDown(position, MouseButton.Left);
                    break;
                case PointerUpdateKind.RightButtonPressed:
                    Backend.MouseDown(position, MouseButton.Right);
                    break;
                case PointerUpdateKind.MiddleButtonPressed:
                    Backend.MouseDown(position, MouseButton.Middle);
                    break;
                default:
                    Backend.MouseDown(position, MouseButton.Unknown);
                    break;
            }

            e.Pointer.Capture(this);

            if (e.ClickCount == 2)
            {
                Backend.DoubleClick();
            }
        }

        private void OnMouseUp(object sender, PointerReleasedEventArgs e)
        {
            var position = GetPointerPosition(e);

            switch (e.GetCurrentPoint(this).Properties.PointerUpdateKind)
            {
                case PointerUpdateKind.LeftButtonReleased:
                    Backend.MouseUp(position, MouseButton.Left);
                    break;
                case PointerUpdateKind.RightButtonReleased:
                    Backend.MouseUp(position, MouseButton.Right);
                    break;
                case PointerUpdateKind.MiddleButtonReleased:
                    Backend.MouseUp(position, MouseButton.Middle);
                    break;
                default:
                    Backend.MouseUp(position, MouseButton.Unknown);
                    break;
            }

            e.Pointer.Capture(null);
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            Backend.MouseMove(GetPointerPosition(e));
        }

        private void OnMouseWheel(object sender, PointerWheelEventArgs e)
        {
            Backend.MouseWheel(GetPointerPosition(e), (float)e.Delta.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // This will fire many times when the key is held, causing performance issues. Avalonia doesn't seem to offer a solution?
            Backend.KeyDown(GetKey(e));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.KeyUp(GetKey(e));
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
