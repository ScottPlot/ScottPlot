# Contribute to ScottPlot

**ScottPlot welcomes contributions from the open-source community!** Bug reports, questions, and feature suggestions are welcome as [issues](https://github.com/swharden/ScottPlot/issues), [discussions](https://github.com/swharden/ScottPlot/discussions/categories/q-a), or chat in the [ScottPlot Discord](https://ScottPlot.NET/discord).

### Contribute by Addressing Open Issues
One of the most helpful things members of the open-source community can do is to lighten the burden on the core project developers by providing feedback on open issues! Even if someone doesn't know how to resolve an open issue, engaging in conversation to more clearly define the issue and create a working code example that reproduces it is a big win.

### ScottPlot 4
* Enhancements are welcome as long as they do not break the existing API.
* Simple code contributions include adding new plot types, palettes, and colormaps. See [Hacktoberfest issues](https://github.com/ScottPlot/ScottPlot/issues?q=is%3Aissue+hacktoberfest) for examples and recommendations for first-time contributors.

### ScottPlot 5
* Breaking changes are welcome if they make the API objectively better.
* A running list of next steps is maintained in [TODO.cs](src/ScottPlot5/ScottPlot5/TODO.cs)

### Seek Feedback _Before_ Opening Large Pull Requests

Consider creating an issue to discuss your pull request _before_ investing a lot of work into it. This is an opportunity to ensure your modifications (and their timing) will be consistent with the direction ScottPlot is going, also a good chance to get feedback and suggestions that may improve your implementation.

### Run the AutoFormatter

**If your code is not auto-formatted, the build will fail** when you create your pull request. Use [autoformat.bat](src/ScottPlot4/autoformat.bat) or run these commands to autoformat the entire code base:

```sh
dotnet tool update -g dotnet-format
dotnet format
```

### Join the ScottPlot Discord

ScottPlot maintainers and users frequently discuss development ideas and progress in the [ScottPlot Discord channel](https://ScottPlot.NET/discord). We also informally discuss issues and provide feedback on feature ideas and code contributions.