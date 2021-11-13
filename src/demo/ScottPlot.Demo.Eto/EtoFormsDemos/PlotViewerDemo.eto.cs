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
            //this.label1.AutoSize = true;
            //this.label1.Location = new System.Drawing.Point(12, 9);
            //this.label1.Name = "label1";
            //this.label1.Size = new Size(338, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "The PlotViewer lets you launch a plot in an interactive pop-up window.";
            // 
            // label2
            // 
            //this.label2.AutoSize = true;
            //this.label2.Location = new System.Drawing.Point(12, 32);
            //this.label2.Name = "label2";
            //this.label2.Size = new Size(409, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "You can focus on generating interesting data, and the PlotViewer will handle the " +
    "GUI!";
            // 
            // groupBox1
            // 
            //this.groupBox1.Location = new System.Drawing.Point(15, 65);
            //this.groupBox1.Name = "groupBox1";
            //this.groupBox1.Size = new Size(156, 65);
            //this.groupBox1.TabIndex = 2;
            //this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Random Walk Generator";
            // 
            // label3
            // 
            //this.label3.AutoSize = true;
            //this.label3.Location = new System.Drawing.Point(6, 19);
            //this.label3.Name = "label3";
            //this.label3.Size = new Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Points:";
            // 
            // nudWalkPoints
            // 
            //this.nudWalkPoints.Location = new System.Drawing.Point(9, 35);
            this.nudWalkPoints.MaxValue = 1000; // .Maximum = new decimal(new int[] {1000,0,0,0});
            this.nudWalkPoints.MinValue = 1; // .Minimum = new decimal(new int[] {1,0,0,0});
            //this.nudWalkPoints.Name = "nudWalkPoints";
            //this.nudWalkPoints.Size = new Size(63, 20);
            this.nudWalkPoints.TabIndex = 1;
            this.nudWalkPoints.Value = 100;// new decimal(new int[] {100,0,0,0});
            // 
            // btnLaunchRandomWalk
            // 
            //this.btnLaunchRandomWalk.Location = new System.Drawing.Point(78, 19);
            //this.btnLaunchRandomWalk.Name = "btnLaunchRandomWalk";
            this.btnLaunchRandomWalk.Size = new Size(69, 36);
            this.btnLaunchRandomWalk.TabIndex = 0;
            this.btnLaunchRandomWalk.Text = "Launch";
            //this.btnLaunchRandomWalk.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            //this.groupBox2.Location = new System.Drawing.Point(194, 65);
            //this.groupBox2.Name = "groupBox2";
            //this.groupBox2.Size = new Size(156, 65);
            //this.groupBox2.TabIndex = 3;
            //this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Random Sine Generator";
            // 
            // label4
            // 
            //this.label4.AutoSize = true;
            //this.label4.Location = new System.Drawing.Point(6, 19);
            //this.label4.Name = "label4";
            //this.label4.Size = new Size(44, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Waves:";
            // 
            // nudSineCount
            // 
            //this.nudSineCount.Location = new System.Drawing.Point(9, 35);
            this.nudSineCount.MaxValue = 1000; //.Maximum = new decimal(new int[] {1000,0,0,0});
            this.nudSineCount.MinValue = 1; //.Minimum = new decimal(new int[] {1,0,0,0});
            //this.nudSineCount.Name = "nudSineCount";
            //this.nudSineCount.Size = new Size(63, 20);
            this.nudSineCount.TabIndex = 1;
            this.nudSineCount.Value = 3;// new decimal(new int[] {3,0,0,0});
            // 
            // btnLaunchRandomSine
            // 
            //this.btnLaunchRandomSine.Location = new System.Drawing.Point(78, 19);
            //this.btnLaunchRandomSine.Name = "btnLaunchRandomSine";
            this.btnLaunchRandomSine.Size = new Size(69, 36);
            this.btnLaunchRandomSine.TabIndex = 0;
            this.btnLaunchRandomSine.Text = "Launch";
            //this.btnLaunchRandomSine.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            //this.groupBox3.Location = new System.Drawing.Point(15, 149);
            //this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(406, 134);
            //this.groupBox3.TabIndex = 4;
            //this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sample Code";
            // 
            // textBox1
            // 
            this.textBox1.BackgroundColor = SystemColors.Control;
            //this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = Fonts.Monospace(8);// new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.textBox1.Location = new System.Drawing.Point(3, 16);
            //this.textBox1.Multiline = true;
            //this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(400, 115);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "// create a ScottPlot\r\nvar plt = new ScottPlot.Plot();\r\nplt.PlotSignal(dataArray)" +
    ";\r\n\r\n// launch it in a PlotViewer\r\nnew ScottPlot.FormsPlotViewer(plt).Show();";
            // 
            // PlotViewerDemo
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(439, 299);
            //this.Name = "PlotViewerDemo";
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
