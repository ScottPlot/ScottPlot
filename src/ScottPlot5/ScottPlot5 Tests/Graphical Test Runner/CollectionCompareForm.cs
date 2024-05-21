using GraphicalTestRunner;
using ScottPlot;
using System.Data;
using System.Diagnostics;

namespace Graphical_Test_Runner;

public partial class CollectionCompareForm : Form
{
    FolderComparisonResults? FolderResults = null;

    public CollectionCompareForm()
    {
        InitializeComponent();
        Width = 890;
        Height = 832;

        var docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        string defaultFolder = Path.Combine(docsFolder, @"ScottPlot\TestImageCollections");
        if (Directory.Exists(defaultFolder))
        {
            defaultFolder = Directory.GetDirectories(defaultFolder).Last();
        }
        else
        {
            defaultFolder = "C:/path/to/old/images/";
        }

        tbBefore.Text = defaultFolder;
        tbAfter.Text = Path.GetFullPath(@"..\..\..\..\..\..\..\dev\www\cookbook\5.0\images");

        btnHelp.Click += (s, e) => new HelpForm().Show();

        btnAnalyze.Click += (s, e) =>
        {
            if (btnAnalyze.Text == "Analyze")
                Analyze();
            else
                Stop = true;
        };

        dataGridView1.SelectionChanged += (s, e) =>
        {
            if (FolderResults is null)
                return;

            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 0)
                return;

            string selectedFilename = dataGridView1.SelectedRows[0].Cells[0].Value.ToString()!;
            int index = Array.IndexOf(FolderResults.Filenames, selectedFilename);
            string path1 = FolderResults.GetPath1(index);
            string path2 = FolderResults.GetPath2(index);
            imageComparer1.SetImages(path1, path2);
        };

        dataGridView1.Sorted += (s, e) =>
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
                Recolor(dataGridView1.Rows[i]);
        };

        btnUT.Click += (s, e) =>
        {
            string path = Path.GetFullPath("../../../../../../../src/ScottPlot5/ScottPlot5 Tests/Unit Tests/bin/Debug/net6.0/test-images");
            Process.Start("explorer.exe", path);
        };

        btnCB.Click += (s, e) =>
        {
            string path = Path.GetFullPath("../../../../../../../dev/www/cookbook/5.0/images");
            Process.Start("explorer.exe", path);
        };

        btn1.Click += (s, e) =>
        {
            string path = Path.GetFullPath(tbBefore.Text);
            Process.Start("explorer.exe", path);
        };

        btn2.Click += (s, e) =>
        {
            string path = Path.GetFullPath(tbAfter.Text);
            Process.Start("explorer.exe", path);
        };
    }

    private bool Stop = false;

    private void Analyze()
    {
        FolderResults = new(tbBefore.Text, tbAfter.Text);

        Stop = false;
        btnAnalyze.Text = "Stop";

        DataTable table = new();
        table.Columns.Add("name", typeof(string));
        table.Columns.Add("change", typeof(string));
        table.Columns.Add("total diff", typeof(double));
        table.Columns.Add("max diff", typeof(double));

        dataGridView1.DataSource = table;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.MultiSelect = false;
        dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        int MAX_IMAGE_COUNT = int.MaxValue;
        //MAX_IMAGE_COUNT = 20;

        for (int i = 0; i < Math.Min(FolderResults.ImageDiffs.Length, MAX_IMAGE_COUNT); i++)
        {
            if (Stop)
                break;

            progressBar1.Maximum = FolderResults.ImageDiffs.Length;
            progressBar1.Value = i + 1;
            Text = $"Analyzing {i + 1} of {FolderResults.ImageDiffs.Length}...";
            FolderResults.Analyze(i);
            Application.DoEvents();

            if (checkHideUnchanged.Checked && FolderResults.Summaries[i] == "unchanged")
                continue;

            DataRow row = table.NewRow();
            row.SetField(0, FolderResults.Filenames[i]);
            row.SetField(1, FolderResults.Summaries[i]);
            row.SetField(2, FolderResults.ImageDiffs[i]?.TotalDifference);
            row.SetField(3, FolderResults.ImageDiffs[i]?.MaxDifference);

            table.Rows.Add(row);
            Recolor(dataGridView1.Rows[table.Rows.Count - 1]);
            dataGridView1.AutoResizeColumns();

            if (table.Rows.Count == 1)
                dataGridView1.Rows[0].Selected = true;
        }

        Text = $"Analyzed {FolderResults.ImageDiffs.Length} image pairs";
        progressBar1.Value = 0;
        btnAnalyze.Text = "Analyze";
    }

    private void Recolor(DataGridViewRow row)
    {
        if (row.Cells[1].Value.ToString() == "changed")
        {
            row.Cells[1].Style.BackColor = System.Drawing.Color.Yellow;
        }
        else if (row.Cells[1].Value.ToString() == "unchanged")
        {
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                row.Cells[j].Style.BackColor = SystemColors.ControlLight;
                row.Cells[j].Style.ForeColor = SystemColors.ControlDark;
            }
        }
        else
        {
            row.Cells[1].Style.BackColor = System.Drawing.Color.LightSteelBlue;
        }
    }
}
