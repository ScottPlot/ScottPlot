using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Eto
{
    partial class FormHelp : Form
    {
        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblVersion = new Label();
            this.lblMessage = new Label();
            this.SuspendLayout();

            this.Content = new DynamicLayout(lblTitle, lblVersion, lblMessage) { Padding = 10 };

            // 
            // lblTitle
            // 
            //this.lblTitle.AutoSize = true;
            this.lblTitle.Font = Fonts.Sans(18);// new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblTitle.Location = new System.Drawing.Point(12, 9);
            //this.lblTitle.Name = "lblTitle";
            //this.lblTitle.Size = new System.Drawing.Size(339, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Mouse and Keyboard Controls";
            // 
            // lblVersion
            // 
            //this.lblVersion.AutoSize = true;
            this.lblVersion.Font = Fonts.Sans(10);//new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.TextColor = SystemColors.DisabledText;
            //this.lblVersion.Location = new System.Drawing.Point(14, 41);
            //this.lblVersion.Name = "lblVersion";
            //this.lblVersion.Size = new System.Drawing.Size(188, 17);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "ScottPlot.WinForms 1.1.1-alpha";
            // 
            // lblMessage
            // 
            //this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Font = Fonts.Sans(12);//new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblMessage.Location = new System.Drawing.Point(12, 72);
            //this.lblMessage.Name = "lblMessage";
            //this.lblMessage.Size = new System.Drawing.Size(391, 337);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "help message...";
            // 
            // FormHelp
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(415, 418);
            //this.Name = "FormHelp";
            this.Title = "Help";
            this.ResumeLayout();
        }

        private Label lblTitle;
        private Label lblVersion;
        private Label lblMessage;
    }
}
