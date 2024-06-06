
namespace WinForms_Demo;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();

        // CTRL+D opens this window (useful for testing in development)
        Type testingFormType = typeof(Demos.ShowValueOnHover);

        Application.Run(new MainMenuForm(testingFormType));
    }
}
