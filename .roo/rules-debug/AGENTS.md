# Debug Rules (Non-Obvious)

- **`RenderManager.LastRender` contains per-action timing** — each render action's elapsed time is stored in `LastRender.ActionTimes`. Use this to pinpoint which step is slow, rather than guessing. See [`RenderManager.cs:132`](src/ScottPlot5/ScottPlot5/Rendering/RenderManager.cs:132).
- **`Render()` retries up to 5 times silently** — if axis limits change mid-render (common with auto-scaling feedback), the render loop re-executes. A "stuck" render may actually be cycling 5 times. Check `LastRender.AxisLimitsChanged` to see if re-renders occurred.
- **`EnableRendering = false` is a hidden kill-switch** — set `Plot.RenderManager.EnableRendering = false` to suppress all rendering without exceptions. Useful for isolating whether rendering is causing an issue.
- **`DisableAxisLimitsChangedEventOnNextRender` prevents infinite loops** — when linking axes across plots, this flag prevents recursive `AxisLimitsChanged` event storms. If linked axes aren't syncing, check if this flag is stuck (it resets automatically after each render).
- **OpenGL plottables render BLACK if GL state is corrupted** — if `StoreGLState`/`RestoreGLState` actions are missing or out of order in the render pipeline, the GL context leaks and SkiaSharp draws black frames.
- **`ContinuousIntegrationBuild=true` only affects SourceLink** — this property controls deterministic builds and source embedding for NuGet symbols. It does NOT affect local build behavior. If debugging NuGet packages, ensure you have SourceLink enabled in your IDE.
- **Unit test image comparison** — graphical tests compare rendered output against reference images. If a render change is intentional, update the reference images in `TestImages/`. Differences are reported as pixel-by-pixel deltas.
