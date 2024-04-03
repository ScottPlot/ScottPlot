namespace WinForms_Demo;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();

        // use this to quickly launch a test Form while developing
        if (true && Environment.MachineName == "DESKTOP-L7MMAB7")
        {
            Demos.SignalXYDrag window = new() { StartPosition = FormStartPosition.CenterScreen };
            Application.Run(window);
            Application.Run(new MainMenuForm());
        }
        else
        {
            Application.Run(new MainMenuForm());
        }
    }
}
