using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class ShowValueOnHover2 : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.label1 = new Label();
            this.SuspendLayout();

            this.Content = new DynamicLayout(label1, formsPlot1) { Padding = 5 };

            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            //this.formsPlot1.Location = new System.Drawing.Point(14, 38);
            //this.formsPlot1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            //this.formsPlot1.Name = "formsPlot1";
            //this.formsPlot1.Size = new Size(679, 352);
            this.formsPlot1.TabIndex = 0;
            // 
            // label1
            // 
            //this.label1.AutoSize = true;
            this.label1.Font = Fonts.Sans(12); // new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.label1.Location = new System.Drawing.Point(14, 10);
            //this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.label1.Name = "label1";
            //this.label1.Size = new Size(71, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Message";
            // 
            // ShowValueOnHover2
            // 
            //this.AutoScaleDimensions = new SizeF(7F, 15F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(707, 404);
            //this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.Name = "ShowValueOnHover2";
            this.Title = "Show Value Nearest Cursor";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private Label label1;
    }
}
