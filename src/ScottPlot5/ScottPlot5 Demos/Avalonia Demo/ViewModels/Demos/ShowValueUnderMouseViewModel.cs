using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public enum ClosestValueMode
{
    NearestXY,
    NearestX
}

public partial class ShowValueUnderMouseViewModel : ViewModelBase
{
    [ObservableProperty]
    private ClosestValueMode _closestValueMode = ClosestValueMode.NearestXY;
}
