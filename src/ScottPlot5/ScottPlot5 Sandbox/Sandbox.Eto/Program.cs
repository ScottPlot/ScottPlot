using Eto.Forms;


namespace Sandbox.Eto
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Application(global::Eto.Platform.Detect).Run(new MainWindow());
        }
    }
}
