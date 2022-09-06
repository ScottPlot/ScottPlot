using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace ScottPlot5_WinForms_Demo;

public partial class Form1 : Form
{
    private readonly Dictionary<string, Type> Examples;

    public Form1()
    {
        InitializeComponent();
        Text = ScottPlot.Version.VersionString + " WinForms Demo";

        Examples = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsSubclassOf(typeof(Form)))
            .Where(x => x.GetInterfaces().Contains(typeof(IDemoForm)))
            .Select(x => (IDemoForm)FormatterServices.GetUninitializedObject(x))
            .ToDictionary(keySelector: x => x.Title, elementSelector: x => x.GetType());

        listView1.Items.Clear();
        Examples.Select(x => x.Key).ToList().ForEach(x => listView1.Items.Add(x));

        listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
        btnLaunch.Click += BtnLaunch_Click;

        listView1.Items[0].Selected = true;
    }

    private void ListView1_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 0)
            return;
        Type formType = Examples[listView1.SelectedItems[0].Text];
        IDemoForm form = (IDemoForm)FormatterServices.GetUninitializedObject(formType);
        lblTitle.Text = form.Title;
        rtbDescription.Text = form.Description;
    }

    private void BtnLaunch_Click(object? sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 0)
            return;

        Type formType = Examples[listView1.SelectedItems[0].Text];
        Form form = (Form)Activator.CreateInstance(formType)!;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Text = listView1.SelectedItems[0].Text;
        form.Width = 800;
        form.Height = 500;
        form.ShowDialog();
    }
}
