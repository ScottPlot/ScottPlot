using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Eto
{
    partial class FormHelp : Form
    {
        private void InitializeComponent()
        {
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

        private Label lblTitle = new Label();
        private Label lblVersion = new Label();
        private Label lblMessage = new Label();
    }
}
