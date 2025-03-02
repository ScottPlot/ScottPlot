using Avalonia.Controls;
using Avalonia_Demo.ViewModels;

namespace Avalonia_Demo.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        MainWindowViewModel vm => (MainWindowViewModel)DataContext;
        double margin = 6;
        int div_width = 400;

        private void Window_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            double winwidth = args.NewSize.Width;
            double winheight = args.NewSize.Height;
            int div_n = (int)(winwidth / div_width + 0.5);
            //int div_res =((int)(winheight)) % div_width;
            vm.DemoWidth = (winwidth - margin) / (div_n);
        }
    }
}
