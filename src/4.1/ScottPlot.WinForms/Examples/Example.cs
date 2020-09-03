using System;
using System.Windows.Forms;

namespace ScottPlot.WinForms.Examples
{
    public class Example
    {
        public string Title;
        public string Description;
        public Type FormType;
        public override string ToString() => Title;

        public Example(Type formType, string title, string description)
        {
            if (!formType.IsSubclassOf(typeof(Form)))
                throw new ArgumentException("formType must be a Form");
            (FormType, Title, Description) = (formType, title, description);
        }

        public void Launch(bool blocking = false)
        {
            Form form = (Form)Activator.CreateInstance(FormType);
            if (blocking)
                form.ShowDialog();
            else
                form.Show();
        }
    }
}
