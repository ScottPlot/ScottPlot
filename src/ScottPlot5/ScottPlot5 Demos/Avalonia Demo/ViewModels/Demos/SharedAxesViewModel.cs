using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public partial class SharedAxesViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _shareX = true;
    
    [ObservableProperty]
    private bool _shareY = true;
}
