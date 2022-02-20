# ScottPlot 5

[![Test ScottPlot v5](https://github.com/ScottPlot/ScottPlot/actions/workflows/ci-ScottPlot-v5.yaml/badge.svg)](https://github.com/ScottPlot/ScottPlot/actions/workflows/ci-ScottPlot-v5.yaml)

### ⚠️ WARNING: This project is pre-alpha and not ready for use

This project explores feasibility of cutting the dependency on `System.Drawing.Common` in favor of a rendering system using `Microsoft.Maui.Graphics` (#1036). Depending on what is discovered here, code here may evolve into the next major version of ScottPlot (version 5).

## Difficult Problems

The following list of features/behaviors have been unusually difficult in previous versions of ScottPlot. This version will attempt to solve these issues:

* **Automatic layout:** Circular logic problem where layout padding depends on tick label size, tick positions and labels depend on tick density, tick density depends on the layout and padding.

* **Tick customization:** Ticks can be customized in numerous ways. Custom density, manual placement, DateTime format, custom styling, etc. ScottPlot5 will have an `ITickGenerator` that advanced users can implement to create custom tick systems.

* **Plottables with growing data:** Users often find it hard to add data points to existing plottables. ScottPlot 5 will have separate plottables optimized for adding/removing data (using `List<T>`) vs. those optimized for speed (using `Array<T>` or `Span<T>`). _Support for generic math would have been nice, but that is only supported on .NET 6 with preview features enabled._

* **Managing state:** The `Plot` module itself will be as stateless as possible to simplify testing and minimize surprising behavior. Interactive controls may store state (rather than the `Plot` module) for actions like mouse-panning. These are the main categories of state that require consideration:
  * List of plottables (each with their own stored state)
  * List of Axes (each with their own stored state)
  * Plot styling (e.g., background color)
  * Palette (default colors for new plottables)
  * Axis Limits
  * Figure Size

* **Mouse customizations:** Controls should be easy to customize, allowing the user to define which mouse buttons perform which actions, and making it easy to customize behaviors like the right-click menu.

## Goals
* Support back to `.NET Standard 2.0` and `.NET Framework 4.6.2`
* no `System.Drawing` anywhere in the project
* nullable support throughout the code base #691
* hardware-accelerated rendering in controls using SkiaSharp and OpenGL
* stateless rendering system to improve support for multi-threading and testing
  * size will never be stored - it will be passed-in and the layout will be recalculated on each render
  * mouse positions or state should not be stored - mouse actions (e.g., click/drag) will be built by controls and passed in

## Limitations
These limitations are because Maui.Graphics is not fully mature yet, but:
* There is no `Microsoft.Maui.Graphics.ICanvas.MeasureString()`
* `Microsoft.Maui.Graphics.ICanvas.DrawString()` does not support custom fonts

## Naming

Mimic Matplotlib where possible

![](https://matplotlib.org/stable/_images/sphx_glr_anatomy_001.png)
* https://matplotlib.org/stable/gallery/showcase/anatomy.html

### Resources
* https://maui.graphics