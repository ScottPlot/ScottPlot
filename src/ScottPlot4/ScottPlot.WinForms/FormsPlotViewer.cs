using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class FormsPlotViewer : Form
    {
        private readonly FormsPlot ParentControl;
        public enum SyncType { NoSync, SyncToParent, SyncFromParent };

        public FormsPlotViewer(ScottPlot.Plot plot, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer")
        {
            InitializeComponent();
            Width = windowWidth;
            Height = windowHeight;
            Text = windowTitle;

            formsPlot1.Reset(plot);
            formsPlot1.Refresh();
        }

        // CALL THIS OVERLOAD WHEN LAUNCHING A NEW WINDOW FROM AN EXISTING FORM
        public FormsPlotViewer(FormsPlot parent, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer", SyncType syncType = SyncType.NoSync)
        {
            InitializeComponent();
            Width = windowWidth;
            Height = windowHeight;
            Text = windowTitle;

            ParentControl = parent;
            formsPlot1.Reset(ParentControl.Plot);
            formsPlot1.Refresh();

            switch (syncType)
            {
                case SyncType.NoSync:
                    break;

                case SyncType.SyncToParent:
                    formsPlot1.AxesChanged += (s, e) => ParentControl.Refresh();
                    break;

                case SyncType.SyncFromParent:
                    parent.BitmapChangedOrUpdated += (s, e) => formsPlot1.Refresh();
                    break;
            }
        }
    }
}
