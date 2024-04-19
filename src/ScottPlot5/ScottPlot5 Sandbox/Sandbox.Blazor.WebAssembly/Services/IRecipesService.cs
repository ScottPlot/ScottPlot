using ScottPlotCookbook;
using System.Collections.ObjectModel;

namespace Sandbox.Blazor.WebAssembly.Services
{
    public interface IRecipesService
    {
        public ReadOnlyDictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory { get; }
        public event Action? RecipeChanged;
        public event Action? BackendChanged;

        public bool ShowOpenGL { get; set; }

        public bool IsSelected(IRecipe? recipe);
        public bool HasRecipe { get; }
        public string RecipeName { get; }
        public string RecipeDescription { get; }

        public IRecipe? Recipe { get; set; }
        public string SourceCode { get; }
        public bool HasSourceCode { get; }
        public bool FindRecipe(string name);
    }
}
