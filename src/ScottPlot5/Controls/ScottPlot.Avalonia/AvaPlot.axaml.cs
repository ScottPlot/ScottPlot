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

namespace ScottPlot.Avalonia
{
    public partial class AvaPlot : UserControl, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Backend Backend { get; private set; }

        public Coordinates MouseCoordinates => Backend.MouseCoordinates;

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
            this.Focusable = true;

            PropertyChanged += OnPropertyChanged;
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

                var bmp = new WriteableBitmap(new global::Avalonia.PixelSize((int)image.Width, (int)image.Height), new (1, 1), PixelFormat.Rgba8888, AlphaFormat.Unpremul);
                var buf = bmp.Lock();

                Marshal.Copy(pixels.GetPixelSpan().ToArray(), 0, buf.Address, pixels.BytesSize); // Not happy with the .ToArray()

                using var stream = new MemoryStream();
                bmp.Save(stream);
                stream.Position = 0;
                image.Source = new Bitmap(stream); // You can't set image.Source to a WritableBitmap, even though it implements IImage
            }
        }

        private MouseInputState GetMouseState(PointerEventArgs e)
        {
            Pixel mousePosition = new(
                    x: (float)(e.GetPosition(this).X),
                    y: (float)(e.GetPosition(this).Y));

            List<MouseButton?> pressedButtons = new()
            {
                e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed ? MouseButton.Mouse1 : null,
                e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed ? MouseButton.Mouse2 : null,
                e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonPressed ? MouseButton.Mouse3 : null,
            };

            return new MouseInputState(mousePosition, pressedButtons);
        }

        private Key GetKey(KeyEventArgs e)
        {
            return Key.UNKNOWN;
        }

        // TODO: should every On event call its base event???
        private void OnMouseDown(object sender, PointerEventArgs e)
        {
            //Keyboard.Focus(this);

            MouseInputState state = GetMouseState(e);

            Backend.TriggerMouseDown(state);

            e.Pointer.Capture(this);

            //if (e.ClickCount == 2)
            //{
            //    Backend.TriggerDoubleClick(MouseInputState.Empty);
            //}
        }

        private void OnMouseUp(object sender, PointerEventArgs e)
        {
            Backend.TriggerMouseUp(GetMouseState(e));
            e.Pointer.Capture(null);
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            Backend.TriggerMouseMove(GetMouseState(e));
        }

        private void OnMouseWheel(object sender, PointerWheelEventArgs e)
        {
            Backend.TriggerMouseWheel(GetMouseState(e), 0, (float)e.Delta.Y);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Backend.TriggerKeyDown(GetKey(e));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.TriggerKeyUp(GetKey(e));
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
