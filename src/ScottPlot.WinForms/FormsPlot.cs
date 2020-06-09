using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot
{

    public partial class FormsPlot : UserControl
    {
        public Plot plt { get; private set; }
        private Settings settings;
        private bool isDesignerMode;
        public Cursor cursor = Cursors.Arrow;

        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                pbPlot.BackColor = value;
            }
        }

        public FormsPlot(Plot plt)
        {
            InitializeComponent();
            Reset(plt);
        }

        public FormsPlot()
        {
            InitializeComponent();
            Reset(null);
        }

        public void Reset()
        {
            Reset(null);
        }

        public void Reset(Plot plt)
        {
            if (plt is null)
            {
                this.plt = new Plot();
                InitializeScottPlot();
            }
            else
            {
                this.plt = plt;
                InitializeScottPlot(applyDefaultStyle: false);
            }
            Render();
        }

        private void InitializeScottPlot(bool applyDefaultStyle = true)
        {
            ContextMenuStrip = DefaultRightClickMenu();

            pbPlot.MouseWheel += PbPlot_MouseWheel;

            isDesignerMode = Process.GetCurrentProcess().ProcessName == "devenv";
            lblTitle.Visible = isDesignerMode;
            lblVersion.Visible = isDesignerMode;
            lblVersion.Text = Tools.GetVersionString();
            pbPlot.BackColor = ColorTranslator.FromHtml("#003366");
            lblTitle.BackColor = ColorTranslator.FromHtml("#003366");
            lblVersion.BackColor = ColorTranslator.FromHtml("#003366");

            if (applyDefaultStyle)
                plt.Style(Style.Control);

            settings = plt.GetSettings(showWarning: false);

            PbPlot_MouseUp(null, null);
            PbPlot_SizeChanged(null, null);
        }

        private ContextMenuStrip DefaultRightClickMenu()
        {
            var cms = new ContextMenuStrip();
            cms.Items.Add(new ToolStripMenuItem("Save Image", null, new EventHandler(SaveImage)));
            cms.Items.Add(new ToolStripMenuItem("Copy Image", null, new EventHandler(CopyImage)));
            cms.Items.Add(new ToolStripMenuItem("Open in New Window", null, new EventHandler(OpenNewWindow)));
            cms.Items.Add(new ToolStripMenuItem("Settings", null, new EventHandler(OpenSettingsWindow)));
            cms.Items.Add(new ToolStripMenuItem("Help", null, new EventHandler(OpenHelpWindow)));
            return cms;
        }

        private bool currentlyRendering;
        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false, bool recalculateLayout = false, bool processEvents = false)
        {
            if (isDesignerMode)
                return;

            if (recalculateLayout)
                plt.TightenLayout();

            if (equalAxes)
                plt.AxisEqual();

            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                pbPlot.Image = plt?.GetBitmap(true, lowQuality);
                if (isPanningOrZooming || isMovingDraggable || processEvents)
                    Application.DoEvents();
                currentlyRendering = false;
                Rendered?.Invoke(this, EventArgs.Empty);
            }
        }

        private void PbPlot_SizeChanged(object sender, EventArgs e)
        {
            if (plt is null)
                return;

            plt.Resize(Width, Height);
            Render(skipIfCurrentlyRendering: false);
        }

        #region user control configuration

        private bool enablePanning = true;
        private bool enableRightClickZoom = true;
        private bool enableScrollWheelZoom = true;
        private bool lowQualityWhileDragging = true;
        private bool doubleClickingTogglesBenchmark = true;
        private bool lockVerticalAxis = false;
        private bool lockHorizontalAxis = false;
        private bool equalAxes = false;
        private double middleClickMarginX = .1;
        private double middleClickMarginY = .1;
        private bool? recalculateLayoutOnMouseUp = null;
        private bool showCoordinatesTooltip = false;
        public void Configure(
            bool? enablePanning = null,
            bool? enableZooming = null,
            bool? enableRightClickMenu = null,
            bool? enableScrollWheelZoom = null,
            bool? lowQualityWhileDragging = null,
            bool? enableDoubleClickBenchmark = null,
            bool? lockVerticalAxis = null,
            bool? lockHorizontalAxis = null,
            bool? equalAxes = null,
            double? middleClickMarginX = null,
            double? middleClickMarginY = null,
            bool? recalculateLayoutOnMouseUp = null,
            bool? showCoordinatesTooltip = null
            )
        {
            if (enablePanning != null) this.enablePanning = (bool)enablePanning;
            if (enableZooming != null) this.enableRightClickZoom = (bool)enableZooming;
            if (enableRightClickMenu != null) ContextMenuStrip = (enableRightClickMenu.Value) ? DefaultRightClickMenu() : null;
            if (enableScrollWheelZoom != null) this.enableScrollWheelZoom = (bool)enableScrollWheelZoom;
            if (lowQualityWhileDragging != null) this.lowQualityWhileDragging = (bool)lowQualityWhileDragging;
            if (enableDoubleClickBenchmark != null) this.doubleClickingTogglesBenchmark = (bool)enableDoubleClickBenchmark;
            if (lockVerticalAxis != null) this.lockVerticalAxis = (bool)lockVerticalAxis;
            if (lockHorizontalAxis != null) this.lockHorizontalAxis = (bool)lockHorizontalAxis;
            if (equalAxes != null) this.equalAxes = (bool)equalAxes;
            this.middleClickMarginX = middleClickMarginX ?? this.middleClickMarginX;
            this.middleClickMarginY = middleClickMarginY ?? this.middleClickMarginY;
            this.recalculateLayoutOnMouseUp = recalculateLayoutOnMouseUp;
            this.showCoordinatesTooltip = showCoordinatesTooltip ?? this.showCoordinatesTooltip;
        }

        private bool isShiftPressed { get { return (ModifierKeys.HasFlag(Keys.Shift) || (lockHorizontalAxis)); } }
        private bool isCtrlPressed { get { return (ModifierKeys.HasFlag(Keys.Control) || (lockVerticalAxis)); } }

        #endregion

        #region mouse tracking

        ToolTip tooltip = new ToolTip();
        private Point? mouseLeftDownLocation, mouseRightDownLocation, mouseMiddleDownLocation;
        double[] axisLimitsOnMouseDown;
        private bool isPanningOrZooming
        {
            get
            {
                if (axisLimitsOnMouseDown is null) return false;
                if (mouseLeftDownLocation != null) return true;
                else if (mouseRightDownLocation != null) return true;
                else if (mouseMiddleDownLocation != null) return true;
                return false;
            }
        }

        IDraggable plottableBeingDragged = null;
        private bool isMovingDraggable { get { return (plottableBeingDragged != null); } }

        private Cursor GetCursor(Config.Cursor scottPlotCursor)
        {
            switch (scottPlotCursor)
            {
                case Config.Cursor.Arrow: return Cursors.Arrow;
                case Config.Cursor.WE: return Cursors.SizeWE;
                case Config.Cursor.NS: return Cursors.SizeNS;
                case Config.Cursor.All: return Cursors.SizeAll;
                default: return Cursors.Help;
            }
        }

        private void PbPlot_MouseDown(object sender, MouseEventArgs e)
        {
            var mousePixel = e.Location;
            plottableBeingDragged = plt.GetDraggableUnderMouse(mousePixel.X, mousePixel.Y);

            if (plottableBeingDragged is null)
            {
                // MouseDown event is to start a pan or zoom
                if (e.Button == MouseButtons.Left && ModifierKeys.HasFlag(Keys.Alt)) mouseMiddleDownLocation = e.Location;
                else if (e.Button == MouseButtons.Left && enablePanning) mouseLeftDownLocation = e.Location;
                else if (e.Button == MouseButtons.Right && enableRightClickZoom) mouseRightDownLocation = e.Location;
                else if (e.Button == MouseButtons.Middle && enableScrollWheelZoom) mouseMiddleDownLocation = e.Location;
                axisLimitsOnMouseDown = plt.Axis();
            }
            else
            {
                // mouse is being used to drag a plottable
                OnMouseDownOnPlottable(EventArgs.Empty);
            }

            base.OnMouseDown(e);
        }

        [Obsolete("use Plot.CoordinateFromPixelX() and Plot.CoordinateFromPixelY()")]
        public PointF mouseCoordinates { get { return plt.CoordinateFromPixel(mouseLocation); } }
        Point mouseLocation;

        private int dpiScale = 1; //Heres hoping that WinForms becomes DPI aware

        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            OnMouseMoved(e);

            tooltip.Hide(this);
            if (isPanningOrZooming)
                MouseMovedToPanOrZoom(e);
            else if (isMovingDraggable)
                MouseMovedToMoveDraggable(e);
            else
                MouseMovedWithoutInteraction(e);

            base.OnMouseMove(e);
        }


        private void MouseMovedToPanOrZoom(MouseEventArgs e)
        {
            plt.Axis(axisLimitsOnMouseDown);

            if (mouseLeftDownLocation != null)
            {
                // left-click-drag panning
                int deltaX = ((Point)mouseLeftDownLocation).X - e.Location.X;
                int deltaY = e.Location.Y - ((Point)mouseLeftDownLocation).Y;

                if (isCtrlPressed) deltaY = 0;
                if (isShiftPressed) deltaX = 0;

                settings.AxesPanPx(deltaX, deltaY);
                OnAxisChanged();
            }
            else if (mouseRightDownLocation != null)
            {
                // right-click-drag zooming
                int deltaX = ((Point)mouseRightDownLocation).X - e.Location.X;
                int deltaY = e.Location.Y - ((Point)mouseRightDownLocation).Y;

                if (isCtrlPressed == true && isShiftPressed == false) deltaY = 0;
                if (isShiftPressed == true && isCtrlPressed == false) deltaX = 0;

                settings.AxesZoomPx(-deltaX, -deltaY, lockRatio: isCtrlPressed && isShiftPressed);
                OnAxisChanged();
            }
            else if (mouseMiddleDownLocation != null)
            {
                // middle-click-drag zooming to rectangle
                int x1 = Math.Min(e.Location.X, ((Point)mouseMiddleDownLocation).X);
                int x2 = Math.Max(e.Location.X, ((Point)mouseMiddleDownLocation).X);
                int y1 = Math.Min(e.Location.Y, ((Point)mouseMiddleDownLocation).Y);
                int y2 = Math.Max(e.Location.Y, ((Point)mouseMiddleDownLocation).Y);

                Point origin = new Point(x1 - settings.dataOrigin.X, y1 - settings.dataOrigin.Y);
                Size size = new Size(x2 - x1, y2 - y1);

                if (lockVerticalAxis)
                {
                    origin.Y = 0;
                    size.Height = settings.dataSize.Height - 1;
                }
                if (lockHorizontalAxis)
                {
                    origin.X = 0;
                    size.Width = settings.dataSize.Width - 1;
                }

                settings.mouseMiddleRect = new Rectangle(origin, size);
            }

            Render(true, lowQuality: lowQualityWhileDragging);
            return;
        }

        public (double x, double y) GetMouseCoordinates()
        {
            double x = plt.CoordinateFromPixelX(mouseLocation.X / dpiScale);
            double y = plt.CoordinateFromPixelY(mouseLocation.Y / dpiScale);
            return (x, y);
        }

        private void MouseMovedToMoveDraggable(MouseEventArgs e)
        {
            plottableBeingDragged.DragTo(plt.CoordinateFromPixelX(e.Location.X), plt.CoordinateFromPixelY(e.Location.Y));
            OnMouseDragPlottable(EventArgs.Empty);
            Render(true, lowQuality: lowQualityWhileDragging);
        }

        private void MouseMovedWithoutInteraction(MouseEventArgs e)
        {
            if (showCoordinatesTooltip)
            {
                double coordX = plt.CoordinateFromPixelX(e.Location.X);
                double coordY = plt.CoordinateFromPixelY(e.Location.Y);
                tooltip.Show($"{coordX:N2}, {coordY:N2}", this, e.Location.X + 15, e.Location.Y);
            }

            // set the cursor based on what's beneath it
            var draggableUnderCursor = plt.GetDraggableUnderMouse(e.Location.X, e.Location.Y);
            var spCursor = (draggableUnderCursor is null) ? Config.Cursor.Arrow : draggableUnderCursor.DragCursor;
            pbPlot.Cursor = GetCursor(spCursor);
        }

        private void PbPlot_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseMiddleDownLocation != null)
            {
                int x1 = Math.Min(e.Location.X, ((Point)mouseMiddleDownLocation).X);
                int x2 = Math.Max(e.Location.X, ((Point)mouseMiddleDownLocation).X);
                int y1 = Math.Min(e.Location.Y, ((Point)mouseMiddleDownLocation).Y);
                int y2 = Math.Max(e.Location.Y, ((Point)mouseMiddleDownLocation).Y);

                Point topLeft = new Point(x1, y1);
                Size size = new Size(x2 - x1, y2 - y1);
                Point botRight = new Point(topLeft.X + size.Width, topLeft.Y + size.Height);

                if ((size.Width > 2) && (size.Height > 2))
                {
                    // only change axes if suffeciently large square was drawn
                    if (!lockHorizontalAxis)
                        plt.Axis(
                            x1: plt.CoordinateFromPixelX(topLeft.X),
                            x2: plt.CoordinateFromPixelX(botRight.X));
                    if (!lockVerticalAxis)
                        plt.Axis(
                            y1: plt.CoordinateFromPixelY(botRight.Y),
                            y2: plt.CoordinateFromPixelY(topLeft.Y));
                    OnAxisChanged();
                }
                else
                {
                    bool shouldTighten = recalculateLayoutOnMouseUp ?? !plt.containsHeatmap;
                    plt.AxisAuto(middleClickMarginX, middleClickMarginY, tightenLayout: shouldTighten);
                    OnAxisChanged();
                }
            }

            if (mouseRightDownLocation != null)
            {
                int deltaX = Math.Abs(((Point)mouseRightDownLocation).X - e.Location.X);
                int deltaY = Math.Abs(((Point)mouseRightDownLocation).Y - e.Location.Y);
                bool mouseDraggedFar = (deltaX > 3 || deltaY > 3);

                if (mouseDraggedFar)
                    ContextMenuStrip?.Hide();
                else
                    OnMenuDeployed();
            }

            if (isPanningOrZooming)
            {
                OnMouseDragged(EventArgs.Empty);
            }

            if (plottableBeingDragged != null)
            {
                OnMouseDropPlottable(EventArgs.Empty);
            }

            OnMouseClicked(e);
            base.OnMouseUp(e);

            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
            settings.mouseMiddleRect = null;
            plottableBeingDragged = null;

            bool shouldRecalculate = recalculateLayoutOnMouseUp ?? !plt.containsHeatmap;
            Render(recalculateLayout: shouldRecalculate);
        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClicked(e);
            base.OnMouseDoubleClick(e);
        }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            if (enableScrollWheelZoom == false)
                return;

            double xFrac = (e.Delta > 0) ? 1.15 : 0.85;
            double yFrac = (e.Delta > 0) ? 1.15 : 0.85;

            if (isCtrlPressed) yFrac = 1;
            if (isShiftPressed) xFrac = 1;

            plt.AxisZoom(xFrac, yFrac, plt.CoordinateFromPixelX(e.Location.X), plt.CoordinateFromPixelY(e.Location.Y));

            bool shouldRecalculate = recalculateLayoutOnMouseUp ?? !plt.containsHeatmap;
            Render(recalculateLayout: shouldRecalculate);
            OnAxisChanged();

            base.OnMouseWheel(e);
        }

        #endregion

        #region menus and forms

        private void SaveImage(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "ScottPlot.png";
            savefile.Filter = "PNG Files (*.png)|*.png;*.png";
            savefile.Filter += "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            savefile.Filter += "|BMP Files (*.bmp)|*.bmp;*.bmp";
            savefile.Filter += "|TIF files (*.tif, *.tiff)|*.tif;*.tiff";
            savefile.Filter += "|All files (*.*)|*.*";
            if (savefile.ShowDialog() == DialogResult.OK)
                plt.SaveFig(savefile.FileName);
        }

        private void CopyImage(object sender, EventArgs e)
        {
            Clipboard.SetImage(plt.GetBitmap(true));
        }

        private void OpenSettingsWindow(object sender, EventArgs e)
        {
            new UserControls.FormSettings(plt).ShowDialog();
            Render();
        }

        private void OpenHelpWindow(object sender, EventArgs e)
        {
            new UserControls.FormHelp().ShowDialog();
        }

        private void OpenNewWindow(object sender, EventArgs e)
        {
            new FormsPlotViewer(plt.Copy()).Show();
        }

        #endregion

        #region custom events

        public event EventHandler Rendered;
        public event EventHandler MouseDownOnPlottable;
        public event EventHandler MouseDragPlottable;
        public event MouseEventHandler MouseMoved;
        public event EventHandler MouseDragged;
        public event EventHandler MouseDropPlottable;
        public event EventHandler AxesChanged;
        public event MouseEventHandler MouseClicked;
        public event MouseEventHandler MouseDoubleClicked;
        public event MouseEventHandler MenuDeployed;
        protected virtual void OnMouseDownOnPlottable(EventArgs e) { MouseDownOnPlottable?.Invoke(this, e); }
        protected virtual void OnMouseDragPlottable(EventArgs e) { MouseDragPlottable?.Invoke(this, e); }
        protected virtual void OnMouseMoved(MouseEventArgs e) { MouseMoved?.Invoke(this, e); }
        protected virtual void OnMouseDragged(EventArgs e) { MouseDragged?.Invoke(this, e); }
        protected virtual void OnMouseDropPlottable(EventArgs e) { MouseDropPlottable?.Invoke(this, e); }
        protected virtual void OnMouseClicked(MouseEventArgs e) { MouseClicked?.Invoke(this, e); }
        protected virtual void OnAxisChanged() { AxesChanged?.Invoke(this, null); }

        protected virtual void OnMouseDoubleClicked(MouseEventArgs e)
        {
            MouseDoubleClicked?.Invoke(this, e);
            if (doubleClickingTogglesBenchmark)
            {
                plt.Benchmark(toggle: true);
                Render();
            }
        }

        protected virtual void OnMenuDeployed()
        {
            MenuDeployed?.Invoke(this, null);
        }

        #endregion
    }
}
