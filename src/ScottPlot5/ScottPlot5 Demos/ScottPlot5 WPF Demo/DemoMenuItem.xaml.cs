using System.Windows.Controls;

namespace WPF_Demo;

public partial class DemoMenuItem : UserControl
{
    public DemoMenuItem()
    {
        InitializeComponent();
    }

    public DemoMenuItem(Type type)
    {
        var instance = Activator.CreateInstance(type);
        InitializeComponent();
        GroupBox1.Header = ((IDemoWindow)instance!).DemoTitle;
        TextBlock1.Text = ((IDemoWindow)instance!).Description;
        ((System.Windows.Window)instance).Close();

        Button1.Click += (s, e) => ((System.Windows.Window)Activator.CreateInstance(type)!).Show();
    }
}
