# Contribute to ScottPlot

**ScottPlot welcomes contributions from the open-source community!** To report a bug, ask a question, or suggest a feature [open an issue](https://github.com/swharden/ScottPlot/issues) or ask in the [ScottPlot Discord](https://ScottPlot.NET/discord).

### Contribute by Answering Questions
One of the most helpful things members of the open-source community can do is to lighten the burden on the core project developers by answering questions and providing feedback on open issues! Even if someone doesn't know how to resolve an open issue, engaging in conversation to more clearly define the issue and create a working code example that reproduces it is a big win. [Join the ScottPlot Discord](https://ScottPlot.NET/discord) to get started!

### Ideas for New Contributors
Common first-time contributions include adding or fixing XML docs, creating new plot types, and adding new color palettes. See issues labeled [Good First Issue](https://github.com/ScottPlot/ScottPlot/issues?q=is%3Aissue+hacktoberfest) (especially the latest [Hacktoberfest issue](https://github.com/ScottPlot/ScottPlot/search?q=hacktoberfest&type=issues)) for code contribution ideas aimed at new contributors. 

### Seek Feedback Before Opening Large Pull Requests
Consider [creating an issue](https://github.com/ScottPlot/ScottPlot/issues) or [posting on Discord](https://ScottPlot.NET/discord) to discuss your pull request _before_ investing a lot of work into it. This is an opportunity to ensure your modifications (and their timing) will be consistent with the direction ScottPlot is going, also a good chance to get feedback and suggestions that may improve your implementation.

### Run the AutoFormatter

**If your code is not auto-formatted, the build will fail** when you create your pull request. Use [autoformat.bat](src/ScottPlot4/autoformat.bat) or run this command to autoformat all code in a solution:

```sh
dotnet format ScottPlot4.sln
```

### How to Add a Cookbook Recipe

Too add a cookbook recipe, create a new class in one of the existing recipe namespaces. For example, [`ScottPlot4/ScottPlot.Cookbook/Recipes/Plottable/Scatter.cs`](https://github.com/ScottPlot/ScottPlot/blob/63bb3e82e9aa13c2881e6d3aea1db8fb8b30cd74/src/ScottPlot4/ScottPlot.Cookbook/Recipes/Plottable/Scatter.cs#L8-L29)

```cs
public class Quickstart : IRecipe
{
    public ICategory Category => new Categories.PlotTypes.Scatter();
    public string ID => "scatter_quickstart";
    public string Title => "Scatter Plot Quickstart";
    public string Description =>
        "Scatter plots are best for small numbers of paired X/Y data points. " +
        "For evenly-spaced data points Signal is much faster.";

    public void ExecuteRecipe(Plot plt)
    {
        // create sample X/Y data
        int pointCount = 51;
        double[] x = DataGen.Consecutive(pointCount);
        double[] sin = DataGen.Sin(pointCount);
        double[] cos = DataGen.Cos(pointCount);

        // add scatter plots
        plt.AddScatter(x, sin);
        plt.AddScatter(x, cos);
    }
}
```

* When you launch the demo application, the recipe will appear in the list.

* When you run the test suite, the cookbook images and code are locally viewable as `src/ScottPlot4/ScottPlot.Cookbook/CookbookOutput/index.dev.html`

### How to Create a New Plot Type
Users can create their own classes that implement `IPlottable` then `Add()` them to a plot, as described in the [ScottPlot FAQ: Create a Custom Plot Types](https://scottplot.net/faq/custom-plot-type/). This does not require downloading this repository or modifying ScottPlot's source code, so it is easy for anybody to get started!

### Edit the ScottPlot FAQ Website
The [ScottPlot FAQ](https://scottplot.net/faq/) is a collection of web pages generated from Markdown files that can be edited using GitHub. A [Page Source](https://github.com/ScottPlot/ScottPlot.NET) link is at the bottom of all pages on [ScottPlot.NET](https://ScottPlot.NET), and merged changes are automatically deployed to the website. To create a new page, create a new folder mimicking the format of existing pages. The [ScottPlot.NET (the website) Repository](https://github.com/ScottPlot/ScottPlot.NET) has more information about building and testing the website locally for advanced users.