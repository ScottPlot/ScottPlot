using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ViewModels.Demos;

public partial class BackgroundImagesViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _showFigureBackground = true;
    
    [ObservableProperty]
    private bool _showDataBackground = true;

    public ImagePosition[] ImagePositions { get; } = Enum.GetValues<ImagePosition>();

    [ObservableProperty]
    private int _imagePositionIndex;

    [ObservableProperty]
    private ImagePosition _selectedImagePosition;

    public BackgroundImagesViewModel()
    {
        PropertyChanged += UpdateCalculatedProperties;
        ImagePositionIndex = Array.FindIndex(ImagePositions, p => p == ImagePosition.Stretch);
    }

    private void UpdateCalculatedProperties(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ImagePositionIndex))
        {
            this.SelectedImagePosition = ImagePositions[ImagePositionIndex];
        }
    }
}
