using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia_Demo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string VersionStringShort => ScottPlot.Version.VersionString;
    public string VersionStringFull => $"ScottPlot.Avalonia Version {ScottPlot.Version.VersionString}";

    [ObservableProperty]
    public double _DemoWidth = 400;

    public ObservableCollection<DemoMenuItemViewModel> DemoWindows { get; } = new(
        System.Reflection.Assembly.GetAssembly(typeof(Views.MainWindow))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IDemo)))
            .Where(x => !x.IsInterface)
            .Select(x => new DemoMenuItemViewModel() { Demo = Activator.CreateInstance(x) as IDemo })
            .Where(x => x is not null)
            .OrderBy(d => d.Demo is Avalonia_Demo.Demos.CookbookDemo ? -1 : 0)
            .ThenBy(d => d.Demo is Avalonia_Demo.Demos.QuickstartDemo ? -1 : 0)
            .ThenBy(d => d.Title)
            .ToList()
    );

}
