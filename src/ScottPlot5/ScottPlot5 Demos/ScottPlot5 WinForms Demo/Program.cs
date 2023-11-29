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
        if (Environment.MachineName == "DESKTOP-L7MMAB7")
        {
            Application.Run(new Demos.MultiAxis());
        }

        Application.Run(new MainMenuForm());
    }
}
