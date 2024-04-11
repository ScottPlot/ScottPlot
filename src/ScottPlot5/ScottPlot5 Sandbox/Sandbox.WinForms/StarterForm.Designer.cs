namespace Sandbox.WinForms
{
    partial class StarterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonLaunch = new Button();
            SuspendLayout();
            // 
            // buttonLaunch
            // 
            buttonLaunch.Location = new Point(263, 119);
            buttonLaunch.Name = "buttonLaunch";
            buttonLaunch.Size = new Size(75, 23);
            buttonLaunch.TabIndex = 0;
            buttonLaunch.Text = "Launch";
            buttonLaunch.UseVisualStyleBackColor = true;
            buttonLaunch.Click += buttonLaunch_Click;
            // 
            // StarterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonLaunch);
            Name = "StarterForm";
            Text = "StarterForm";
            ResumeLayout(false);
        }

        #endregion

        private Button buttonLaunch;
    }
}