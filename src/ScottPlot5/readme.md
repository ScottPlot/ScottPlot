# ScottPlot 5

* ⚠️ ScottPlot 5 early in development and **not recommended for production use**

* ⚠️ ScottPlot 5 contributions are **not** invited at this time

* ✔️ ScottPlot 4 remains fully supported (and contributions are welcome)

* Development progress is discussed in the [**ScottPlot Discord**](https://ScottPlot.NET/discord)

# ScottPlot 5 Architecture

## Cookbook
* The cookbook is an NUnit test project
* When it runs it outputs HTML pages in /dev/www/cookbook/5.0
* `index.html` pages have pretty folder-level URLs (e.g., `/5.0/#plot`) and are suitable for deploying online
* `index.local.html` pages have ugly URLs that point to HTML files by name (e.g., `/5.0/index.html#plot`) but they can be browsed on a local file system

## Coding

### Drawing
* Although you could draw by calling methods in the `SkiaSharp.SKCanvas` object, favor using methods in `ScottPlot.Drawing` so enhancements and optimizations can be applied system-wide