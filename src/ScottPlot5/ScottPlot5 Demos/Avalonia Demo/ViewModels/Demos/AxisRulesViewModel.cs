using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public partial class AxisRulesCategoryViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _category;

    [ObservableProperty]
    private ObservableCollection<string> _options;

    private AxisRulesViewModel axisRulesViewModel;

    public AxisRulesCategoryViewModel(AxisRulesViewModel axisRulesViewModel, string category, IEnumerable<string> options)
    {
        this.axisRulesViewModel = axisRulesViewModel;
        Category = category;
        Options = new(options);
    }

    public void Select(string option)
    {
        this.axisRulesViewModel.Select(Category, option);
    }
}

public partial class AxisRulesViewModel : ViewModelBase
{
    public ObservableCollection<AxisRulesCategoryViewModel> Categories => [
        new(this, "Boundary", ["Minimum", "Maximum"]),
        new(this, "Square Scaling", ["Preserve X", "Preserve Y", "Zoom Out"]),
        new(this, "Span Limit", ["Minimum", "Maximum"]),
        new(this, "Axis Lock", ["Horizontal", "Vertical"]),
    ];

    [ObservableProperty]
    private (string Category, string Option)? _selected = null;

    [ObservableProperty]
    private bool _invertX = false;

    [ObservableProperty]
    private bool _invertY = false;

    [ObservableProperty]
    private bool _buttonsAreEnabled = true;

    public void Select(string category, string option)
    {
        Selected = (category, option);
        ButtonsAreEnabled = false;
    }

    public void ToggleInvertAxis(string axis)
    {
        if (axis == "x")
        {
            InvertX = !InvertX;
        }
        else if (axis == "y")
        {
            InvertY = !InvertY;
        }
    }

    public void Reset()
    {
        Selected = null;
        InvertX = false;
        InvertY = false;
        ButtonsAreEnabled = true;
    }
}
