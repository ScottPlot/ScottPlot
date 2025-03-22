using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlotCookbook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using static ScottPlotCookbook.JsonCookbookInfo;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace Avalonia_Demo.ViewModels.Demos;

public class TreeViewNode : ObservableObject
{
    private readonly CookbookViewModel _viewModel;
    public string Name { get; init; }
    public TreeViewNode[] Children { get; init; }
    public bool IsLeafNode => Children.Length == 0;
    public void Select()
    {
        if (IsLeafNode)
        {
            _viewModel.SetSelectedRecipe(Name);
        }
    }

    public TreeViewNode(CookbookViewModel viewModel)
    {
        _viewModel = viewModel;
    }
}

public struct RecipeDetails
{
    public readonly IRecipe Recipe { get; init; }
    public readonly JsonRecipeInfo? RecipeInfo { get; init; }
}

public partial class CookbookViewModel : ViewModelBase
{
    private readonly Dictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory = Query.GetRecipesByCategory();

    private readonly JsonCookbookInfo? JsonInfo = null;

    public ObservableCollection<TreeViewNode> Categories => new(GetCategories());

    private IEnumerable<TreeViewNode> GetCategories()
    {
        foreach (Chapter chapter in Query.GetChaptersInOrder())
        {
            IEnumerable<ICategory> categoriesInChapter = RecipesByCategory.Keys.Where(x => x.Chapter == chapter);

            foreach (var category in categoriesInChapter)
            {
                var children = RecipesByCategory[category].Select(r => new TreeViewNode(this)
                {
                    Name = r.Name,
                    Children = []
                })
                .ToArray();

                if (children.Length == 0)
                    continue;

                yield return new TreeViewNode(this)
                {
                    Name = category.CategoryName,
                    Children = children
                };
            }
        }

    }

    [ObservableProperty]
    private string? _selectedRecipe = null;

    [ObservableProperty]
    private RecipeDetails? _selectedRecipeDetails;

    [ObservableProperty]
    private RecipeInfoViewModel? _recipeViewModel;

    public RecipeDetails? GetRecipeDetails(string? recipeName)
    {
        if (recipeName is null)
            return null;

        var recipe = RecipesByCategory
            .SelectMany(x => x.Value)
            .Where(r => r.Name == recipeName)
            .SingleOrDefault();

        var recipeInfo = JsonInfo?.Recipes.Where(r => r.Name == recipeName).SingleOrDefault();

        if (recipe is null)
            return null;

        return new() { Recipe = recipe, RecipeInfo = recipeInfo };
    }

    public void SetSelectedRecipe(string? recipe)
    {
        SelectedRecipe = recipe;
        SelectedRecipeDetails = GetRecipeDetails(recipe);

        if (!SelectedRecipeDetails.HasValue || !SelectedRecipeDetails.Value.RecipeInfo.HasValue)
        {
            RecipeViewModel = null;
        }
        else
        {
            RecipeViewModel = new()
            {
                Recipe = SelectedRecipeDetails.Value.Recipe,
                RecipeInfo = SelectedRecipeDetails.Value.RecipeInfo.Value
            };
        }
    }

    public CookbookViewModel()
    {
        string jsonFilePathInRepo = Path.GetFullPath("../../../../../../../dev/www/cookbook/5.0/recipes.json");
        string jsonFilePathHere = Path.GetFullPath("recipes.json");

        if (File.Exists(jsonFilePathInRepo))
        {
            JsonInfo = JsonCookbookInfo.FromJsonFile(jsonFilePathInRepo);
        }
        else if (File.Exists(jsonFilePathHere))
        {
            JsonInfo = JsonCookbookInfo.FromJsonFile(jsonFilePathHere);
        }

        SetSelectedRecipe(Categories[0]?.Children[0].Name);
    }
}
