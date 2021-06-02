﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#pragma warning disable IDE1006 // lowercase public properties
#pragma warning disable CS0067 // unused events

namespace ScottPlot
{
    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class WpfPlot : UserControl
    {
        /// <summary>
        /// This is the plot displayed by the user control.
        /// After modifying it you may need to call Render() to request the plot be redrawn on the screen.
        /// </summary>
        public Plot Plot => Backend.Plot;

        /// <summary>
        /// This object can be used to modify advanced behaior and customization of this user control.
        /// </summary>
        public Control.Configuration Configuration => Backend.Configuration;

        /// <summary>
        /// This event is invoked any time the axis limits are modified.
        /// </summary>
        public event EventHandler AxesChanged;

        /// <summary>
        /// This event is invoked any time the plot is right-clicked.
        /// By default it contains DefaultRightClickEvent(), but you can remove this and add your own method.
        /// </summary>
        public event EventHandler RightClicked;

        /// <summary>
        /// This event is invoked after the mouse moves while dragging a draggable plottable.
        /// The object passed is the plottable being dragged.
        /// </summary>
        public event EventHandler PlottableDragged;

        [Obsolete("use 'PlottableDragged' instead", error: true)]
        public event EventHandler MouseDragPlottable;

        /// <summary>
        /// This event is invoked right after a draggable plottable was dropped.
        /// The object passed is the plottable that was just dropped.
        /// </summary>
        public event EventHandler PlottableDropped;

        [Obsolete("use 'PlottableDropped' instead", error: true)]
        public event EventHandler MouseDropPlottable;

        private readonly Control.ControlBackEnd Backend = new(1, 1);
        private readonly Dictionary<Cursor, System.Windows.Input.Cursor> Cursors;
        private readonly DispatcherTimer PlottableCountTimer = new();
        private readonly Control.DisplayScale DisplayScale = new();
        private float ScaledWidth => (float)ActualWidth * DisplayScale.ScaleRatio;
        private float ScaledHeight => (float)ActualHeight * DisplayScale.ScaleRatio;

        [Obsolete("Reference Plot instead of plt")]
        public Plot plt => Plot;

        public WpfPlot()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                try
                {
                    Plot.Title($"ScottPlot {Plot.Version}");
                    Plot.Render();
                }
                catch (Exception e)
                {
                    InitializeComponent();
                    PlotImage.Visibility = System.Windows.Visibility.Hidden;
                    ErrorLabel.Text = "ERROR: ScottPlot failed to render in design mode.\n\n" +
                        "This may be due to incompatible System.Drawing.Common versions or a 32-bit/64-bit mismatch.\n\n" +
                        "Although rendering failed at design time, it may still function normally at runtime.\n\n" +
                        $"Exception details:\n{e}";
                    return;
                }
            }

            Backend.Resize((float)ActualWidth, (float)ActualHeight, useDelayedRendering: false);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            Backend.AxesChanged += new EventHandler(OnAxesChanged);
            Backend.PlottableDragged += new EventHandler(OnPlottableDragged);
            Backend.PlottableDropped += new EventHandler(OnPlottableDropped);

