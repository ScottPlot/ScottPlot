using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia_Demo.ViewModels.Demos;

public partial class SimpleDemoViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _demoName;

    public SimpleDemoViewModel(string demoName)
    {
        DemoName = demoName;
    }
}
