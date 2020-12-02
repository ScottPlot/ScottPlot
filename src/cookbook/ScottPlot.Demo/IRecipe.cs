namespace ScottPlot.Demo
{
    public interface IRecipe
    {
        string name { get; }
        string description { get; }

        string categoryMajor { get; }
        string categoryMinor { get; }
        string categoryClass { get; }

        string classPath { get; } // used to instantiate new instances of the recipe by its name
        string id { get; } // unique identifier used for filenames and URLs
        string sourceFile { get; } // path to the cs file containig the source code for the recipe

        void Render(Plot plt);
        string GetSourceCode(string pathDemoFolder);
    }
}
