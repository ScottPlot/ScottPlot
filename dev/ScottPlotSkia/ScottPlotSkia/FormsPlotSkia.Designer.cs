using OpenTK.Graphics;

namespace ScottPlotSkia
{
    partial class FormsPlotSkia
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.glControl1 = new OpenTK.GLControl(new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8, 4));
            ((System.ComponentModel.ISupportInitialize)(this.lastInteractionTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).BeginInit();
            this.SuspendLayout();
            // 
            // lastInteractionTimer
            // 
            this.lastInteractionTimer.Enabled = false;
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.glControl1.Location = new System.Drawing.Point(77, 35);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(443, 310);
            this.glControl1.TabIndex = 1;
            this.glControl1.VSync = false;
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.GlControl1_Paint);
            // 
            // FormsPlotSkia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.glControl1);
            this.Name = "FormsPlotSkia";
            this.Controls.SetChildIndex(this.pbPlot, 0);
            this.Controls.SetChildIndex(this.glControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.lastInteractionTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion 
        OpenTK.GLControl glControl1;
    }
}
