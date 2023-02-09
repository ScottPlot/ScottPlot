using Microsoft.UI.Xaml.Controls;
using ScottPlot;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sandbox.Uno.WinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.Window.Title = "ScottPlot 5 - WinUI Sandbox";
            WinUIPlot.AppWindow = App.Window;

            WinUIPlot.Plot.Add.Signal(Generate.Sin(51));
            WinUIPlot.Plot.Add.Signal(Generate.Cos(51));
        }
    }
}
