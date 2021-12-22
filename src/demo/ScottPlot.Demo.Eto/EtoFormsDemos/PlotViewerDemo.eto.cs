using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class PlotViewerDemo : Form
    {
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.groupBox1 = new GroupBox();
            this.label3 = new Label();
            this.nudWalkPoints = new NumericStepper();
            this.btnLaunchRandomWalk = new Button();
            this.groupBox2 = new GroupBox();
            this.label4 = new Label();
            this.nudSineCount = new NumericStepper();
            this.btnLaunchRandomSine = new Button();
            this.groupBox3 = new GroupBox();
            this.textBox1 = new TextArea();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();

            var layout1 = new StackLayout(label3, nudWalkPoints) { Padding = 5 };
            this.groupBox1.Content = new StackLayout(layout1, btnLaunchRandomWalk) { Padding = 5, Orientation = global::Eto.Forms.Orientation.Horizontal };

            var layout2 = new StackLayout(label4, nudSineCount) { Padding = 5 };
            this.groupBox2.Content = new StackLayout(layout2, btnLaunchRandomSine) { Padding = 5, Orientation = global::Eto.Forms.Orientation.Horizontal };

            this.groupBox3.Content = textBox1;

            var layout = new DynamicLayout() { Spacing = Size.Empty + 4, Padding = 8 };
            layout.BeginVertical();
            layout.AddRow(label1);
            layout.AddRow(label2);
            layout.EndVertical();
            layout.BeginVertical();
            layout.AddRow(groupBox1, null, groupBox2);
            layout.EndVertical();
            layout.BeginVertical();
            layout.AddRow(groupBox3);
            layout.EndVertical();

            this.Content = layout;

            // 
            // label1
            // 
            this.label1.Text = "The PlotViewer lets you launch a plot in an interactive pop-up window.";
            // 
            // label2
            // 
            this.label2.Text = "You can focus on generating interesting data, and the PlotViewer will handle the GUI!";
            // 
            // groupBox1
            // 
            this.groupBox1.Text = "Random Walk Generator";
            // 
            // label3
            // 
            this.label3.Text = "Points:";
            // 
            // nudWalkPoints
            // 
            this.nudWalkPoints.MaxValue = 1000;
            this.nudWalkPoints.MinValue = 1;
            this.nudWalkPoints.Value = 100;
            // 
            // btnLaunchRandomWalk
            // 
            this.btnLaunchRandomWalk.Size = new Size(69, 36);
            this.btnLaunchRandomWalk.Text = "Launch";
            // 
            // groupBox2
            // 
            this.groupBox2.Text = "Random Sine Generator";
            // 
            // label4
            // 
            this.label4.Text = "Waves:";
            // 
            // nudSineCount
            // 
            this.nudSineCount.MaxValue = 1000;
            this.nudSineCount.MinValue = 1;
            this.nudSineCount.Value = 3;
            // 
            // btnLaunchRandomSine
            // 
            this.btnLaunchRandomSine.Size = new Size(69, 36);
            this.btnLaunchRandomSine.Text = "Launch";
            // 
            // groupBox3
            // 
            this.groupBox3.Size = new Size(406, 134);
            this.groupBox3.Text = "Sample Code";
            // 
            // textBox1
            // 
            this.textBox1.BackgroundColor = SystemColors.Control;
            this.textBox1.Font = Fonts.Monospace(8);
            this.textBox1.Size = new Size(400, 115);
            this.textBox1.Text = "// create a ScottPlot\r\nvar plt = new ScottPlot.Plot();\r\nplt.PlotSignal(dataArray)" +
    ";\r\n\r\n// launch it in a PlotViewer\r\nnew ScottPlot.FormsPlotViewer(plt).Show();";
            // 
            // PlotViewerDemo
            // 
            this.ClientSize = new Size(439, 299);
            this.Title = "Plot Viewer Demo";
            this.groupBox1.ResumeLayout();
            this.groupBox2.ResumeLayout();
            this.groupBox3.ResumeLayout();
            this.ResumeLayout();
        }

        private Label label1;
        private Label label2;
        private GroupBox groupBox1;
        private Label label3;
        private NumericStepper nudWalkPoints;
        private Button btnLaunchRandomWalk;
        private GroupBox groupBox2;
        private Label label4;
        private NumericStepper nudSineCount;
        private Button btnLaunchRandomSine;
        private GroupBox groupBox3;
        private TextArea textBox1;
    }
}
