namespace Avalonia_Demo.ViewModels;

public partial class DemoMenuItemViewModel : ViewModelBase
{
    public IDemo? Demo { get; set; }

    public string Title => Demo?.Title ?? "Title Goes Here";
    public string Description => Demo?.Description ?? "Description goes here";

    public void OpenDemo()
    {
        if (Demo is null)
        {
            throw new System.ArgumentNullException("No demo provided");
        }

        Demo?.GetWindow().Show();
    }
}
