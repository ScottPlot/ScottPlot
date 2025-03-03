using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public partial class ScrollViewerViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _zoomAllowed = true;

    public void SetZoomAllowed(bool zoomAllowed)
    {
        ZoomAllowed = zoomAllowed;
    }
}
