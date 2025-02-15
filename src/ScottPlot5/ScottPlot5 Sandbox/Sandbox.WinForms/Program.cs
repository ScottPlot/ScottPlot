namespace Sandbox.WinForms;

static class Program
{
    [STAThread]
    static void Main()
    {
#if NETFRAMEWORK
#else
        ApplicationConfiguration.Initialize();
#endif
        Application.Run(new Form1());
    }
}
