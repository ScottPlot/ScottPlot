using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class ColormapViewer : Form
    {
        private void InitializeComponent()
        {
            //this.panel1 = new Panel();
            this.lbColormapNames = new ListBox();
            this.lblColormap = new Label();
            this.pbColormap = new ImageView();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.formsPlot2 = new ScottPlot.Eto.PlotView();
            this.rbData = new RadioButton();
            this.rbImage = new RadioButton(rbData);
            this.formsPlot3 = new ScottPlot.Eto.PlotView();
            //this.panel1.SuspendLayout();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(lblColormap, null, rbData, rbImage);
            layout.AddSeparateRow(pbColormap, null);
            layout.AddSeparateRow(formsPlot1, formsPlot2);
            layout.AddSeparateRow(formsPlot3);

            this.Content = new StackLayout(lbColormapNames, layout) { Orientation = global::Eto.Forms.Orientation.Horizontal, Padding = 5 };
            // 
            // panel1
            // 
            //this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left)));
            //this.panel1.Controls.Add(this.lbColormapNames);
            //this.panel1.Location = new System.Drawing.Point(12, 12);
            //this.panel1.Name = "panel1";
            //this.panel1.Size = new Size(148, 633);
            //this.panel1.TabIndex = 0;
            // 
            // lbColormapNames
            // 
            //this.lbColormapNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbColormapNames.Font = Fonts.Sans(12);// new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lbColormapNames.FormattingEnabled = true;
            //this.lbColormapNames.ItemHeight = 21;
            //this.lbColormapNames.Location = new System.Drawing.Point(0, 0);
            //this.lbColormapNames.Name = "lbColormapNames";
            this.lbColormapNames.Size = new Size(148, 633);
            this.lbColormapNames.TabIndex = 0;
            // 
            // lblColormap
            // 
            //this.lblColormap.AutoSize = true;
            this.lblColormap.Font = Fonts.Sans(16);//new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblColormap.Location = new System.Drawing.Point(166, 12);
            //this.lblColormap.Name = "lblColormap";
            this.lblColormap.Size = new Size(68, 30);
            this.lblColormap.TabIndex = 1;
            this.lblColormap.Text = "label1";
            // 
            // pbColormap
            // 
            //this.pbColormap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.pbColormap.BackgroundColor = SystemColors.Control;
            //this.pbColormap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //this.pbColormap.Location = new System.Drawing.Point(171, 45);
            //this.pbColormap.Name = "pbColormap";
            this.pbColormap.Size = new Size(432, 37);
            this.pbColormap.TabIndex = 2;
            //this.pbColormap.TabStop = false;
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left)));
            //this.formsPlot1.Location = new System.Drawing.Point(171, 88);
            //this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new Size(432, 336);
            this.formsPlot1.TabIndex = 3;
            // 
            // formsPlot2
            // 
            //this.formsPlot2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.formsPlot2.Location = new System.Drawing.Point(609, 88);
            //this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new Size(671, 336);
            this.formsPlot2.TabIndex = 4;
            // 
            // rbData
            // 
            //this.rbData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //this.rbData.AutoSize = true;
            this.rbData.Font = Fonts.Sans(12);// new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.rbData.Location = new System.Drawing.Point(1011, 14);
            //this.rbData.Name = "rbData";
            this.rbData.Size = new Size(116, 25);
            this.rbData.TabIndex = 5;
            this.rbData.Text = "Sample Data";
            //this.rbData.UseVisualStyleBackColor = true;
            // 
            // rbImage
            // 
            //this.rbImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //this.rbImage.AutoSize = true;
            this.rbImage.Checked = true;
            this.rbImage.Font = Fonts.Sans(12);//new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.rbImage.Location = new System.Drawing.Point(1153, 14);
            //this.rbImage.Name = "rbImage";
            this.rbImage.Size = new Size(127, 25);
            this.rbImage.TabIndex = 6;
            //this.rbImage.TabStop = true;
            this.rbImage.Text = "Sample Image";
            //this.rbImage.UseVisualStyleBackColor = true;
            // 
            // formsPlot3
            // 
            //this.formsPlot3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.formsPlot3.Location = new System.Drawing.Point(171, 430);
            //this.formsPlot3.Name = "formsPlot3";
            this.formsPlot3.Size = new Size(1109, 215);
            this.formsPlot3.TabIndex = 7;
            // 
            // ColormapViewer
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(1292, 657);
            //this.Name = "ColormapViewer";
            //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Title = "Colormap Viewer";
            //this.panel1.ResumeLayout(false);
            this.ResumeLayout();
        }

        //private Panel panel1;
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
