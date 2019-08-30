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
            ((System.ComponentModel.ISupportInitialize)(this.lastInteractionTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).BeginInit();
            this.SuspendLayout();
            // 
            // lastInteractionTimer
            // 
            this.lastInteractionTimer.Enabled = false;            
            // 
            // FormsPlotSkia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            
            this.Name = "FormsPlotSkia";
            this.Controls.SetChildIndex(this.pbPlot, 0);
            ((System.ComponentModel.ISupportInitialize)(this.lastInteractionTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion         
    }
}
