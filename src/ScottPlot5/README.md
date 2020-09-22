# ScottPlot 5 

**⚠️ WARNING:** ScottPlot 5 is early in development and highly experimental at this time.

### Design Goals

* Figure items (ticks, axis labels, grid, etc.) will implement `IRenderable` and track state (styling) and handle rendering in the same class. This makes experimental renderables (like new tick systems) easy to implement and test.

* Support for an arbitrary number of coordinate planes (allowing plots with two different Y scales)

* Improved rendering system
  * The Plot module will not store Bitmap objects
  * Existing Bitmaps can be passed-in to render onto, improving performance of user controls
  * Architecture is designed for eventual support of System.Drawing alternatives like SkiaSharp and ImageSharp

* Mouse interactivity of all user controls will be abstracted into a `PlotControl` module, ensuring all user controls to have the same functionality and options without requiring maintenance of duplicate code for each control.

* All plottables will have improved error-checking when data is modified

* After things settle down, work through every item in [#412](https://github.com/swharden/ScottPlot/issues/412) to ensure nothing was forgotten
