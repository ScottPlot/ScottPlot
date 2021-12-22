using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class Styles : Form
    {
        private void InitializeComponent()
        {
            this.lbStyles = new ListBox();
            this.groupBox1 = new GroupBox();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.groupBox2 = new GroupBox();
            this.lbPalettes = new ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();

            groupBox1.Content = lbStyles;
            groupBox2.Content = lbPalettes;

            var layout = new DynamicLayout() { DefaultSpacing = Size.Empty + 5, Padding = 5 };
            layout.AddSeparateRow(groupBox1, groupBox2, formsPlot1);

            this.Content = layout;
            // 
            // lbStyles
            // 
            this.lbStyles.Font = Fonts.Sans(12);
            // 
            // groupBox1
            // 
            this.groupBox1.Font = Fonts.Sans(12);
            this.groupBox1.Size = new Size(215, 423);
            this.groupBox1.Text = "Styles";
            // 
            // formsPlot1
            // 
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            // 
            // groupBox2
            // 
            this.groupBox2.Font = Fonts.Sans(12);
            this.groupBox2.Size = new Size(215, 423);
            this.groupBox2.Text = "Palettes";
            // 
            // lbPalettes
            // 
            this.lbPalettes.Font = Fonts.Sans(12);
            // 
            // Styles
            // 
            this.ClientSize = new Size(1073, 447);
            this.Title = "ScottPlot Styles";
            this.groupBox1.ResumeLayout();
            this.groupBox2.ResumeLayout();
            this.ResumeLayout();
        }

        private ListBox lbStyles;
        private GroupBox groupBox1;
        private ScottPlot.Eto.PlotView formsPlot1;
        private GroupBox groupBox2;
        private ListBox lbPalettes;
    }
}
