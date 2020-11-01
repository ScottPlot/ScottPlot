using Avalonia.Controls;
using Avalonia.VisualTree;
using ScottPlot.Interactive;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Ava = global::Avalonia;

namespace ScottPlot.Avalonia
{
    internal class AvaPlotBackend : ScottPlot.Interactive.ControlBackend
    {
        private AvaPlot view;
        public AvaPlotBackend(AvaPlot view)
        {
            this.view = view;
        }

        public override void InitializeScottPlot()
        {
            view.Find<TextBlock>("lblVersion").Text = Tools.GetVersionString();
            //isDesignerMode = DesignerProperties.GetIsInDesignMode(this);
            isDesignerMode = false;

            settings = plt.GetSettings(showWarning: false);

            var mainGrid = view.Find<Ava.Controls.Grid>("mainGrid");

            if (isDesignerMode)
            {
                // hide the plot
                mainGrid.RowDefinitions[1].Height = new GridLength(0);
            }
            else
            {
                // hide the version info
                mainGrid.RowDefinitions[0].Height = new GridLength(0);
                //dpiScaleInput = settings.gfxFigure.DpiX / 96; THIS IS ONLY NECESSARY ON WPF
                dpiScaleOutput = settings.gfxFigure.DpiX / 96;
                view.Find<StackPanel>("canvasDesigner").Background = view.transparentBrush;
                var canvasPlot = view.Find<Canvas>("canvasPlot");
                canvasPlot.Background = view.transparentBrush;
                CanvasSizeChanged((int)(canvasPlot.Bounds.Width), (int)(canvasPlot.Bounds.Height));
            }
        }

        public void SetAltPressed(bool value)
        {
            isAltPressed = value;
        }

        public void SetCtrlPressed(bool value)
        {
            isCtrlPressed = value;
        }

        public void SetShiftPressed(bool value)
        {
            isShiftPressed = value;
        }

        public override async void SaveImage()
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.InitialFileName = "ScottPlot.png";

            var filtersPNG = new FileDialogFilter();
            filtersPNG.Name = "PNG Files";
            filtersPNG.Extensions.Add("png");

            var filtersJPEG = new FileDialogFilter();
            filtersJPEG.Name = "JPG Files";
            filtersJPEG.Extensions.Add("jpg");
            filtersJPEG.Extensions.Add("jpeg");

            var filtersBMP = new FileDialogFilter();
            filtersBMP.Name = "BMP Files";
            filtersBMP.Extensions.Add("bmp");

            var filtersTIFF = new FileDialogFilter();
            filtersTIFF.Name = "TIF Files";
            filtersTIFF.Extensions.Add("tif");
            filtersTIFF.Extensions.Add("tiff");

            var filtersGeneric = new FileDialogFilter();
            filtersGeneric.Name = "All Files";
            filtersGeneric.Extensions.Add("*");

            savefile.Filters.Add(filtersPNG);
            savefile.Filters.Add(filtersJPEG);
            savefile.Filters.Add(filtersBMP);
            savefile.Filters.Add(filtersTIFF);
            savefile.Filters.Add(filtersGeneric);


            Task<string> filenameTask = savefile.ShowAsync((Window)view.GetVisualRoot());
            await filenameTask;

            if (filenameTask.Exception != null)
            {
                return;
            }

            if ((filenameTask.Result ?? "") != "")
                plt.SaveFig(filenameTask.Result);
        }

        public override void SetImagePlot(bool lowQuality)
        {
            view.Find<Ava.Controls.Image>("imagePlot").Source = BmpImageFromBmp(plt.GetBitmap(true, lowQuality));
        }

        public override void OpenInNewWindow()
        {
            new AvaPlotViewer(plt.Copy()).Show();
        }

        public override void OpenHelp()
        {
            new HelpWindow().Show();
        }
        public static Ava.Media.Imaging.Bitmap BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new Ava.Media.Imaging.Bitmap(memory);

                return bitmapImage;
            }
        }

        public override List<ContextMenuItem> DefaultRightClickMenu()
        {
            var list = new List<ContextMenuItem>();
            list.Add(new ContextMenuItem()
            {
                itemName = "Save Image",
                onClick = () => SaveImage()
            });
            list.Add(new ContextMenuItem()
            {
                itemName = "Open in New Window",
                onClick = () => OpenInNewWindow()
            });
            list.Add(new ContextMenuItem()
            {
                itemName = "Help",
                onClick = () => OpenHelp()
            });

            return list;
        }

        public override void MouseMovedWithoutInteraction(PointF mouseLocation)
        {
            //nop
        }

        public override void CopyImage()
        {
            //nop
        }
    }

}
