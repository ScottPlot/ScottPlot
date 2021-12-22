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
            this.lblTitle.Font = Fonts.Sans(18);
            this.lblTitle.Text = "Mouse and Keyboard Controls";
            // 
            // lblVersion
            // 
            this.lblVersion.Font = Fonts.Sans(10);
            this.lblVersion.TextColor = SystemColors.DisabledText;
            this.lblVersion.Text = "ScottPlot.WinForms 1.1.1-alpha";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = Fonts.Sans(12);
            this.lblMessage.Text = "help message...";
            // 
            // FormHelp
            // 
            this.ClientSize = new Size(415, 418);
            this.Title = "Help";
            this.ResumeLayout();
        }

        private Label lblTitle;
        private Label lblVersion;
        private Label lblMessage;
    }
}
