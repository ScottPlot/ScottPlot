# Contribute to ScottPlot

**ScottPlot welcomes contributions from the open-source community!** To report a bug, ask a question, or suggest a feature [open an issue](https://github.com/swharden/ScottPlot/issues) or ask in the [ScottPlot Discord](https://ScottPlot.NET/discord).

## How To Contribute

### Answer Questions
One of the most helpful things members of the open-source community can do is to lighten the burden on the core project developers by answering questions and providing feedback on open issues! Even if someone doesn't know how to resolve an open issue, engaging in conversation to more clearly define the issue and create a working code example that reproduces it is a big win. [Join the ScottPlot Discord](https://ScottPlot.NET/discord) to get started!

### Good First Issues
Common first-time contributions include adding or fixing XML docs, creating new plot types, and adding new color palettes. See issues labeled [Good First Issue]([https://github.com/ScottPlot/ScottPlot/issues?q=is%3Aissue+hacktoberfest](https://github.com/ScottPlot/ScottPlot/issues?q=label%3A%22Good+First+Issue%22) (including closed issues) and the latest [Hacktoberfest issue](https://github.com/ScottPlot/ScottPlot/search?q=hacktoberfest&type=issues) for code contribution ideas aimed at new contributors.

### How to Add a Cookbook Recipe

Too add a cookbook recipe, create a new class in one of the existing recipe namespaces. For example, [`ScottPlot4/ScottPlot.Cookbook/Recipes/Plottable/Scatter.cs`](https://github.com/ScottPlot/ScottPlot/blob/63bb3e82e9aa13c2881e6d3aea1db8fb8b30cd74/src/ScottPlot4/ScottPlot.Cookbook/Recipes/Plottable/Scatter.cs#L8-L29)

* When you launch the demo application, the recipe will appear in the list.

* When you run the test suite, the cookbook images and code are locally viewable as `src/ScottPlot4/ScottPlot.Cookbook/CookbookOutput/index.dev.html`

### How to Create a New Plot Type
Users can create their own classes that implement `IPlottable` then `Add()` them to a plot, as described in the [ScottPlot FAQ: Create a Custom Plot Types](https://scottplot.net/faq/custom-plot-type/). This does not require downloading this repository or modifying ScottPlot's source code, so it is easy for anybody to get started!

### How to Edit the ScottPlot Website
The [ScottPlot FAQ](https://scottplot.net/faq/) is a collection of web pages generated from Markdown files that can be edited using GitHub. A [Page Source](https://github.com/ScottPlot/ScottPlot.NET) link is at the bottom of all pages on [ScottPlot.NET](https://ScottPlot.NET), and merged changes are automatically deployed to the website. To create a new page, create a new folder mimicking the format of existing pages. The [ScottPlot.NET (the website) Repository](https://github.com/ScottPlot/ScottPlot.NET) has more information about building and testing the website locally for advanced users.

## Pull Requests

### Seek Feedback Before Opening Large PRs
Consider [creating an issue](https://github.com/ScottPlot/ScottPlot/issues) or [posting on Discord](https://ScottPlot.NET/discord) to discuss your pull request _before_ investing a lot of work into it. This is an opportunity to ensure your modifications (and their timing) will be consistent with the direction ScottPlot is going, also a good chance to get feedback and suggestions that may improve your implementation.

### Run the AutoFormatter

**If your code is not auto-formatted, the build will fail** when you create your pull request. Use [autoformat.bat](src/ScottPlot4/autoformat.bat) or run this command to autoformat all code in a solution:

```sh
dotnet format ScottPlot4.sln
```

## Bug Reports

### Reproducible Code Samples

Questions or bug reports provided without code that allows others to reproduce the issue are unlikely to be resolved quickly. Providing a reproducible code sample that allows others to evaluate the bug (and know when it's fixed) makes it easier for others to participate in the discussion and increases the likelihood the issue can be resolved quickly. Here are some example code samples that can be adapted to provide reproducible code for questions and bug reports:

#### Console Application

```
dotnet new console
dotnet add package scottplot
```

```cs
// create a plot
ScottPlot.Plot plt = new();
plt.AddSignal(ScottPlot.Generate.Sin());
plt.AddSignal(ScottPlot.Generate.Cos());

// save the result as an image file
plt.SaveFig("demo.png");
```

#### WinForms Application

```
dotnet new console
dotnet add package scottplot
dotnet add package scottplot.winforms
```

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net6.0-windows</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <UseWindowsForms>true</UseWindowsForms>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="scottplot" Version="4.1.*" />
        <PackageReference Include="scottplot.winforms" Version="4.1.*" />
    </ItemGroup>

</Project>
```

```cs
// create a plot
ScottPlot.Plot plt = new();
plt.AddSignal(ScottPlot.Generate.Sin());
plt.AddSignal(ScottPlot.Generate.Cos());

// display it in a Windows Form
Form form = new() { Width = 600, Height = 400 };
ScottPlot.FormsPlot formsPlot1 = new() { Dock = DockStyle.Fill };
formsPlot1.Reset(plt);
formsPlot1.Refresh();
form.Controls.Add(formsPlot1);
form.ShowDialog();
```
