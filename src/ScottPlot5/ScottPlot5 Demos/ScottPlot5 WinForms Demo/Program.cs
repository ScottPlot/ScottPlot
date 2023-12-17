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
            Application.Run(new Demos.ShowValueOnHover());
        }
        else
        {
            Application.Run(new MainMenuForm());
        }
    }
}
