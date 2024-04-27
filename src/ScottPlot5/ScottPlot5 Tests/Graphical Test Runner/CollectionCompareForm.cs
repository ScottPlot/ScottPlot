using GraphicalTestRunner;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphical_Test_Runner;

public partial class CollectionCompareForm : Form
{
    FolderComparisonResults? FolderResults = null;
    string? SelectedBeforeImagePath = null;
    string? SelectedAfterImagePath = null;

    public CollectionCompareForm()
    {
        InitializeComponent();

        tbBefore.Text = @"C:\Users\scott\Documents\ScottPlot\TestImageCollections\2024-04-23";
        tbAfter.Text = @"C:\Users\scott\Documents\ScottPlot\TestImageCollections\2024-04-27";

        btnAnalyze.Click += (s, e) =>
        {
            FolderResults = new(tbBefore.Text, tbAfter.Text);

            DataTable table = new();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("change", typeof(string));
            table.Columns.Add("difference", typeof(double));

            for (int i = 0; i < FolderResults.Images.Length; i++)
            {
                progressBar1.Maximum = FolderResults.Images.Length;
                progressBar1.Value = i + 1;
                ImageComparisonDetails image = FolderResults.Images[i];

                if (cbChanged.Checked && image.Change != "changed")
                    continue;

                DataRow row = table.NewRow();
                row.SetField(0, image.Filename);
                row.SetField(1, image.Change);
                row.SetField(2, image.Difference);

                table.Rows.Add(row);
            }

            dataGridView1.DataSource = table;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        };

        dataGridView1.SelectionChanged += (s, e) =>
        {
            if (FolderResults is null)
            {
                SelectedBeforeImagePath = null;
                SelectedAfterImagePath = null;
                return;
            }

            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 0)
            {
                SelectedBeforeImagePath = null;
                SelectedAfterImagePath = null;
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;
            SelectedBeforeImagePath = FolderResults.Images[rowIndex].BeforePath;
            SelectedAfterImagePath = FolderResults.Images[rowIndex].AfterPath;

            pictureBox1.Image = new Bitmap(SelectedBeforeImagePath);
            pictureBox2.Image = new Bitmap(SelectedAfterImagePath);
        };

        pictureBox1.Click += (s, e) =>
        {
            Text = SelectedBeforeImagePath;
        };

        pictureBox2.Click += (s, e) =>
        {
            Text = SelectedAfterImagePath;
        };
    }
}
