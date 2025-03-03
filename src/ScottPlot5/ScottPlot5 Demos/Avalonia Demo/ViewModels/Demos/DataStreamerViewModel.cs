using Avalonia.Data.Converters;
using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public enum DataStreamerAxisOptions
{
    ManageAxisLimits,
    ContinuouslyAutoscale
}

public enum DataStreamerViewMode
{
    Wipe,
    Scroll
}

public partial class DataStreamerViewModel : ViewModelBase
{
    [ObservableProperty]
    private DataStreamerAxisOptions _selectedDataStreamerAxisOption = DataStreamerAxisOptions.ManageAxisLimits;

    [ObservableProperty]
    private DataStreamerViewMode _selectedDataStreamerViewMode = DataStreamerViewMode.Wipe;

    public void SetViewMode(DataStreamerViewMode mode)
    {
        SelectedDataStreamerViewMode = mode;
    }
}
