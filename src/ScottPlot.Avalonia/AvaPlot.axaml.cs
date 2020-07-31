using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using ScottPlot.Avalonia;
using ScottPlot.Config;
using Avalonia.Platform;

using Ava = Avalonia;
using ScottPlot.Control;

namespace ScottPlot.Avalonia
{
    /// <summary>
    /// Interaction logic for AvaPlot.axaml
    /// </summary>

    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class AvaPlot : UserControl
    {
        public Plot plt { get { return backend.plt; } }
        private Settings settings;
        private bool isDesignerMode;
        private double dpiScaleInput = 1;
        private double dpiScaleOutput = 1;
        internal readonly SolidColorBrush transparentBrush = new SolidColorBrush(Ava.Media.Color.FromUInt32(0), 0);
        internal AvaPlotBackend backend;

        public AvaPlot(Plot plt)
        {
            InitializeComponent();
            backend = new AvaPlotBackend(this);
            SetContextMenu(backend.DefaultRightClickMenu());
            backend.Reset(plt);
        }

        public AvaPlot()
        {
            InitializeComponent();
            backend = new AvaPlotBackend(this);
            SetContextMenu(backend.DefaultRightClickMenu());
            backend.Reset(null);
        }

        public void SetContextMenu(List<ContextMenuItem> contextMenuItems)
        {
            backend.contextMenuItems = contextMenuItems;
            var cm = new ContextMenu();

            List<MenuItem> menuItems = new List<MenuItem>();
            foreach (var curr in contextMenuItems)
            {
                var menuItem = new MenuItem() { Header = curr.itemName };
                menuItem.Click += (object sender, RoutedEventArgs e) => curr.onClick();
                menuItems.Add(menuItem);
            }
            cm.Items = menuItems;

            ContextMenu = cm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.Focusable = true;

            PointerPressed += UserControl_MouseDown;
            PointerMoved += UserControl_MouseMove;
            PointerReleased += UserControl_MouseUp;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            PointerWheelChanged += UserControl_MouseWheel;
        }

        public void Render()
        {
            backend.Render();
        }

        #region user control configuration

        public void Configure(
            bool? enablePanning = null,
            bool? enableRightClickZoom = null,
            bool? enableRightClickMenu = null,
            bool? enableScrollWheelZoom = null,
            bool? lowQualityWhileDragging = null,
            bool? enableDoubleClickBenchmark = null,
            bool? lockVerticalAxis = null,
            bool? lockHorizontalAxis = null,
            bool? equalAxes = null,
            double? middleClickMarginX = null,
            double? middleClickMarginY = null,
            bool? recalculateLayoutOnMouseUp = null
            )
        {
            backend.Configure(enablePanning, enableRightClickZoom, enableRightClickMenu, enableScrollWheelZoom, lowQualityWhileDragging, enableDoubleClickBenchmark,
                lockVerticalAxis, lockHorizontalAxis, equalAxes, middleClickMarginX, middleClickMarginY, recalculateLayoutOnMouseUp);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftAlt:
                case Key.RightAlt:
                    backend.SetAltPressed(true);
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                    backend.SetShiftPressed(true);
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    backend.SetCtrlPressed(true);
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftAlt:
                case Key.RightAlt:
                    backend.SetAltPressed(false);
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                    backend.SetShiftPressed(false);
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    backend.SetCtrlPressed(false);
                    break;
            }
        }
        #endregion

        #region mouse tracking

        private Ava.Point GetPixelPosition(PointerEventArgs e)
        {
            Ava.Point pos = e.GetPosition(this);
            Ava.Point dpiCorrectedPos = new Ava.Point(pos.X * dpiScaleInput, pos.Y * dpiScaleInput);
            return dpiCorrectedPos;
        }

        private System.Drawing.Point SDPoint(Ava.Point pt)
        {
            return new System.Drawing.Point((int)pt.X, (int)pt.Y);
        }

        private System.Drawing.PointF SDPointF(Ava.Point pt)
        {
            return new System.Drawing.PointF((float)pt.X, (float)pt.Y);
        }

        void UserControl_MouseDown(object sender, PointerPressedEventArgs e)
        {
            e.Pointer.Capture(this);

            var mousePixel = GetPixelPosition(e);
            MouseButtons button = MouseButtons.Left;
            if (e.MouseButton == MouseButton.Left) button = MouseButtons.Left;
            else if (e.MouseButton == MouseButton.Right) button = MouseButtons.Right;
            else if (e.MouseButton == MouseButton.Middle) button = MouseButtons.Middle;


            backend.MouseDrag(SDPointF(mousePixel), button);
        }

        private void UserControl_MouseMove(object sender, PointerEventArgs e)
        {
            backend.MouseMove(SDPointF(GetPixelPosition(e)));
        }

        public (double x, double y) GetMouseCoordinates() => backend.GetMouseCoordinates();

        private void UserControl_MouseUp(object sender, PointerEventArgs e)
        {
            e.Pointer.Capture(null);
            var mouseLocation = GetPixelPosition(e);

            if (backend.mouseRightDownLocation != null)
            {
                double deltaX = Math.Abs(mouseLocation.X - backend.mouseRightDownLocation.Value.X);
                double deltaY = Math.Abs(mouseLocation.Y - backend.mouseRightDownLocation.Value.Y);
                bool mouseDraggedFar = (deltaX > 3 || deltaY > 3);
                if (mouseDraggedFar)
                {
                    e.Handled = true; //I wish I was bullshitting you but this is the only way to prevent opening the context menu that works in Avalonia right now
                }
            }
            else
            {
                if (ContextMenu != null)
                {
                    ContextMenu.Close();
                }
            }

            backend.MouseUp();


        }

        #endregion

        #region mouse clicking

        private void UserControl_MouseWheel(object sender, PointerWheelEventArgs e)
        {
            backend.MouseWheel(e.Delta.Y);
        }

        #endregion

        #region event handling

        public event EventHandler Rendered;
        public event EventHandler AxisChanged;

        #endregion
    }
}
