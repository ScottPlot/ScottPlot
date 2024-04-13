# ScottPlot 5 Developer Notes

This folder contains the ScottPlot 5 source code

## Building ScottPlot 5

This project is developed using the latest free version of [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)

💡 **TIP:** Develop using one of the `.slnf` files, not the full `.sln` file

### Solution File (.sln)

* [ScottPlot5.sln](ScottPlot5.sln) contains all ScottPlot projects. It is used to confirm all projects build and pass tests.
* It has many platform targets so it requires many SDKs and is slow to build and test.
* Developers will probably want to unload all projects except those they are working on to reduce build times.
* The CI system builds and tests all platform targets so the developer does not have to.
* Using the full solution file with all projects loaded is recommended for breaking API changes.

### Filtered Solution Files (.slnf)
* [Filtered solution files](https://learn.microsoft.com/en-us/visualstudio/ide/filtered-solutions) contain only the projects necessary to build and test a certain platform
* They are much more convenient to work with because they build very fast
* Developers should open these files if they do not intend to make breaking API changes.

## Testing and Cookbook

* ScottPlot 5 has two test projects: unit tests and the cookbook
* Running the cookbook tests generates the cookbook in `dev/www/cookbook/5.0/`
* Open `index.local.html` to browse the cookbook website locally
* I use [LivePictureViewer](https://github.com/swharden/LivePictureViewer) to observe test-generated images in real time
* To add a new cookbook recipe, add a `class` that inherits `RecipeTestBase` as seen in [Quickstart.cs](https://github.com/ScottPlot/ScottPlot/blob/main/src/ScottPlot5/ScottPlot5%20Cookbook/Recipes/Introduction/Quickstart.cs) or [Bar.cs](https://github.com/ScottPlot/ScottPlot/blob/main/src/ScottPlot5/ScottPlot5%20Cookbook/Recipes/PlotTypes/Bar.cs)
