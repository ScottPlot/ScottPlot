
namespace WinForms_Demo;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();

        Type testingFormType = typeof(Demos.CookbookViewer);
        Application.Run(new MainMenuForm(testingFormType));
    }
}
