using ScottPlotCookbook;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Sandbox.Blazor.WebAssembly.Services
{
    public class RecipesService : IRecipesService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<ICategory, IEnumerable<IRecipe>> _recipesByCategory = Query.GetRecipesByCategory();
        private JsonDocument? JsonRecipes = null;

        public ReadOnlyDictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory => _recipesByCategory.AsReadOnly();

        public event Action? RecipeChanged;
        public event Action? BackendChanged;
        public IRecipe? Recipe
        {
            get => _recipe;
            set
            {
                if (value == _recipe)
                    return;
                SetRecipe(value);
                RecipeChanged?.Invoke();
            }
        }
        private IRecipe? _recipe = null;

        public bool HasRecipe => _recipe != null;
        public string RecipeName => _recipe?.Name ?? string.Empty;
        public string RecipeDescription => _recipe?.Description ?? string.Empty;
        public bool HasSourceCode => !string.IsNullOrEmpty(SourceCode);
        public string SourceCode { get; private set; } = string.Empty;
        public async Task GetRecipesAsync()
        {
            if (JsonRecipes is null)
            {
                string json = await _httpClient.GetStringAsync("sample-data/recipes.json");
                JsonRecipes = JsonDocument.Parse(json);
            }
        }

        private bool _showOpenGL = false;
        public bool ShowOpenGL
        {
            get => _showOpenGL;
            set
            {
                if (_showOpenGL == value) return;
                _showOpenGL = value;
                BackendChanged?.Invoke();
            }
        }
        private void SetRecipe(IRecipe? recipe)
        {
            _recipe = recipe;
            string source = "// source code not found";
            if (JsonRecipes != null && HasRecipe)
            {
                foreach (JsonElement recipeElement in JsonRecipes.RootElement.GetProperty("recipes").EnumerateArray())
                {
                    string name = recipeElement.GetProperty("name").GetString() ?? string.Empty;
                    string desc = recipeElement.GetProperty("description").GetString() ?? string.Empty;
                    if (name == Recipe!.Name && desc == Recipe!.Description)
                    {
                        source = recipeElement.GetProperty("source").GetString() ?? string.Empty;
                        break;
                    }
                }
            }
            SourceCode = source;
        }
        public bool IsSelected(IRecipe? recipe)
        {
            return recipe?.Name == _recipe?.Name && recipe?.Description == _recipe?.Description;
        }

        public bool FindRecipe(string name)
        {
            foreach (var category in RecipesByCategory)
            {
                foreach (var recipe in category.Value)
                {
                    if (recipe.GetType().Name == name)
                    {
                        SetRecipe(recipe);
                        return true;
                    }
                }
            }
            return false;
        }
        public RecipesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
