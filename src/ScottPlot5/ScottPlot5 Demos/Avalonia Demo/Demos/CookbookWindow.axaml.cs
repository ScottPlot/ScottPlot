using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System;
using Avalonia.Threading;
using Avalonia_Demo.ViewModels.Demos;
using System.ComponentModel;
using System.Diagnostics;
using ScottPlotCookbook;

namespace Avalonia_Demo.Demos;

public class CookbookDemo : IDemo
{
    public string Title => $"{ScottPlot.Version.LongString} Cookbook";
    public string Description => "Common ScottPlot features demonstrated " +
        "as interactive graphs displayed next to the code used to create them";

    public Window GetWindow()
    {
        return new CookbookWindow();
    }
}

public partial class CookbookWindow : Window
{
    private CookbookViewModel TypedDataContext => (DataContext as CookbookViewModel) ?? throw new ArgumentException(nameof(DataContext));
    public CookbookWindow()
    {
        InitializeComponent();

        DataContext = new CookbookViewModel();

        TypedDataContext.PropertyChanged += HandleDataContextChanged;
        UpdateChart();
    }

    private void HandleDataContextChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(TypedDataContext.SelectedRecipe))
        {
            UpdateChart();
        }
    }

    private void UpdateChart()
    {
        AvaPlot.Reset();

        var recipeDetails = TypedDataContext.GetRecipeDetails(TypedDataContext.SelectedRecipe);

        if (recipeDetails is null)
            return;

        recipeDetails.Value.Recipe.Execute(AvaPlot.Plot);

        AvaPlot.Refresh();
    }
}
