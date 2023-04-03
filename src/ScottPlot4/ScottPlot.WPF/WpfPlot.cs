using Microsoft.Win32;
using ScottPlot.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#pragma warning disable IDE1006 // lowercase public properties
#pragma warning disable CS0067 // unused events

namespace ScottPlot
{
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    [TemplatePart(Name = PART_LABEL_NAME, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_IMAGE_NAME, Type = typeof(Image))]
    public class WpfPlot : System.Windows.Controls.Control, IPlotControl
    {
        private const string PART_LABEL_NAME = "PART_ErrorLabel";
        private const string PART_IMAGE_NAME = "PART_PlotImage";

        private TextBlock ErrorLabel;
        private Image PlotImage;

        public static readonly RoutedEvent AxesChangedEvent = MakeRoutedEvent("AxesChanged");
        public static readonly RoutedEvent RightClickedEvent = MakeRoutedEvent("RightClicked");
        public static readonly RoutedEvent LeftClickedEvent = MakeRoutedEvent("LeftClicked");
        public static readonly RoutedEvent LeftClickedPlottableEvent = MakeRoutedEvent("LeftClickedPlottable");
        public static readonly RoutedEvent PlottableDraggedEvent = MakeRoutedEvent("PlottableDragged");
        public static readonly RoutedEvent PlottableDroppedEvent = MakeRoutedEvent("PlottableDropped");

        private static RoutedEvent MakeRoutedEvent(string name) =>
            EventManager.RegisterRoutedEvent(
                name: name,
                routingStrategy: RoutingStrategy.Bubble,
                handlerType: typeof(RoutedEventHandler),
                ownerType: typeof(WpfPlot));

        /// <summary>
        /// This is the plot displayed by the user control.
        /// After modifying it you may need to call Render() to request the plot be redrawn on the screen.
        /// </summary>
        public Plot Plot => Backend.Plot;

        /// <summary>
        /// This object can be used to modify advanced behaior and customization of this user control.
        /// </summary>
        public Configuration Configuration => Backend.Configuration;

        /// <summary>
        /// This event is invoked any time the axis limits are modified.
        /// </summary>
        public event RoutedEventHandler AxesChanged
        {
            add { AddHandler(AxesChangedEvent, value); }
            remove { RemoveHandler(AxesChangedEvent, value); }
        }
        protected virtual void RaiseAxesChangedEvent()
        {
            RaiseEvent(new RoutedEventArgs(AxesChangedEvent, this));
        }

        /// <summary>
        /// This event is invoked any time the plot is right-clicked.
        /// By default it contains DefaultRightClickEvent(), but you can remove this and add your own method.
        /// </summary>
        public event RoutedEventHandler RightClicked
        {
            add { AddHandler(RightClickedEvent, value); }
            remove { RemoveHandler(RightClickedEvent, value); }
        }

        protected virtual void RaiseRightClickedEvent()
        {
            RaiseEvent(new RoutedEventArgs(RightClickedEvent, this));
        }

        /// <summary>
        /// This event is invoked any time the plot is left-clicked.
        /// It is typically used to interact with custom plot types.
        /// </summary>
        public event RoutedEventHandler LeftClicked
        {
            add { AddHandler(LeftClickedEvent, value); }
            remove { RemoveHandler(LeftClickedEvent, value); }
        }

        protected virtual void RaiseLeftClickedEvent()
        {
            RaiseEvent(new RoutedEventArgs(LeftClickedEvent, this));
        }

        /// <summary>
        /// This event is invoked when a <seealso cref="Plottable.IHittable"/> plottable is left-clicked.
        /// </summary>
        public event RoutedEventHandler LeftClickedPlottable
        {
            add { AddHandler(LeftClickedPlottableEvent, value); }
            remove { RemoveHandler(LeftClickedPlottableEvent, value); }
        }

        protected virtual void RaiseLeftClickedPlottableEvent()
        {
            RaiseEvent(new RoutedEventArgs(LeftClickedPlottableEvent, this));
        }

