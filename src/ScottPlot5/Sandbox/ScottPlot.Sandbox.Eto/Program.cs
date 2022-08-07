using Eto.Forms;


namespace ScottPlot.Sandbox.Eto
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
