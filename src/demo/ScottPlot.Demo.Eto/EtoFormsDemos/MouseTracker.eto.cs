using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class MouseTracker : Form
    {
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.XPixelLabel = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.YPixelLabel = new Label();
            this.XCoordinateLabel = new Label();
            this.YCoordinateLabel = new Label();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.lblMouse = new Label();
            this.SuspendLayout();

            var layout = new DynamicLayout();
            layout.AddRow(
                new DynamicLayout(new Label() { Text = "*", TextColor = BackgroundColor }, label2, label1),
                new DynamicLayout(label4, XPixelLabel, YPixelLabel),
                new DynamicLayout(label5, XCoordinateLabel, YCoordinateLabel),
                new DynamicLayout(lblMouse)
                );

            layout = new DynamicLayout();
            layout.AddRow(new Label() { Text = "*", TextColor = BackgroundColor }, label4, label5, lblMouse);
            layout.AddRow(label2, XPixelLabel, XCoordinateLabel);
            layout.AddRow(label1, YPixelLabel, YCoordinateLabel);

            layout.SetPaddingRecursive(4);

            this.Content = new DynamicLayout(layout, formsPlot1);

            // 
            // label1
            // 
            this.label1.Font = Fonts.Sans(12);
            this.label1.Size = new Size(20, 20);
            this.label1.Text = "Y";
            // 
            // label2
            // 
            this.label2.Font = Fonts.Sans(12);
            this.label2.Size = new Size(20, 20);
            this.label2.Text = "X";
            // 
            // XPixelLabel
            // 
            this.XPixelLabel.Font = Fonts.Monospace(12);
            this.XPixelLabel.Size = new Size(100, 19);
            this.XPixelLabel.Text = "?";
            // 
            // label4
            // 
            this.label4.Font = Fonts.Sans(12);
            this.label4.Size = new Size(41, 20);
            this.label4.Text = "Pixel";
            // 
            // label5
            // 
            this.label5.Font = Fonts.Sans(12);
            this.label5.Size = new Size(87, 20);
            this.label5.Text = "Coordinate";
            // 
            // YPixelLabel
            // 
            this.YPixelLabel.Font = Fonts.Monospace(12);
            this.YPixelLabel.Size = new Size(100, 19);
            this.YPixelLabel.Text = "?";
            // 
            // XCoordinateLabel
            // 
            this.XCoordinateLabel.Font = Fonts.Monospace(12);
            this.XCoordinateLabel.Size = new Size(120, 19);
            this.XCoordinateLabel.Text = "?";
            // 
            // YCoordinateLabel
            // 
            this.YCoordinateLabel.Font = Fonts.Monospace(12);
            this.YCoordinateLabel.Size = new Size(120, 19);
            this.YCoordinateLabel.Text = "?";
            // 
            // formsPlot1
            // 
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            // 
            // lblMouse
            // 
            this.lblMouse.Font = Fonts.Sans(12);
            this.lblMouse.Size = new Size(149, 20);
            this.lblMouse.Text = "Waiting for mouse...";
            // 
            // MouseTracker
            // 
            this.ClientSize = new Size(736, 405);
            this.Title = "MouseTracker";
            this.ResumeLayout();
        }

        private Label label1;
        private Label label2;
        private Label XPixelLabel;
        private Label label4;
        private Label label5;
        private Label YPixelLabel;
        private Label XCoordinateLabel;
        private Label YCoordinateLabel;
        private ScottPlot.Eto.PlotView formsPlot1;
        private Label lblMouse;
    }
}
