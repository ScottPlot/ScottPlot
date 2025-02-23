using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public enum PlotType
{
    Scatter,
    Signal,
    SignalConst
}

public partial class ScatterVsSignalViewModel : ViewModelBase
{
    [ObservableProperty]
    private PlotType _plotType = PlotType.Scatter;

    public ObservableCollection<PlotType> PlotTypes { get; } = new(Enum.GetValues<PlotType>());

    [ObservableProperty]
    private int _numberOfPoints = 10_000;

    [ObservableProperty]
    private int _numberOfPointsOptionIndex;

    public int[] NumberOfPointOptions { get; } = [1000, 10_000, 100_000, 1_000_000, 10_000_000];

    public ScatterVsSignalViewModel()
    {
        PropertyChanged += UpdateCalculatedProperties;
        NumberOfPointsOptionIndex = Array.FindIndex(NumberOfPointOptions, n => n == 10_000);
    }

    public void SetPlotType(PlotType plotType)
    {
        PlotType = plotType;
    }

    private void UpdateCalculatedProperties(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NumberOfPointsOptionIndex))
        {
            this.NumberOfPoints = NumberOfPointOptions[NumberOfPointsOptionIndex];
        }
    }
}
