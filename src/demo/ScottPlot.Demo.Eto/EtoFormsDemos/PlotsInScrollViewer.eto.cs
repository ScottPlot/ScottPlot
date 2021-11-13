using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class PlotsInScrollViewer : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.formsPlot2 = new ScottPlot.Eto.PlotView();
            this.formsPlot3 = new ScottPlot.Eto.PlotView();
            this.panel1 = new Scrollable();
            this.panel2 = new DynamicLayout();
            //this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbScroll = new RadioButton();
            this.rbZoom = new RadioButton(rbScroll);
            this.groupBox1 = new GroupBox();
            //this.panel1.SuspendLayout();
            //this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();

            this.groupBox1.Content = new StackLayout(rbScroll, rbZoom) { Padding = 5, Orientation = global::Eto.Forms.Orientation.Horizontal };

            // scrollable type 1
            panel1.Content = new DynamicLayout(formsPlot1, formsPlot2, formsPlot3) { Padding = 2 };
            panel1.Size = new Size(480, 430);
            // scrollable type 2 (works on wpf, not winforms)
            panel2 = new DynamicLayout() { Size = new Size(480, 430) };
            panel2.BeginScrollable();
            panel2.AddRow(formsPlot1);
            panel2.AddRow(formsPlot2);
            panel2.AddRow(formsPlot3);
            panel2.EndScrollable();

            this.Content = new StackLayout(groupBox1, panel1) { Padding = 5 };

            // 
            // formsPlot1
            // 
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            //this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.formsPlot1.Location = new System.Drawing.Point(3, 3);
            //this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new Size(452, 327);
            this.formsPlot1.TabIndex = 0;
            // 
            // formsPlot2
            // 
            this.formsPlot2.BackgroundColor = Colors.Transparent;
            //this.formsPlot2.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.formsPlot2.Location = new System.Drawing.Point(3, 336);
            //this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new Size(452, 327);
            this.formsPlot2.TabIndex = 1;
            // 
            // formsPlot3
            // 
            this.formsPlot3.BackgroundColor = Colors.Transparent;
            //this.formsPlot3.Dock = System.Windows.Forms.DockStyle.Fill;
            ////this.formsPlot3.Location = new System.Drawing.Point(3, 669);
            //this.formsPlot3.Name = "formsPlot3";
            this.formsPlot3.Size = new Size(452, 328);
            this.formsPlot3.TabIndex = 2;
            // 
            // panel1
            // 
            //this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.panel1.AutoScroll = true;
            //this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            //this.panel1.Controls.Add(this.tableLayoutPanel1);
            //this.panel1.Location = new System.Drawing.Point(12, 62);
            //this.panel1.Name = "panel1";
            //this.panel1.Size = new Size(485, 433);
            //this.panel1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            //this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.tableLayoutPanel1.ColumnCount = 1;
            //this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //this.tableLayoutPanel1.Controls.Add(this.formsPlot3, 0, 2);
            //this.tableLayoutPanel1.Controls.Add(this.formsPlot1, 0, 0);
            //this.tableLayoutPanel1.Controls.Add(this.formsPlot2, 0, 1);
            //this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            //this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            //this.tableLayoutPanel1.RowCount = 3;
            //this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            //this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            //this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            //this.tableLayoutPanel1.Size = new Size(458, 1000);
            //this.tableLayoutPanel1.TabIndex = 4;
            // 
            // rbScroll
            // 
            //this.rbScroll.AutoSize = true;
            //this.rbScroll.Location = new System.Drawing.Point(6, 19);
            //this.rbScroll.Name = "rbScroll";
            this.rbScroll.Size = new Size(97, 17);
            this.rbScroll.TabIndex = 4;
            this.rbScroll.Text = "Scroll up/down";
            //this.rbScroll.UseVisualStyleBackColor = true;
            // 
            // rbZoom
            // 
            //this.rbZoom.AutoSize = true;
            this.rbZoom.Checked = true;
            //this.rbZoom.Location = new System.Drawing.Point(109, 19);
            //this.rbZoom.Name = "rbZoom";
            this.rbZoom.Size = new Size(83, 17);
            this.rbZoom.TabIndex = 5;
            //this.rbZoom.TabStop = true;
            this.rbZoom.Text = "Zoom in/out";
            //this.rbZoom.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            //this.groupBox1.Controls.Add(this.rbScroll);
            //this.groupBox1.Controls.Add(this.rbZoom);
            //this.groupBox1.Location = new System.Drawing.Point(12, 12);
            //this.groupBox1.Name = "groupBox1";
            //this.groupBox1.Size = new Size(197, 44);
            this.groupBox1.TabIndex = 6;
            //this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mouse Wheel Action";
            // 
            // PlotsInScrollViewer
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.AutoScroll = true;
            //this.ClientSize = new Size(509, 507);
            //this.Controls.Add(this.groupBox1);
            //this.Controls.Add(this.panel1);
            //this.Name = "PlotsInScrollViewer";
            this.Title = "Plots in a Scroll Viewer";
            //this.panel1.ResumeLayout(false);
            //this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout();
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private ScottPlot.Eto.PlotView formsPlot2;
        private ScottPlot.Eto.PlotView formsPlot3;
        private Scrollable panel1;
        private DynamicLayout panel2;
        //private Panel panel1;
        //private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RadioButton rbScroll;
        private RadioButton rbZoom;
        private GroupBox groupBox1;
    }
}
