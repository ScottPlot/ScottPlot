# ScottPlot 5

### ⚠️ WARNING: This project is pre-alpha and not ready for use

This project explores feasibility of cutting the dependency on `System.Drawing.Common` in favor of a rendering system using `Microsoft.Maui.Graphics` (#1036). Depending on what is discovered here, code here may evolve into the next major version of ScottPlot (version 5).

### Goals
* Support back to `.NET Standard 2.0` and `.NET Framework 4.6.2`
* no `System.Drawing` anywhere in the project
* nullable support throughout the code base #691
* hardware-accelerated rendering in controls using SkiaSharp and OpenGL
* stateless rendering system to improve support for multi-threading and testing
  * size will never be stored - it will be passed-in and the layout will be recalculated on each render
  * mouse positions or state should not be stored - mouse actions (e.g., click/drag) will be built by controls and passed in

### Limitations
These limitations are because Maui.Graphics is not fully mature yet, but:
* There is no `Microsoft.Maui.Graphics.ICanvas.MeasureString()`
* `Microsoft.Maui.Graphics.ICanvas.DrawString()` does not support custom fonts

### Resources
* https://maui.graphics