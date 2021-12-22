using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class LiveDataIncoming : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.label1 = new Label();
            this.label2 = new Label();
            this.tbLatestValue = new TextBox();
            this.label3 = new Label();
            this.tbLastValue = new TextBox();
            this.dataTimer = new UITimer();
            this.renderTimer = new UITimer();
            this.cbAutoAxis = new CheckBox();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(label1);
            var layout2 = new StackLayout(label2, tbLatestValue, label3, tbLastValue, cbAutoAxis)
            {
                Spacing = 10,
                Orientation = global::Eto.Forms.Orientation.Horizontal
            };
            layout.AddSeparateRow(layout2);
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;

            // 
            // formsPlot1
            // 
            this.formsPlot1.Size = new Size(-1, 380);
            // 
            // label1
            // 
            this.label1.Text = "This example simulates live display of a growing dataset";
            // 
            // label2
            // 
            this.label2.Text = "readings: ";
            // 
            // tbLatestValue
            // 
            this.tbLatestValue.Font = Fonts.Monospace(10);
            this.tbLatestValue.ReadOnly = true;
            this.tbLatestValue.Text = "123";
            // 
            // label3
            // 
            this.label3.Text = "latest value:";
            // 
            // tbLastValue
            // 
            this.tbLastValue.Font = Fonts.Monospace(10);
            this.tbLastValue.ReadOnly = true;
            this.tbLastValue.Text = "+123.4";
            // 
            // cbAutoAxis
            // 
            this.cbAutoAxis.Checked = true;
            this.cbAutoAxis.Text = "auto-axis on each update";
            // 
            // LiveDataIncoming
            // 
            this.ClientSize = new Size(800, -1);
            this.Title = "Live Data (growing)";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private Label label1;
        private Label label2;
        private TextBox tbLatestValue;
        private Label label3;
        private TextBox tbLastValue;
        private UITimer dataTimer;
        private UITimer renderTimer;
        private CheckBox cbAutoAxis;
    }
}
