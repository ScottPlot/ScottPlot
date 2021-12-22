using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class TransparentBackground : Form
    {
        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.button4 = new Button();
            this.button5 = new Button();
            this.button6 = new Button();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(spacing: Size.Empty + 5, controls: new[] { button1, button2, button3, button4, button5, button6, null });
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;

            // 
            // button1
            // 
            this.button1.Size = new Size(75, 23);
            this.button1.Text = "red";
            // 
            // button2
            // 
            this.button2.Size = new Size(75, 23);
            this.button2.Text = "green";
            // 
            // button3
            // 
            this.button3.Size = new Size(75, 23);
            this.button3.Text = "blue";
            // 
            // button4
            // 
            this.button4.Size = new Size(75, 23);
            this.button4.Text = "white";
            // 
            // button5
            // 
            this.button5.Size = new Size(75, 23);
            this.button5.Text = "control";
            // 
            // button6
            // 
            this.button6.Size = new Size(75, 23);
            this.button6.Text = "image";
            // 
            // TransparentBackground
            // 
            this.ClientSize = new Size(623, 328);
            this.Title = "Transparent Background";
            this.ResumeLayout();
        }

        private Button button1;
        private Button button2;
        private Button button3;
        private ScottPlot.Eto.PlotView formsPlot1;
        private Button button4;
        private Button button5;
        private Button button6;
    }
}
