using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public partial class CustomMarkerViewModel : ViewModelBase
{
    [ObservableProperty]
    private double _happiness = 0.25;
}
