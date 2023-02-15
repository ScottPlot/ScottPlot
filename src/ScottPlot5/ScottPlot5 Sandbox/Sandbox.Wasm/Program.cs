
namespace Sandbox.Uno.Wasm
{
    public class Program
    {
        private static Sandbox.WinUI.App? _app;

        static int Main(string[] args)
        {
            Microsoft.UI.Xaml.Application.Start(_ => _app = new Sandbox.WinUI.App());

            return 0;
        }
    }
}
