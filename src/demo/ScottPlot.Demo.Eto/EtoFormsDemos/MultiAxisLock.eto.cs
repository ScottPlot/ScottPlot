using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class MultiAxisLock : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.cbPrimary = new CheckBox();
            this.cbSecondary = new CheckBox();
            this.cbTertiary = new CheckBox();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(cbPrimary, cbSecondary, cbTertiary);
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            //this.formsPlot1.Location = new System.Drawing.Point(12, 43);
            //this.formsPlot1.Name = "formsPlot1";
            //this.formsPlot1.Size = new Size(594, 326);
            this.formsPlot1.TabIndex = 0;
            // 
            // cbPrimary
            // 
            //this.cbPrimary.AutoSize = true;
            this.cbPrimary.Checked = true;
            //this.cbPrimary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPrimary.Font = Fonts.Sans(12, FontStyle.Bold);// new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPrimary.TextColor = Colors.Magenta;
            //this.cbPrimary.Location = new System.Drawing.Point(12, 12);
            //this.cbPrimary.Name = "cbPrimary";
            //this.cbPrimary.Size = new Size(85, 25);
            //this.cbPrimary.TabIndex = 7;
            this.cbPrimary.Text = "Primary";
            //this.cbPrimary.UseVisualStyleBackColor = true;
            // 
            // cbSecondary
            // 
            //this.cbSecondary.AutoSize = true;
            this.cbSecondary.Font = Fonts.Sans(12, FontStyle.Bold);//new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSecondary.TextColor = Colors.Green;
            //this.cbSecondary.Location = new System.Drawing.Point(103, 12);
            //this.cbSecondary.Name = "cbSecondary";
            //this.cbSecondary.Size = new Size(107, 25);
            //this.cbSecondary.TabIndex = 8;
            this.cbSecondary.Text = "Secondary";
            //this.cbSecondary.UseVisualStyleBackColor = true;
            // 
            // cbTertiary
            // 
            //this.cbTertiary.AutoSize = true;
            this.cbTertiary.Font = Fonts.Sans(12, FontStyle.Bold);//new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTertiary.TextColor = Colors.Navy;
            //this.cbTertiary.Location = new System.Drawing.Point(216, 12);
            //this.cbTertiary.Name = "cbTertiary";
            //this.cbTertiary.Size = new Size(84, 25);
            //this.cbTertiary.TabIndex = 9;
            this.cbTertiary.Text = "Tertiary";
            //this.cbTertiary.UseVisualStyleBackColor = true;
            // 
            // MultiAxis
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(618, 381);
            //this.Name = "MultiAxis";
            this.Title = "MultiAxis";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private CheckBox cbPrimary;
        private CheckBox cbSecondary;
        private CheckBox cbTertiary;
    }
}
