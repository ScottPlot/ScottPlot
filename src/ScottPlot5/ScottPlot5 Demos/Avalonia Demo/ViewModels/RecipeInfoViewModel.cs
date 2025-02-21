using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlotCookbook;
using static ScottPlotCookbook.JsonCookbookInfo;

namespace Avalonia_Demo.ViewModels;

public partial class RecipeInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private IRecipe _recipe;

    [ObservableProperty]
    private JsonRecipeInfo _recipeInfo;
}
