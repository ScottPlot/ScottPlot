namespace GraphicalTestRunner;

public partial class HelpForm : Form
{
    public HelpForm()
    {
        InitializeComponent();
        richTextBox1.Text =
            """
            This tool was created to facilitate semi-automated comparison of
            cookbook and test images across different versions of ScottPlot
            so potential errors may be visually identified.

            To use it, create two folders (one for each version of ScottPlot
            you wish to test) and copy cookbook images into each folder.
            
            Images with identical filenames will be compared for differences.
            """;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Close();
    }
}
