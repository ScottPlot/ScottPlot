using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public partial class MultiThreadingViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _buttonsAreEnabled = true;

    [ObservableProperty]
    public string? _timerInUse = null;
}
