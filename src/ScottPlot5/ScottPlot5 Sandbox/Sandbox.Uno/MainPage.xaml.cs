using Microsoft.UI.Xaml.Controls;
using ScottPlot;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sandbox.Uno
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            UnoPlot.Plot.Add.Signal(Generate.Sin(51));
            UnoPlot.Plot.Add.Signal(Generate.Cos(51));
        }
    }
}