            Cursors = new Dictionary<Cursor, System.Windows.Input.Cursor>()
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
            PlottableCountTimer.Tick += PlottableCountTimer_Tick;
            PlottableCountTimer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 10);
            PlottableCountTimer.Start();

            InitializeComponent();

            ErrorLabel.Visibility = System.Windows.Visibility.Hidden;

            Backend.StartProcessingEvents();
        }

        /// <summary>
        /// Return the mouse position on the plot (in coordinate space) for the latest X and Y coordinates
        /// </summary>
        public (double x, double y) GetMouseCoordinates() => Backend.GetMouseCoordinates();

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
        /// <param name="lowQuality">disable anti-aliasing to produce faster (but lower quality) plots</param>
        public void Render(bool lowQuality = false) => Backend.Render(lowQuality);

        /// <summary>
        /// Request the control re-render the next time it is available.
        /// This method does not block the calling thread.
        /// </summary>
        public void RenderRequest(RenderType renderType = RenderType.LowQualityThenHighQualityDelayed) => Backend.RenderRequest(renderType);

        private void PlottableCountTimer_Tick(object sender, EventArgs e) => Backend.RenderIfPlottableListChanged();
        private void OnBitmapChanged(object sender, EventArgs e) => PlotImage.Source = BmpImageFromBmp(Backend.GetLatestBitmap());
        private void OnBitmapUpdated(object sender, EventArgs e) => PlotImage.Source = BmpImageFromBmp(Backend.GetLatestBitmap());
        private void OnCursorChanged(object sender, EventArgs e) => Cursor = Cursors[Backend.Cursor];
        private void OnRightClicked(object sender, EventArgs e) => RightClicked?.Invoke(this, e);
        private void OnPlottableDragged(object sender, EventArgs e) => PlottableDragged?.Invoke(sender, e);
        private void OnPlottableDropped(object sender, EventArgs e) => PlottableDropped?.Invoke(sender, e);
        private void OnAxesChanged(object sender, EventArgs e) => AxesChanged?.Invoke(this, e);
        private void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e) => Backend.Resize(ScaledWidth, ScaledHeight, useDelayedRendering: true);
        private void OnMouseDown(object sender, MouseButtonEventArgs e) { CaptureMouse(); Backend.MouseDown(GetInputState(e)); }
        private void OnMouseUp(object sender, MouseButtonEventArgs e) { Backend.MouseUp(GetInputState(e)); ReleaseMouseCapture(); }
        private void OnDoubleClick(object sender, MouseButtonEventArgs e) => Backend.DoubleClick();
        private void OnMouseWheel(object sender, MouseWheelEventArgs e) => Backend.MouseWheel(GetInputState(e, e.Delta));
        private void OnMouseMove(object sender, MouseEventArgs e) { Backend.MouseMove(GetInputState(e)); base.OnMouseMove(e); }
        private void OnMouseEnter(object sender, MouseEventArgs e) => base.OnMouseEnter(e);
        private void OnMouseLeave(object sender, MouseEventArgs e) => base.OnMouseLeave(e);

        private Control.InputState GetInputState(MouseEventArgs e, double? delta = null) =>
            new()
            {
                X = (float)e.GetPosition(this).X * DisplayScale.ScaleRatio,
                Y = (float)e.GetPosition(this).Y * DisplayScale.ScaleRatio,
                LeftWasJustPressed = e.LeftButton == MouseButtonState.Pressed,
                RightWasJustPressed = e.RightButton == MouseButtonState.Pressed,
                MiddleWasJustPressed = e.MiddleButton == MouseButtonState.Pressed,
                ShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift),
                CtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl),
                AltDown = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt),
                WheelScrolledUp = delta.HasValue && delta > 0,
                WheelScrolledDown = delta.HasValue && delta < 0,
            };

        private static BitmapImage BmpImageFromBmp(System.Drawing.Bitmap bmp)
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
        /// Launch the default right-click menu.
        /// </summary>
        public void DefaultRightClickEvent(object sender, EventArgs e)
        {
            var cm = new ContextMenu();

            MenuItem SaveImageMenuItem = new() { Header = "Save Image" };
            SaveImageMenuItem.Click += RightClickMenu_SaveImage_Click;
            cm.Items.Add(SaveImageMenuItem);

            MenuItem CopyImageMenuItem = new() { Header = "Copy Image" };
            CopyImageMenuItem.Click += RightClickMenu_Copy_Click;
            cm.Items.Add(CopyImageMenuItem);

            MenuItem AutoAxisMenuItem = new() { Header = "Zoom to Fit Data" };
            AutoAxisMenuItem.Click += RightClickMenu_AutoAxis_Click;
            cm.Items.Add(AutoAxisMenuItem);

            MenuItem HelpMenuItem = new() { Header = "Help" };
            HelpMenuItem.Click += RightClickMenu_Help_Click;
            cm.Items.Add(HelpMenuItem);

            MenuItem OpenInNewWindowMenuItem = new() { Header = "Open in New Window" };
            OpenInNewWindowMenuItem.Click += RightClickMenu_OpenInNewWindow_Click;
            cm.Items.Add(OpenInNewWindowMenuItem);

            cm.IsOpen = true;
        }

        private void RightClickMenu_Copy_Click(object sender, EventArgs e) => System.Windows.Clipboard.SetImage(BmpImageFromBmp(Backend.GetLatestBitmap()));
        private void RightClickMenu_Help_Click(object sender, EventArgs e) => new WPF.HelpWindow().Show();
        private void RightClickMenu_OpenInNewWindow_Click(object sender, EventArgs e) => new WpfPlotViewer(Plot).Show();
        private void RightClickMenu_AutoAxis_Click(object sender, EventArgs e) { Plot.AxisAuto(); Render(); }
        private void RightClickMenu_SaveImage_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = "ScottPlot.png",
                Filter = "PNG Files (*.png)|*.png;*.png" +
                         "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                         "|BMP Files (*.bmp)|*.bmp;*.bmp" +
                         "|All files (*.*)|*.*"
            };

            if (sfd.ShowDialog() is true)
                Plot.SaveFig(sfd.FileName);
        }
    }
}