        /// <summary>
        /// This event is invoked after the mouse moves while dragging a draggable plottable.
        /// The object passed is the plottable being dragged.
        /// </summary>
        public event RoutedEventHandler PlottableDragged
        {
            add { AddHandler(PlottableDroppedEvent, value); }
            remove { RemoveHandler(PlottableDraggedEvent, value); }
        }

        protected virtual void RaisePlottableDraggedEvent()
        {
            RaiseEvent(new RoutedEventArgs(PlottableDraggedEvent, this));
        }

        [Obsolete("use 'PlottableDragged' instead", error: true)]
        public event EventHandler MouseDragPlottable;

        /// <summary>
        /// This event is invoked right after a draggable plottable was dropped.
        /// The object passed is the plottable that was just dropped.
        /// </summary> 
        public event RoutedEventHandler PlottableDropped
        {
            add { AddHandler(PlottableDroppedEvent, value); }
            remove { RemoveHandler(PlottableDroppedEvent, value); }
        }

        protected virtual void RaisePlottableDroppedEvent()
        {
            RaiseEvent(new RoutedEventArgs(PlottableDroppedEvent, this));
        }

        [Obsolete("use 'PlottableDropped' instead", error: true)]
        public event EventHandler MouseDropPlottable;

        private readonly ControlBackEnd Backend;
        private readonly Dictionary<Cursor, System.Windows.Input.Cursor> Cursors;
        private float ScaledWidth => (float)(ActualWidth * Configuration.DpiStretchRatio);
        private float ScaledHeight => (float)(ActualHeight * Configuration.DpiStretchRatio);

        [Obsolete("Reference Plot instead of plt")]
        public Plot plt => Plot;

