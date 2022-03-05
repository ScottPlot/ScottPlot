using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class LiveDataUpdate : Form
    {
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.timerUpdateData = new UITimer();
            this.timerRender = new UITimer();
            this.runCheckbox = new CheckBox();
            this.rollCheckbox = new CheckBox();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(label1, null, rollCheckbox, runCheckbox);
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;

            // 
            // label1
            // 
            this.label1.Text = "This example uses a fixed-size array and updates its values with time";
            // 
            // formsPlot1
            // 
            this.formsPlot1.Size = new Size(776, 407);
            // 
            // runCheckbox
            // 
            this.runCheckbox.Checked = true;
            this.runCheckbox.Text = "Run";
            // 
            // rollCheckbox
            // 
            this.rollCheckbox.Text = "Roll";
            // 
            // LiveDataUpdate
            // 
            this.ClientSize = new Size(800, 450);
            this.Title = "Live Data (fixed array)";
            this.ResumeLayout();
        }

        private Label label1;
        private ScottPlot.Eto.PlotView formsPlot1;
        private UITimer timerUpdateData;
        private UITimer timerRender;
        private CheckBox runCheckbox;
        private CheckBox rollCheckbox;
    }
}
