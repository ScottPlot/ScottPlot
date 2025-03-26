using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlotCookbook;
using static ScottPlotCookbook.JsonCookbookInfo;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace Avalonia_Demo.ViewModels;

public partial class RecipeInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private IRecipe _recipe;

    [ObservableProperty]
    private JsonRecipeInfo _recipeInfo;
}
