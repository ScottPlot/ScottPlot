using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ScottPlot.Control;
using SkiaSharp;
using System.Windows.Media;

namespace ScottPlot.WPF
{
    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    [TemplatePart(Name = PART_SKElement, Type = typeof(SkiaSharp.Views.WPF.SKGLElement))]
    public class WpfPlot : System.Windows.Controls.Control, IPlotControl
    {
        private const string PART_SKElement = "PART_SKElement";

        private SkiaSharp.Views.WPF.SKGLElement? SKElement;
        public Plot Plot { get; }

        public Interaction Interaction { get; private set; }

        public GRContext GRContext => SKElement?.GRContext ?? GRContext.CreateGl();

        static WpfPlot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                forType: typeof(WpfPlot),
                typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlot)));
        }

        public WpfPlot()
        {
            Plot = Reset();

            Interaction = new(this)
            {
                ContextMenuItems = GetDefaultContextMenuItems()
            };

            Focusable = true;
        }

        public override void OnApplyTemplate()
        {
            SKElement = Template.FindName(PART_SKElement, this) as SkiaSharp.Views.WPF.SKGLElement;

            if (SKElement == null)
                return;

            SKElement.PaintSurface += (sender, e) => Plot.Render(e.Surface);

            SKElement.MouseDown += (sender, e) =>
            {
                Keyboard.Focus(this);

                Interaction.MouseDown(e.Pixel(this), e.ToButton());

                (sender as UIElement)?.CaptureMouse();

                if (e.ClickCount == 2)
                {
                    Interaction.DoubleClick();
                }
            };

            SKElement.MouseUp += (sender, e) =>
            {
                Interaction.MouseUp(e.Pixel(this), e.ToButton());
                (sender as UIElement)?.ReleaseMouseCapture();
            };

            SKElement.MouseMove += (sender, e) => Interaction.OnMouseMove(e.Pixel(this));

            SKElement.MouseWheel += (sender, e) => Interaction.MouseWheelVertical(e.Pixel(this), e.Delta);
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

        private ContextMenuItem[] GetDefaultContextMenuItems()
        {
            ContextMenuItem saveImage = new() { Label = "Save Image", OnInvoke = OpenSaveImageDialog };
            ContextMenuItem copyImage = new() { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

            return new ContextMenuItem[] { saveImage, copyImage };
        }

        public Plot Reset()
        {
            return new Plot()
            {
                ScaleFactor = (float)VisualTreeHelper.GetDpi(this).DpiScaleX
            };
        }

        public void Replace(Interaction interaction)
        {
            Interaction = interaction;
        }

        public void Refresh()
        {
            SKElement?.InvalidateVisual();
        }

        public void ShowContextMenu(Pixel position)
        {
            ContextMenu menu = Interaction.GetContextMenu();
            menu.PlacementTarget = this;
            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            menu.IsOpen = true;
        }

        public void OpenSaveImageDialog()
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

        public void CopyImageToClipboard()
        {
            BitmapImage bmp = Plot.GetBitmapImage((int)ActualWidth, (int)ActualHeight);
            Clipboard.SetImage(bmp);
        }
    }
}
