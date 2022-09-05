using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;
using WinForms.Examples;

namespace ScottPlot.Sandbox.WinForms;

public partial class Form1 : Form
{
    private readonly Dictionary<string, Type> Examples;

    public Form1()
    {
        InitializeComponent();

        Examples = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsSubclassOf(typeof(Form)))
            .Where(x => x.GetInterfaces().Contains(typeof(IExampleForm)))
            .Select(x => (IExampleForm)FormatterServices.GetUninitializedObject(x))
            .ToDictionary(keySelector: x => x.SandboxTitle, elementSelector: x => x.GetType());

        listBox1.Items.AddRange(Examples.Select(x => x.Key).ToArray());

        if (listBox1.Items.Count > 0)
            listBox1.SelectedIndex = 0;
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string title = listBox1.SelectedItem.ToString();
        if (Examples.ContainsKey(title))
        {
            Type formType = Examples[title];
            IExampleForm form = (IExampleForm)FormatterServices.GetUninitializedObject(formType);
            richTextBox1.Text = form.SandboxDescription;
            button1.Enabled = true;
        }
        else
        {
            richTextBox1.Text = string.Empty;
            button1.Enabled = false;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        string title = listBox1.SelectedItem.ToString();
        if (!Examples.ContainsKey(title))
            return;
        Type formType = Examples[title];
        Form form = (Form)Activator.CreateInstance(formType);
        form.StartPosition = FormStartPosition.CenterScreen;
        form.ShowDialog();
    }
}
