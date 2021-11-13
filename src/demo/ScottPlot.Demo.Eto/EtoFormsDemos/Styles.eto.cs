using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class Styles : Form
    {
        private void InitializeComponent()
        {
            this.lbStyles = new ListBox();
            this.groupBox1 = new GroupBox();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.groupBox2 = new GroupBox();
            this.lbPalettes = new ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();

            groupBox1.Content = lbStyles;
            groupBox2.Content = lbPalettes;

            var layout = new DynamicLayout() { DefaultSpacing = Size.Empty + 5, Padding = 5 };
            layout.AddSeparateRow(groupBox1, groupBox2, formsPlot1);

            this.Content = layout;
            // 
            // lbStyles
            // 
            //this.lbStyles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbStyles.Font = Fonts.Sans(12);// new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lbStyles.FormattingEnabled = true;
            //this.lbStyles.ItemHeight = 21;
            //this.lbStyles.Location = new System.Drawing.Point(3, 25);
            //this.lbStyles.Name = "lbStyles";
            //this.lbStyles.Size = new Size(209, 395);
            this.lbStyles.TabIndex = 0;
            // 
            // groupBox1
            // 
            //this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left)));
            //this.groupBox1.Controls.Add(this.lbStyles);
            this.groupBox1.Font = Fonts.Sans(12);// new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.groupBox1.Location = new System.Drawing.Point(12, 12);
            //this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(215, 423);
            this.groupBox1.TabIndex = 1;
            //this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Styles";
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            //this.formsPlot1.Location = new System.Drawing.Point(454, 12);
            //this.formsPlot1.Name = "formsPlot1";
            //this.formsPlot1.Size = new Size(607, 423);
            this.formsPlot1.TabIndex = 2;
            // 
            // groupBox2
            // 
            //this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left)));
            //this.groupBox2.Controls.Add(this.lbPalettes);
            this.groupBox2.Font = Fonts.Sans(12);//new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.groupBox2.Location = new System.Drawing.Point(233, 12);
            //this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(215, 423);
            this.groupBox2.TabIndex = 3;
            //this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Palettes";
            // 
            // lbPalettes
            // 
            //this.lbPalettes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPalettes.Font = Fonts.Sans(12);//new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lbPalettes.FormattingEnabled = true;
            //this.lbPalettes.ItemHeight = 21;
            //this.lbPalettes.Location = new System.Drawing.Point(3, 25);
            //this.lbPalettes.Name = "lbPalettes";
            //this.lbPalettes.Size = new Size(209, 395);
            this.lbPalettes.TabIndex = 0;
            // 
            // Styles
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(1073, 447);
            //this.Name = "Styles";
            this.Title = "ScottPlot Styles";
            this.groupBox1.ResumeLayout();
            this.groupBox2.ResumeLayout();
            this.ResumeLayout();
        }

        private ListBox lbStyles;
        private GroupBox groupBox1;
        private ScottPlot.Eto.PlotView formsPlot1;
        private GroupBox groupBox2;
        private ListBox lbPalettes;
    }
}
