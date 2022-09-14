using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot5_WinForms_Demo
{
    public partial class DemoButton : UserControl
    {
        public DemoButton()
        {
            InitializeComponent();
            UpdateHighlight();

            foreach (Control control in Controls)
            {
                control.MouseEnter += (s, e) => UpdateHighlight();
                control.MouseLeave += (s, e) => UpdateHighlight();
            }
        }

        private void UpdateHighlight()
        {
            bool mouseIsOverControl = ClientRectangle.Contains(PointToClient(MousePosition));

            Color color = mouseIsOverControl
                ? ColorTranslator.FromHtml("#9a4993")
                : ColorTranslator.FromHtml("#71297f");

            BackColor = color;
            lblTitle.BackColor = color;
            rtbDescription.BackColor = color;
            tableLayoutPanel1.BackColor = color;
            panel1.BackColor = color;
        }
    }
}
