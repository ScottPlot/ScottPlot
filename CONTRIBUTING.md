# How to Contribute to ScottPlot

ScottPlot is made better by contributions from the open-source community!

## Report a Bug

* Create an [issue](https://github.com/swharden/ScottPlot/issues) for the bug
  * Include a **code sample** we can use to reproduce the bug
  * Indicate which ScottPlot **version** you are using
  * Include a screenshot if it would help communicate the problem

## Suggest a Feature
* Check the [ScottPlot roadmap](https://github.com/swharden/ScottPlot/blob/master/dev/roadmap.md) to see if the feature is already planned for a future release
* Open an [issue](https://github.com/swharden/ScottPlot/issues) and describe the feature as clearly as possible
* Include example source code demonstrating how a user might use this new feature
* Include pictures if it would help communicate the new feature

## Fix a Bug or Add a Small Feature
* Open a new GitHub pull request with the patch
* ⚠️ To make your change easy to review, only modify the minimum number of lines/files necessary to implement the change
* If you are considering make a large change, create an issue first so the authors may suggest ideal implementation
* You don't have to create an issue if the bug is sufficiently described in the pull request

## Automatic Code Formatting
The build will fail if code in pull requests has not been auto-formatted. Auto-format your code in Visual Studio, or from the console using these commands:
```
cd ScottPlot/src/
dotnet tool install --global dotnet-format
dotnet format
```

## Modify an Existing Plottable Object

**For extensive changes,** duplicate existing plottable source code (adding `Experimental` to the end of its name) and follow the steps below to create a new Plottable.

**For minimal changes** (e.g., additional styling options) modify the Plottable object source code directly, but consider these points:
  * Add public properties which control the new behavior
  * ⚠️ Do not change constructor arguments of existing plottables\
  _the goal is to avoid changing code in the Plot module_
  * Create a new test (similar to step 2 below) demonstrating this new behavior by instantiating the Plottable, assigning to the new public properties you created, and displaying the output.
  * Make a pull request

## Create a New Plottable Object

💡 **You can create a new Plottable _without modifying any code in the ScottPlot module!_** By writing code only inside 2 new files in the tests folder, new plottables become easier to write, easier to review, and have no risk of breaking anything in ScottPlot.

* **Step 1:** Create a new `PlottableSomething` in `/tests/PlotTypes/PlottableSomething.cs`\
_Your plottable will inherit from `ScottPlot.Plottable`.\
You can model your plottable after [PlottablePolygon.cs](https://github.com/swharden/ScottPlot/blob/master/src/ScottPlot/plottables/PlottablePolygon.cs)_.


* **Step 2:** Create `/tests/PlotTypes/PlottableSomethingTests.cs` and demonstrate the various features of your new plottable. In addition to serving as tests, these code examples may be copied to the Cookbook in the future.\
_You can model your demonstration code after [Polygon.cs](https://github.com/swharden/ScottPlot/blob/master/tests/PlotTypes/Polygon.cs)_.

* **Step 3:** Make a pull request. The open source community will evaluate your new plottable and provide feedback. The pull request page will be used to evaluate and discuss the new plottable.\
\
After development and modification of the new plottable has finished, a ScottPlot administrator will merge the pull request, move the plottable from `/tests` to `/ScottPlot/plottables`, add a public method in the `ScottPlot.Plot` module, and create cookbook examples based on the test code.

## ScottPlot Roadmap

Large-scale future goals for this project are outlined on [dev/roadmap.md](dev/roadmap.md)
