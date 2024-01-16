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
        bool launchDemoFirst = false;
        if (launchDemoFirst && Environment.MachineName == "DESKTOP-L7MMAB7")
        {
            Demos.DataLogger window = new();
            window.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(window);
        }
        else
        {
            Application.Run(new MainMenuForm());
        }
    }
}
