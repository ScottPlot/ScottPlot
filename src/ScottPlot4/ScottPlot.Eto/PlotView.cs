using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Eto
{
    public class PlotView : ImageView, ScottPlot.Control.IPlotControl // ported from ScottPlot..WpPlot.axaml.cs v4.1.27
    {
        /// <summary>
        /// This is the plot displayed by the user control.
        /// After modifying it you may need to call Render() to request the plot be redrawn on the screen.
        /// </summary>
        public Plot Plot => Backend.Plot;

        /// <summary>
        /// This object can be used to modify advanced behaior and customization of this user control.
        /// </summary>
        public Control.Configuration Configuration { get; }

        /// <summary>
        /// This event is invoked any time the axis limits are modified.
        /// </summary>
        public event EventHandler? AxesChanged;

        /// <summary>
        /// This event is invoked any time the plot is right-clicked.
        /// By default it contains DefaultRightClickEvent(), but you can remove this and add your own method.
        /// </summary>
        public event EventHandler? RightClicked;

        /// <summary>
        /// This event is invoked any time the plot is left-clicked.
        /// It is typically used to interact with custom plot types.
        /// </summary>
        public event EventHandler? LeftClicked;

        /// <summary>
        /// This event is invoked when a <seealso cref="Plottable.IHittable"/> plottable is left-clicked.
        /// </summary>
        public event EventHandler? LeftClickedPlottable;

        /// <summary>
        /// This event is invoked after the mouse moves while dragging a draggable plottable.
        /// The object passed is the plottable being dragged.
        /// </summary>
        public event EventHandler? PlottableDragged;

        /// <summary>
        /// This event is invoked right after a draggable plottable was dropped.
        /// The object passed is the plottable that was just dropped.
        /// </summary>
        public event EventHandler? PlottableDropped;

        private readonly Control.ControlBackEnd Backend;
        private readonly Dictionary<Cursor, global::Eto.Forms.Cursor> Cursors;
        public PlotView()
        {
            Backend = new Control.ControlBackEnd(1, 1, GetType().Name);
            Backend.Resize(Width, Height, useDelayedRendering: false);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            Backend.LeftClicked += new EventHandler(OnLeftClicked);
            Backend.LeftClickedPlottable += new EventHandler(OnLeftClickedPlottable);
            Backend.AxesChanged += new EventHandler(OnAxesChanged);
            Backend.PlottableDragged += new EventHandler(OnPlottableDragged);
            Backend.PlottableDropped += new EventHandler(OnPlottableDropped);
            Configuration = Backend.Configuration;

            Cursors = new Dictionary<ScottPlot.Cursor, global::Eto.Forms.Cursor>()
            {
                [ScottPlot.Cursor.Arrow] = global::Eto.Forms.Cursors.Default,
                [ScottPlot.Cursor.WE] = global::Eto.Forms.Cursors.HorizontalSplit,
                [ScottPlot.Cursor.NS] = global::Eto.Forms.Cursors.VerticalSplit,
                [ScottPlot.Cursor.All] = global::Eto.Forms.Cursors.HorizontalSplit,
                [ScottPlot.Cursor.Crosshair] = global::Eto.Forms.Cursors.Crosshair,
                [ScottPlot.Cursor.Hand] = global::Eto.Forms.Cursors.Pointer,
                [ScottPlot.Cursor.Question] = global::Eto.Forms.Cursors.Default,
            };

            BackgroundColor = Colors.Transparent;
            Plot.Style(figureBackground: System.Drawing.Color.Transparent);

            SizeChanged += OnSizeChanged;
            MouseDoubleClick += OnMouseDoubleClick;
            MouseWheel += OnMouseWheel;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;

            RightClicked += DefaultRightClickEvent;

            Backend.Configuration.UseRenderQueue = true;
            Backend.StartProcessingEvents();
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
        public void Reset() => Backend.Reset(Width, Height);

        /// <summary>
        /// Reset this control by replacing the current plot with an existing plot
        /// </summary>
        public void Reset(Plot newPlot) => Backend.Reset(Width, Height, newPlot);

        /// <summary>
        /// Re-render the plot and update the image displayed by this control.
        /// </summary>
        public void Refresh() => Refresh(false);

        /// <summary>
        /// Re-render the plot and update the image displayed by this control.
        /// </summary>
        /// <param name="lowQuality">disable anti-aliasing to produce faster (but lower quality) plots</param>
        public void Refresh(bool lowQuality = false)
        {
            Backend.WasManuallyRendered = true;
            Backend.Render(lowQuality);
        }

        /// <summary>
        /// Request the control to refresh the next time it is available.
        /// This method does not block the calling thread.
        /// </summary>
        public void RefreshRequest(RenderType renderType = RenderType.LowQualityThenHighQualityDelayed)
        {
            Backend.WasManuallyRendered = true;
            Backend.RenderRequest(renderType);
        }

        private void OnBitmapChanged(object? sender, EventArgs e) => ReplacePlotBitmap(Backend.GetLatestBitmap());
        private void OnBitmapUpdated(object? sender, EventArgs e) => UpdatePlotBitmap(Backend.GetLatestBitmap());
        private void OnCursorChanged(object? sender, EventArgs e) => Cursor = Cursors[Backend.Cursor];
        private void OnRightClicked(object? sender, EventArgs e) => RightClicked?.Invoke(this, e);
        private void OnLeftClicked(object sender, EventArgs e) => LeftClicked?.Invoke(this, e);
        private void OnLeftClickedPlottable(object sender, EventArgs e) => LeftClickedPlottable?.Invoke(sender, e);
        private void OnPlottableDragged(object? sender, EventArgs e) => PlottableDragged?.Invoke(sender, e);
        private void OnPlottableDropped(object? sender, EventArgs e) => PlottableDropped?.Invoke(sender, e);
        private void OnAxesChanged(object? sender, EventArgs e) => AxesChanged?.Invoke(this, e);
        private void OnSizeChanged(object? sender, EventArgs e) => Backend.Resize(Width, Height, useDelayedRendering: true);
        private void OnMouseDown(object? sender, MouseEventArgs e) => Backend.MouseDown(GetInputState(e));
        private void OnMouseUp(object? sender, MouseEventArgs e) => Backend.MouseUp(GetInputState(e));
        private void OnMouseDoubleClick(object? sender, MouseEventArgs e) => Backend.DoubleClick();
        private void OnMouseWheel(object? sender, MouseEventArgs e) => Backend.MouseWheel(GetInputState(e));
        private void OnMouseMove(object? sender, MouseEventArgs e) => Backend.MouseMove(GetInputState(e));
        private Control.InputState GetInputState(MouseEventArgs e) =>
             new()
             {
                 X = e.Location.X,
                 Y = e.Location.Y,
                 LeftWasJustPressed = e.Buttons == MouseButtons.Primary,
                 RightWasJustPressed = e.Buttons == MouseButtons.Alternate,
                 MiddleWasJustPressed = e.Buttons == MouseButtons.Middle,
                 ShiftDown = e.Modifiers.HasFlag(Keys.Shift),
                 CtrlDown = e.Modifiers.HasFlag(Keys.Control),
                 AltDown = e.Modifiers.HasFlag(Keys.Alt),
                 WheelScrolledUp = e.Delta.Height > 0,
                 WheelScrolledDown = e.Delta.Height < 0,
             };

        /// <summary>
        /// Replace the existing PlotBitmap with a new one.
        /// </summary>
        public void ReplacePlotBitmap(System.Drawing.Bitmap bmp)
        {
            this.Image = bmp.ToEto();
        }

        /// <summary>
        /// Update the PlotBitmap with pixel data from the latest render.
        /// If a PlotBitmap does not exist one will be created.
        /// </summary>
        private void UpdatePlotBitmap(System.Drawing.Bitmap bmp)
        {
            this.Image = bmp.ToEto();
        }

        /// <summary>
        /// Launch the default right-click menu.
        /// </summary>
        public void DefaultRightClickEvent(object? sender, EventArgs e)
        {
            var cm = new ContextMenu();

            ButtonMenuItem SaveImageMenuItem = new() { Text = "Save Image" };
            SaveImageMenuItem.Click += RightClickMenu_SaveImage_Click;
            cm.Items.Add(SaveImageMenuItem);

            ButtonMenuItem CopyImageMenuItem = new() { Text = "Copy Image" };
            CopyImageMenuItem.Click += RightClickMenu_Copy_Click;
            cm.Items.Add(CopyImageMenuItem);

            cm.Items.AddSeparator();

            ButtonMenuItem AutoAxisMenuItem = new() { Text = "Zoom to Fit Data" };
            AutoAxisMenuItem.Click += RightClickMenu_AutoAxis_Click;
            cm.Items.Add(AutoAxisMenuItem);

            cm.Items.AddSeparator();

            ButtonMenuItem HelpMenuItem = new() { Text = "Help" };
            HelpMenuItem.Click += RightClickMenu_Help_Click;
            cm.Items.Add(HelpMenuItem);

            cm.Items.AddSeparator();

            ButtonMenuItem OpenInNewWindowMenuItem = new() { Text = "Open in New Window" };
            OpenInNewWindowMenuItem.Click += RightClickMenu_OpenInNewWindow_Click;
            cm.Items.Add(OpenInNewWindowMenuItem);

            cm.Show();
        }

        private void RightClickMenu_Copy_Click(object? sender, EventArgs e) => Clipboard.Instance.Image = Plot.Render().ToEto();
        private void RightClickMenu_Help_Click(object? sender, EventArgs e) => new FormHelp().Show();
        private void RightClickMenu_OpenInNewWindow_Click(object? sender, EventArgs e) => new PlotViewForm(Plot).Show();
        private void RightClickMenu_AutoAxis_Click(object? sender, EventArgs e) { Plot.AxisAuto(); Refresh(); }
        private void RightClickMenu_SaveImage_Click(object? sender, EventArgs e)
        {
            var sfd = new SaveFileDialog { FileName = "ScottPlot.png" };

            sfd.Filters.Add(new FileFilter("PNG Files (*.png)", ".png"));
            sfd.Filters.Add(new FileFilter("JPG Files (*.jpg, *.jpeg)", ".jpg", ".jpeg"));
            sfd.Filters.Add(new FileFilter("BMP Files (*.bmp)", ".bmp"));
            sfd.Filters.Add(new FileFilter("All files (*.*)", ".*"));

            if (sfd.ShowDialog(this) == DialogResult.Ok)
                Plot.SaveFig(sfd.FileName);
        }
    }
}
