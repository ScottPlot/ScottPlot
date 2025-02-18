namespace Avalonia_Demo.ViewModels;

public partial class DemoMenuItemViewModel : ViewModelBase
{
    public IDemo? Demo { get; set; }

    public string Title  => Demo?.DemoTitle ?? "Title Goes Here";
    public string Description => Demo?.Description ?? "Description goes here";
}
