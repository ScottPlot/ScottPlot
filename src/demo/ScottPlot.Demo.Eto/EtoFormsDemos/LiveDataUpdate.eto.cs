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
            //this.label1.AutoSize = true;
            //this.label1.Location = new System.Drawing.Point(12, 9);
            //this.label1.Name = "label1";
            //this.label1.Size = new Size(328, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This example uses a fixed-size array and updates its values with time";
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.formsPlot1.Location = new System.Drawing.Point(12, 31);
            //this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new Size(776, 407);
            this.formsPlot1.TabIndex = 1;
            // 
            // timerUpdateData
            // 
            //this.timerUpdateData.Enabled = true;
            // 
            // timerRender
            // 
            //this.timerRender.Enabled = true;
            // 
            // runCheckbox
            // 
            //this.runCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //this.runCheckbox.AutoSize = true;
            this.runCheckbox.Checked = true;
            //this.runCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            //this.runCheckbox.Location = new System.Drawing.Point(742, 8);
            //this.runCheckbox.Name = "runCheckbox";
            //this.runCheckbox.Size = new Size(46, 17);
            this.runCheckbox.TabIndex = 2;
            this.runCheckbox.Text = "Run";
            //this.runCheckbox.UseVisualStyleBackColor = true;
            // 
            // rollCheckbox
            // 
            //this.rollCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //this.rollCheckbox.AutoSize = true;
            //this.rollCheckbox.Location = new System.Drawing.Point(692, 8);
            //this.rollCheckbox.Name = "rollCheckbox";
            //this.rollCheckbox.Size = new Size(44, 17);
            this.rollCheckbox.TabIndex = 3;
            this.rollCheckbox.Text = "Roll";
            //this.rollCheckbox.UseVisualStyleBackColor = true;
            // 
            // LiveDataUpdate
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            //this.Name = "LiveDataUpdate";
            //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
