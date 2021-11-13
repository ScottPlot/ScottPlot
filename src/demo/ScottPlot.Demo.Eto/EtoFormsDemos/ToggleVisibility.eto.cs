using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class ToggleVisibility : Form
    {
        private void InitializeComponent()
        {
            this.cbSin = new CheckBox();
            this.cbCos = new CheckBox();
            this.cbLines = new CheckBox();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(cbSin, cbCos, cbLines, null);
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;

            // 
            // cbSin
            // 
            //this.cbSin.AutoSize = true;
            this.cbSin.Checked = true;
            //this.cbSin.CheckState = System.Windows.Forms.CheckState.Checked;
            //this.cbSin.Location = new System.Drawing.Point(12, 12);
            //this.cbSin.Name = "cbSin";
            //this.cbSin.Size = new Size(41, 17);
            this.cbSin.TabIndex = 0;
            this.cbSin.Text = "Sin";
            //this.cbSin.UseVisualStyleBackColor = true;
            // 
            // cbCos
            // 
            //this.cbCos.AutoSize = true;
            this.cbCos.Checked = true;
            //this.cbCos.CheckState = System.Windows.Forms.CheckState.Checked;
            //this.cbCos.Location = new System.Drawing.Point(59, 12);
            //this.cbCos.Name = "cbCos";
            //this.cbCos.Size = new Size(44, 17);
            this.cbCos.TabIndex = 1;
            this.cbCos.Text = "Cos";
            //this.cbCos.UseVisualStyleBackColor = true;
            // 
            // cbLines
            // 
            //this.cbLines.AutoSize = true;
            this.cbLines.Checked = true;
            //this.cbLines.CheckState = System.Windows.Forms.CheckState.Checked;
            //this.cbLines.Location = new System.Drawing.Point(109, 12);
            //this.cbLines.Name = "cbLines";
            //this.cbLines.Size = new Size(47, 17);
            this.cbLines.TabIndex = 2;
            this.cbLines.Text = "lines";
            //this.cbLines.UseVisualStyleBackColor = true;
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.formsPlot1.Location = new System.Drawing.Point(12, 35);
            //this.formsPlot1.Name = "formsPlot1";
            //this.formsPlot1.Size = new Size(449, 255);
            this.formsPlot1.TabIndex = 3;
            // 
            // ToggleVisibility
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(473, 302);
            //this.Name = "ToggleVisibility";
            //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Title = "ToggleVisibility";
            this.ResumeLayout();
        }

        private CheckBox cbSin;
        private CheckBox cbCos;
        private CheckBox cbLines;
        private ScottPlot.Eto.PlotView formsPlot1;
    }
}
