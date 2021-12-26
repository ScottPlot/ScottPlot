using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class ColormapViewer : Form
    {
        private void InitializeComponent()
        {
            this.lbColormapNames = new ListBox();
            this.lblColormap = new Label();
            this.pbColormap = new ImageView();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.formsPlot2 = new ScottPlot.Eto.PlotView();
            this.rbData = new RadioButton();
            this.rbImage = new RadioButton(rbData);
            this.formsPlot3 = new ScottPlot.Eto.PlotView();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(lblColormap, null, rbData, rbImage);
            layout.AddSeparateRow(pbColormap, null);
            layout.AddSeparateRow(formsPlot1, formsPlot2);
            layout.AddSeparateRow(formsPlot3);

            this.Content = new StackLayout(lbColormapNames, layout) { Orientation = global::Eto.Forms.Orientation.Horizontal, Padding = 5 };
            // 
            // lbColormapNames
            // 
            this.lbColormapNames.Font = Fonts.Sans(12);
            this.lbColormapNames.Size = new Size(148, 633);
            // 
            // lblColormap
            // 
            this.lblColormap.Font = Fonts.Sans(16);
            this.lblColormap.Size = new Size(68, 30);
            this.lblColormap.Text = "label1";
            // 
            // pbColormap
            // 
            this.pbColormap.BackgroundColor = SystemColors.Control;
            this.pbColormap.Size = new Size(432, 37);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Size = new Size(432, 336);
            // 
            // formsPlot2
            // 
            this.formsPlot2.Size = new Size(671, 336);
            // 
            // rbData
            // 
            this.rbData.Font = Fonts.Sans(12);
            this.rbData.Size = new Size(116, 25);
            this.rbData.Text = "Sample Data";
            // 
            // rbImage
            // 
            this.rbImage.Checked = true;
            this.rbImage.Font = Fonts.Sans(12);
            this.rbImage.Size = new Size(127, 25);
            this.rbImage.Text = "Sample Image";
            // 
            // formsPlot3
            // 
            this.formsPlot3.Size = new Size(1109, 215);
            // 
            // ColormapViewer
            // 
            this.ClientSize = new Size(1292, 657);
            this.Title = "Colormap Viewer";
            this.ResumeLayout();
        }

        private ListBox lbColormapNames;
        private Label lblColormap;
        private ImageView pbColormap;
        private ScottPlot.Eto.PlotView formsPlot1;
        private ScottPlot.Eto.PlotView formsPlot2;
        private RadioButton rbData;
        private RadioButton rbImage;
        private ScottPlot.Eto.PlotView formsPlot3;
    }
}
