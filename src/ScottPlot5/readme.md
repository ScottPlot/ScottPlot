# ScottPlot 5 Development

This is extremely experimental code.

## Justification & Previous Discussion

* [Issue #1036](https://github.com/ScottPlot/ScottPlot/issues/1036) - initial plan to refactor/rewrite to cut System.Drawing dependency

* [PR #1647](https://github.com/ScottPlot/ScottPlot/pull/1647#issuecomment-1058725910) - what I learned after a rough draft rewrite with Maui.Graphics

## Design Decisions

### Why not abstract the rendering library?

I could create an `IRenderer` with methods like `DrawLine()` and have separate packages like `ScottPlot.SkiaSharp` implement it with platform-specific drawing methods.

Pros:

* This would allow ScotPlot to support multiple rendering platforms: SkiaSharp, System.Drawing, and maybe even SVG.

Cons:

* Many small packages to maintain
* User controls would all use the SkiaSharp package anyway
* Some features like line styles, advanced gradients, image warping, etc. do not translate well across drawing platforms, so the interface would have to bend to the lowest common denominator. Just targeting the most capable one (SkiaSharp) would allow full use of all its features.
* Optimizations aren't consistent across rendering technologies (e.g., shortcuts for anti-aliased).

### What about render steps?

Rather than directly interacting with an abstract renderer, I could describe the plot to be drawn as a collection of `RenderStep` objects, then a rendering system could take that collection and render it step by step.

Pros:

* Steps would be easy to document with XML comments, and refactoring would be powerful
* It would be easy to debug since steps could be converted to human-readable logs

Cons:

* Same reasons for not abstracting the rendering library. I'd rather target a single, powerful, rendering platform. I'll intentionally undertake that dependency.

### Why SkiaSharp? Is it OK to depend on it?

Pros:

* It's supported virtually everywhere

Cons:

* It's big
* It used to have a complex build system, but it's gotten better

### Why create your own ScottPlot.Color

I don't want to re-invent the wheel, but a custom `Color` seems like the best choice.

I don't like the idea of requiring users to pass a `SKColor` everywhere.

I also like the idea of adding palette and colorbar functionality somehow. `Colors.C0`, `Colors.C1`, etc. returning the first few colors of the default palette would be consistent with matplotlib. 

Name known colors as `Colors.Blue` instead of `Color.Blue` to avoid conflicts with `System.Drawing` which will continue to be a default `using` for new WinForms apps.

I'm also creating my own `LineStyle` and `MarkerShape` classes, so it's not that surprising.

### Why not support .NET Standard / .NET Framework?

I'm not committed one way or the other yet.

Pros:

* The build system is much better. I don't want to deal with msbuild, especially in CI/CD pipeline.
* Newer features like nullable, Span<T>, Memory<T>, generic math, etc. work out of the box.

Cons:

* .NET Framework 4.8 will be supported officially for many years to come, so intentionally not supporting it could limit a good number of potential users. 
  * Are these the users ScottPlot wants a asking questions and posting feature requests? 👿
  * ScottPlot 4 works fine for .NET Standard / .NET Framework

## Supported Features

I'm noting these here so I can ensure they are supported before release

* display scaling and mouse tracking