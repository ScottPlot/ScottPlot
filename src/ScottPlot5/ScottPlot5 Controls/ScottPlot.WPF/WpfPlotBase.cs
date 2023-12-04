using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ScottPlot.Control;
using SkiaSharp;

namespace ScottPlot.WPF
{
    public abstract class WpfPlotBase : System.Windows.Controls.Control, IPlotControl
    {
        public abstract GRContext GRContext { get; }

        public abstract void Refresh();

        public Plot Plot { get; internal set; }

        public Interaction Interaction { get; internal set; }

        public float DisplayScale { get; set; }

        static WpfPlotBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                forType: typeof(WpfPlotBase),
                typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlotBase)));
        }
        public WpfPlotBase()
        {
            DisplayScale = DetectDisplayScale();

            Interaction = new(this)
            {
                ContextMenuItems = GetDefaultContextMenuItems()
            };

            Plot = Reset();

            Focusable = true;
        }

        internal ContextMenuItem[] GetDefaultContextMenuItems()
        {
            ContextMenuItem saveImage = new()
            {
                Label = "Save Image",
                OnInvoke = OpenSaveImageDialog
            };

            ContextMenuItem copyImage = new()
            {
                Label = "Copy to Clipboard",
                OnInvoke = CopyImageToClipboard
            };

            return new ContextMenuItem[] { saveImage, copyImage };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Interaction.KeyDown(e.Key());
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            Interaction.KeyUp(e.Key());
        }

        public Plot Reset()
        {
            return Reset(new Plot());
        }

        public Plot Reset(Plot newPlot)
        {
            Plot oldPlot = Plot;
            Plot = newPlot;
            oldPlot?.Dispose();
            return newPlot;
        }

        public void Replace(Interaction interaction)
        {
            Interaction = interaction;
        }

        public void ShowContextMenu(Pixel position)
        {
            var menu = Interaction.GetContextMenu();
            menu.PlacementTarget = this;
            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            menu.IsOpen = true;
        }

        internal void SKElement_MouseDown(object? sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

            Interaction.MouseDown(e.Pixel(this), e.ToButton());

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                Interaction.DoubleClick();
            }
        }

        internal void SKElement_MouseUp(object? sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Interaction.MouseUp(e.Pixel(this), e.ToButton());
            (sender as UIElement)?.ReleaseMouseCapture();
        }

        internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
        {
            Interaction.OnMouseMove(e.Pixel(this));
        }

        internal void SKElement_MouseWheel(object? sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Interaction.MouseWheelVertical(e.Pixel(this), e.Delta);
        }

        internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
        {
            Interaction.KeyDown(e.Key());
        }

        internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
        {
            Interaction.KeyUp(e.Key());
        }

        internal void OpenSaveImageDialog()
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
                if (string.IsNullOrEmpty(dialog.FileName))
                    return;

                ImageFormat format;

                try
                {
                    format = ImageFormatLookup.FromFilePath(dialog.FileName);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Unsupported image file format", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    Plot.Save(dialog.FileName, (int)ActualWidth, (int)ActualHeight, format);
                }
                catch (Exception)
                {
                    MessageBox.Show("Image save failed", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        public void CopyImageToClipboard()
        {
            BitmapImage bmp = Plot.GetBitmapImage((int)ActualWidth, (int)ActualHeight);
            Clipboard.SetImage(bmp);
        }

        public Coordinates GetCoordinates(Pixel px, IXAxis? xAxis = null, IYAxis? yAxis = null)
        {
            float x = px.X * DisplayScale;
            float y = px.Y * DisplayScale;
            return Plot.GetCoordinates(x, y, xAxis, yAxis);
        }

        public float DetectDisplayScale()
        {
            return (float)VisualTreeHelper.GetDpi(this).DpiScaleX;
        }
    }
}