        static WpfPlot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfPlot), new FrameworkPropertyMetadata(typeof(WpfPlot)));
        }

        public WpfPlot()
        {
            Backend = new(1, 1, GetType().Name);
            Backend.BitmapChanged += (o, e) => ReplacePlotBitmap(Backend.GetLatestBitmap());
            Backend.BitmapUpdated += (o, e) => UpdatePlotBitmap(Backend.GetLatestBitmap());
            Backend.CursorChanged += (o, e) => Cursor = Cursors[Backend.Cursor];
            Backend.RightClicked += (o, e) => RaiseRightClickedEvent();
            Backend.LeftClicked += (o, e) => RaiseLeftClickedEvent();
            Backend.LeftClickedPlottable += (o, e) => RaiseLeftClickedPlottableEvent();
            Backend.AxesChanged += (o, e) => RaiseAxesChangedEvent();
            Backend.PlottableDragged += (o, e) => RaisePlottableDraggedEvent();
            Backend.PlottableDropped += (o, e) => RaisePlottableDroppedEvent();
            Backend.Configuration.ScaleChanged += (o, e) => Backend.Resize(ScaledWidth, ScaledHeight, useDelayedRendering: true);


            Cursors = new()
            {
                [ScottPlot.Cursor.Arrow] = System.Windows.Input.Cursors.Arrow,
                [ScottPlot.Cursor.WE] = System.Windows.Input.Cursors.SizeWE,
                [ScottPlot.Cursor.NS] = System.Windows.Input.Cursors.SizeNS,
                [ScottPlot.Cursor.All] = System.Windows.Input.Cursors.SizeAll,
                [ScottPlot.Cursor.Crosshair] = System.Windows.Input.Cursors.Cross,
                [ScottPlot.Cursor.Hand] = System.Windows.Input.Cursors.Hand,
                [ScottPlot.Cursor.Question] = System.Windows.Input.Cursors.Help,
            };

            RightClicked += DefaultRightClickEvent;

            Backend.StartProcessingEvents();
        }

        public override void OnApplyTemplate()
        {
            ErrorLabel = Template.FindName(PART_LABEL_NAME, this) as TextBlock;
            PlotImage = Template.FindName(PART_IMAGE_NAME, this) as Image;
            if (PlotImage != null)
                PlotImage.Visibility = Visibility.Visible;

            if (ErrorLabel != null)
                ErrorLabel.Visibility = Visibility.Hidden;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                try
                {
                    Configuration.WarnIfRenderNotCalledManually = false;
                    Plot.Title($"ScottPlot {Plot.Version}");
                    Plot.Render();
                }
                catch (Exception e)
                {
                    if (PlotImage != null)
                        PlotImage.Visibility = Visibility.Hidden;
                    if (ErrorLabel != null)
                    {
                        ErrorLabel.Text = "ERROR: ScottPlot failed to render in design mode.\n\n" +
                            "This may be due to incompatible System.Drawing.Common versions or a 32-bit/64-bit mismatch.\n\n" +
                            "Although rendering failed at design time, it may still function normally at runtime.\n\n" +
                            $"Exception details:\n{e}";
                        ErrorLabel.Visibility = Visibility.Visible;
                    }
                }
            }

            Backend.Resize((float)ActualWidth, (float)ActualHeight, useDelayedRendering: false);
            base.OnApplyTemplate();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Backend.Resize(ScaledWidth, ScaledHeight, useDelayedRendering: true);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            CaptureMouse();
            Backend.MouseDown(GetInputState(e));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Backend.MouseMove(GetInputState(e));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            Backend.MouseUp(GetInputState(e));
            ReleaseMouseCapture();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Backend.MouseWheel(GetInputState(e, e.Delta));
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            Backend.DoubleClick();
        }

        /// <summary>
        /// Return the mouse position on the plot (in coordinate space) for the latest X and Y coordinates
        /// </summary>
        public (double x, double y) GetMouseCoordinates(int xAxisIndex = 0, int yAxisIndex = 0) => Backend.GetMouseCoordinates(xAxisIndex, yAxisIndex);

        /// <summary>
        /// Return the mouse position (in pixel space) for the last observed mouse position
        /// </summary>
        public (float x, float y) GetMousePixel() => Backend.GetMousePixel();

        /// <summary>
        /// Reset this control by replacing the current plot with a new empty plot
        /// </summary>
        public void Reset() => Backend.Reset((float)ActualWidth, (float)ActualHeight);

        /// <summary>
        /// Reset this control by replacing the current plot with an existing plot
        /// </summary>
        public void Reset(Plot newPlot) => Backend.Reset((float)ActualWidth, (float)ActualHeight, newPlot);

        /// <summary>
        /// Re-render the plot and update the image displayed by this control.
        /// </summary>
        public void Refresh()
        {
            Refresh(false);
        }

        /// <summary>
        /// Re-render the plot and update the image displayed by this control.
        /// </summary>
        /// <param name="lowQuality">disable anti-aliasing to produce faster (but lower quality) plots</param>
        public void Refresh(bool lowQuality = false)
        {
            Backend.WasManuallyRendered = true;
            Backend.Render(lowQuality);
        }

        // TODO: mark this obsolete in ScottPlot 5.0 (favor Refresh)
        /// <summary>
        /// Re-render the plot and update the image displayed by this control.
        /// </summary>
        /// <param name="lowQuality">disable anti-aliasing to produce faster (but lower quality) plots</param>
        public void Render(bool lowQuality = false) => Refresh(lowQuality);

        /// <summary>
        /// Request the control to refresh the next time it is available.
        /// This method does not block the calling thread.
        /// </summary>
        public void RefreshRequest(RenderType renderType = RenderType.LowQualityThenHighQualityDelayed)
        {
            Backend.WasManuallyRendered = true;
            Backend.RenderRequest(renderType);
        }

        // TODO: mark this obsolete in ScottPlot 5.0 (favor Refresh)
        /// <summary>
        /// Request the control to refresh the next time it is available.
        /// This method does not block the calling thread.
        /// </summary>
        public void RenderRequest(RenderType renderType = RenderType.LowQualityThenHighQualityDelayed) => RefreshRequest(renderType);

        /// <summary>
        /// This object stores the bitmap that is displayed in the PlotImage.
        /// When this control is created or resized this bitmap is replaced by a new one.
        /// When new renders are requested (without resizing) they are drawn onto this existing bitmap.
        /// </summary>
        private WriteableBitmap PlotBitmap;

        private ScottPlot.Control.InputState GetInputState(MouseEventArgs e, double? delta = null) =>
            new()
            {
                X = (float)e.GetPosition(this).X * Configuration.DpiStretchRatio,
                Y = (float)e.GetPosition(this).Y * Configuration.DpiStretchRatio,
                LeftWasJustPressed = e.LeftButton == MouseButtonState.Pressed,
                RightWasJustPressed = e.RightButton == MouseButtonState.Pressed,
                MiddleWasJustPressed = e.MiddleButton == MouseButtonState.Pressed,
                ShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift),
                CtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl),
                AltDown = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt),
                WheelScrolledUp = delta.HasValue && delta > 0,
                WheelScrolledDown = delta.HasValue && delta < 0,
            };

        public static BitmapImage BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            using var memory = new System.IO.MemoryStream();
            bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        /// <summary>
        /// Replace the existing PlotBitmap with a new one.
        /// </summary>
        public void ReplacePlotBitmap(System.Drawing.Bitmap bmp)
        {
            PlotBitmap = new WriteableBitmap(BmpImageFromBmp(bmp));
            if (PlotImage != null)
                PlotImage.Source = PlotBitmap;
        }

        /// <summary>
        /// Update the PlotBitmap with pixel data from the latest render.
        /// If a PlotBitmap does not exist one will be created.
        /// </summary>
        private void UpdatePlotBitmap(System.Drawing.Bitmap bmp)
        {
            if (PlotBitmap is null)
            {
                ReplacePlotBitmap(Backend.GetLatestBitmap());
                return;
            }

            var rect1 = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var flags = System.Drawing.Imaging.ImageLockMode.ReadOnly;
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect1, flags, bmp.PixelFormat);

            try
            {
                var rect2 = new System.Windows.Int32Rect(0, 0, bmpData.Width, bmpData.Height);
                PlotBitmap.WritePixels(
                    sourceRect: rect2,
                    buffer: bmpData.Scan0,
                    bufferSize: bmpData.Stride * bmpData.Height,
                    stride: bmpData.Stride);
            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }
        }

        /// <summary>
        /// Launch the default right-click menu.
        /// </summary>
        public void DefaultRightClickEvent(object sender, EventArgs e)
        {
            MenuItem item;
            var cm = new ContextMenu();

            item = new() { Header = "Save Image..." };
            item.Click += (o, e) => SaveAsImage();
            cm.Items.Add(item);

            item = new() { Header = "Copy Image" };
            item.Click += (o, e) => Clipboard.SetImage(BmpImageFromBmp(Plot.Render()));
            cm.Items.Add(item);

            cm.Items.Add(new Separator());

            item = new() { Header = "Zoom to Fit Data" };
            item.Click += (o, e) => { Plot.AxisAuto(); Refresh(); };
            cm.Items.Add(item);

            cm.Items.Add(new Separator());

            item = new() { Header = "Help" };
            item.Click += (o, e) => new ScottPlot.WPF.HelpWindow().Show();
            cm.Items.Add(item);

            item = new() { Header = "Open in New Window" };
            item.Click += (o, e) => new ScottPlot.WpfPlotViewer(Plot).Show();
            cm.Items.Add(item);

            cm.IsOpen = true;
        }

        public void SaveAsImage()
        {
            var sfd = new SaveFileDialog
            {
                FileName = "ScottPlot.png",
                Filter = "PNG Files (*.png)|*.png" +
                         "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                         "|BMP Files (*.bmp)|*.bmp" +
                         "|All files (*.*)|*.*"
            };

            if (sfd.ShowDialog() is true)
                Plot.SaveFig(sfd.FileName);
        }
    }
}
