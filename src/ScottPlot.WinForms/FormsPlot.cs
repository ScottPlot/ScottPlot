using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace ScottPlot
{

    public partial class FormsPlot : UserControl
    {
        public Plot plt { get; private set; }
        private Settings settings;
        private bool isDesignerMode;
        public Cursor cursor = Cursors.Arrow;
        ContextMenuStrip rightClickMenu;

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
            this.plt = (plt is null) ? new Plot() : plt;
            InitializeScottPlot();
            Render();
        }

        private void InitializeScottPlot()
        {
            rightClickMenu = new ContextMenuStrip();
            rightClickMenu.Items.Add("Save Image");
            rightClickMenu.Items.Add("Copy Image");
            rightClickMenu.Items.Add("Settings");
            rightClickMenu.Items.Add("Help");
            rightClickMenu.ItemClicked += new ToolStripItemClickedEventHandler(RightClickMenuItemClicked);

            pbPlot.MouseWheel += PbPlot_MouseWheel;

            isDesignerMode = Process.GetCurrentProcess().ProcessName == "devenv";
            lblTitle.Visible = isDesignerMode;
            lblVersion.Visible = isDesignerMode;
            lblVersion.Text = Tools.GetVersionString();
            pbPlot.BackColor = ColorTranslator.FromHtml("#003366");
            lblTitle.BackColor = ColorTranslator.FromHtml("#003366");
            lblVersion.BackColor = ColorTranslator.FromHtml("#003366");

            plt.Style(Style.Control);
            settings = plt.GetSettings(showWarning: false);

            PbPlot_MouseUp(null, null);
            PbPlot_SizeChanged(null, null);
        }

        private bool currentlyRendering;
        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false)
        {
            if (isDesignerMode)
                return;

            if (equalAxes)
                plt.AxisEqual();

            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                pbPlot.Image = plt?.GetBitmap(true, lowQuality);
                if (isPanningOrZooming || isMovingDraggable)
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
        private bool enableZooming = true;
        private bool enableRightClickMenu = true;
        private bool lowQualityWhileDragging = true;
        private bool doubleClickingTogglesBenchmark = true;
        private bool lockVerticalAxis = false;
        private bool lockHorizontalAxis = false;
        private bool equalAxes = false;
        public void Configure(
            bool? enablePanning = null,
            bool? enableZooming = null,
            bool? enableRightClickMenu = null,
            bool? lowQualityWhileDragging = null,
            bool? enableDoubleClickBenchmark = null,
            bool? lockVerticalAxis = null,
            bool? lockHorizontalAxis = null,
            bool? equalAxes = null
            )
        {
            if (enablePanning != null) this.enablePanning = (bool)enablePanning;
            if (enableZooming != null) this.enableZooming = (bool)enableZooming;
            if (enableRightClickMenu != null) this.enableRightClickMenu = (bool)enableRightClickMenu;
            if (lowQualityWhileDragging != null) this.lowQualityWhileDragging = (bool)lowQualityWhileDragging;
            if (enableDoubleClickBenchmark != null) this.doubleClickingTogglesBenchmark = (bool)enableDoubleClickBenchmark;
            if (lockVerticalAxis != null) this.lockVerticalAxis = (bool)lockVerticalAxis;
            if (lockHorizontalAxis != null) this.lockHorizontalAxis = (bool)lockHorizontalAxis;
            if (lockHorizontalAxis != null) this.equalAxes = (bool)equalAxes;
        }

        private bool isHorizontalLocked { get { return (ModifierKeys.HasFlag(Keys.Alt) || (lockHorizontalAxis)); } }
        private bool isVerticalLocked { get { return (ModifierKeys.HasFlag(Keys.Control) || (lockVerticalAxis)); } }

        #endregion

        #region mouse tracking

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
                if (e.Button == MouseButtons.Left && ModifierKeys.HasFlag(Keys.Shift)) mouseMiddleDownLocation = e.Location;
                else if (e.Button == MouseButtons.Left && enablePanning) mouseLeftDownLocation = e.Location;
                else if (e.Button == MouseButtons.Right && enableZooming) mouseRightDownLocation = e.Location;
                else if (e.Button == MouseButtons.Middle) mouseMiddleDownLocation = e.Location;
                axisLimitsOnMouseDown = plt.Axis();
            }
            else
            {
                // mouse is being used to drag a plottable
                OnMouseDownOnPlottable(EventArgs.Empty);
            }
        }

        [Obsolete("use Plot.CoordinateFromPixelX() and Plot.CoordinateFromPixelY()")]
        public PointF mouseCoordinates { get { return plt.CoordinateFromPixel(mouseLocation); } }
        Point mouseLocation;
        private void PbPlot_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            OnMouseMoved(EventArgs.Empty);

            if (isPanningOrZooming)
                MouseMovedToPanOrZoom(e);
            else if (isMovingDraggable)
                MouseMovedToMoveDraggable(e);
            else
                MouseMovedWithoutInteraction(e);
        }


        private void MouseMovedToPanOrZoom(MouseEventArgs e)
        {
            plt.Axis(axisLimitsOnMouseDown);

            if (mouseLeftDownLocation != null)
            {
                // left-click-drag panning
                int deltaX = ((Point)mouseLeftDownLocation).X - e.Location.X;
                int deltaY = e.Location.Y - ((Point)mouseLeftDownLocation).Y;

                if (isVerticalLocked) deltaY = 0;
                if (isHorizontalLocked) deltaX = 0;

                settings.AxesPanPx(deltaX, deltaY);
            }
            else if (mouseRightDownLocation != null)
            {
                // right-click-drag zooming
                int deltaX = ((Point)mouseRightDownLocation).X - e.Location.X;
                int deltaY = e.Location.Y - ((Point)mouseRightDownLocation).Y;

                if (isVerticalLocked) deltaY = 0;
                if (isHorizontalLocked) deltaX = 0;

                settings.AxesZoomPx(-deltaX, -deltaY);
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

                settings.mouseMiddleRect = new Rectangle(origin, size);
            }

            Render(true, lowQuality: lowQualityWhileDragging);
            return;
        }

        private void MouseMovedToMoveDraggable(MouseEventArgs e)
        {
            PointF coordinate = plt.CoordinateFromPixel(e.Location);
            plottableBeingDragged.DragTo(coordinate.X, coordinate.Y);
            OnMouseDragPlottable(EventArgs.Empty);
            Render(true, lowQuality: lowQualityWhileDragging);
        }

        private void MouseMovedWithoutInteraction(MouseEventArgs e)
        {
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
                    plt.Axis(
                            x1: plt.CoordinateFromPixel(topLeft).X,
                            x2: plt.CoordinateFromPixel(botRight).X,
                            y1: plt.CoordinateFromPixel(botRight).Y,
                            y2: plt.CoordinateFromPixel(topLeft).Y
                        );
                }
                else
                {
                    plt.AxisAuto();
                }
            }

            if (mouseRightDownLocation != null)
            {
                int deltaX = Math.Abs(((Point)mouseRightDownLocation).X - e.Location.X);
                int deltaY = Math.Abs(((Point)mouseRightDownLocation).Y - e.Location.Y);
                if (deltaX < 3 && deltaY < 3)
                {
                    OnMenuDeployed();
                }
            }

            if (isPanningOrZooming)
            {
                OnMouseDragged(EventArgs.Empty);
                OnAxisChanged();
            }

            if (plottableBeingDragged != null)
            {
                OnMouseDropPlottable(EventArgs.Empty);
            }

            OnMouseClicked(e);

            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
            settings.mouseMiddleRect = null;
            plottableBeingDragged = null;

            Render();
        }

        private void PbPlot_MouseDoubleClick(object sender, MouseEventArgs e) { OnMouseDoubleClicked(); }

        private void PbPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            double xFrac = (e.Delta > 0) ? 1.15 : 0.85;
            double yFrac = (e.Delta > 0) ? 1.15 : 0.85;

            if (isVerticalLocked) yFrac = 1;
            if (isHorizontalLocked) xFrac = 1;

            plt.AxisZoom(xFrac, yFrac, plt.CoordinateFromPixel(e.Location));
            Render();
            OnAxisChanged();
        }

        #endregion

        #region menus and forms

        private void RightClickMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            rightClickMenu.Hide();
            switch (e.ClickedItem.Text)
            {
                case "Save Image":
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.FileName = "ScottPlot.png";
                    savefile.Filter = "PNG Files (*.png)|*.png;*.png";
                    savefile.Filter += "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";
                    savefile.Filter += "|BMP Files (*.bmp)|*.bmp;*.bmp";
                    savefile.Filter += "|TIF files (*.tif, *.tiff)|*.tif;*.tiff";
                    savefile.Filter += "|All files (*.*)|*.*";
                    if (savefile.ShowDialog() == DialogResult.OK)
                        plt.SaveFig(savefile.FileName);
                    break;

                case "Settings":
                    var formSettings = new UserControls.FormSettings(plt);
                    formSettings.ShowDialog();
                    Render();
                    break;

                case "Help":
                    var formHelp = new UserControls.FormHelp();
                    formHelp.ShowDialog();
                    break;

                case "Copy Image":
                    Clipboard.SetImage(plt.GetBitmap(true));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region custom events

        public event EventHandler Rendered;
        public event EventHandler MouseDownOnPlottable;
        public event EventHandler MouseDragPlottable;
        public event EventHandler MouseMoved;
        public event EventHandler MouseDragged;
        public event EventHandler MouseDropPlottable;
        public event EventHandler AxesChanged;
        public event MouseEventHandler MouseClicked;
        public event MouseEventHandler MouseDoubleClicked;
        public event MouseEventHandler MenuDeployed;
        protected virtual void OnMouseDownOnPlottable(EventArgs e) { MouseDownOnPlottable?.Invoke(this, e); }
        protected virtual void OnMouseDragPlottable(EventArgs e) { MouseDragPlottable?.Invoke(this, e); }
        protected virtual void OnMouseMoved(EventArgs e) { MouseMoved?.Invoke(this, e); }
        protected virtual void OnMouseDragged(EventArgs e) { MouseDragged?.Invoke(this, e); }
        protected virtual void OnMouseDropPlottable(EventArgs e) { MouseDropPlottable?.Invoke(this, e); }
        protected virtual void OnMouseClicked(MouseEventArgs e) { MouseClicked?.Invoke(this, e); }
        protected virtual void OnAxisChanged() { AxesChanged?.Invoke(this, null); }

        protected virtual void OnMouseDoubleClicked()
        {
            MouseDoubleClicked?.Invoke(this, null);
            if (doubleClickingTogglesBenchmark)
            {
                plt.Benchmark(toggle: true);
                Render();
            }
        }

        protected virtual void OnMenuDeployed()
        {
            MenuDeployed?.Invoke(this, null);

            if (enableRightClickMenu)
                rightClickMenu.Show(pbPlot, PointToClient(System.Windows.Forms.Cursor.Position));
        }

        #endregion
    }
}
