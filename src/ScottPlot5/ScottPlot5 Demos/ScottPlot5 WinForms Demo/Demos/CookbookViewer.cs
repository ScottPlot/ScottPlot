using ScottPlot5_WinForms_Demo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Demos;
public partial class CookbookViewer : Form, IDemoForm
{
    public string Title => "ScottPlot Cookbook";

    public string Description => "Common ScottPlot features demonstrated " +
        "as interactive graphs displayed next to the code used to create them";

    public CookbookViewer()
    {
        InitializeComponent();
    }
}
