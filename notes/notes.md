# Notes

It's time for a ground-up recode. Let's re-assess priorities.

### Target Structure
* distribute `ScottPlot` core class with minimal interaction calls required
* distribute `ScottPlotUC` user control which is the core class plus graphical elements (like mouse click/drag, zooming, scrollbars, clickable markers, etc)

### Target Technique: Build-A-Bitmap
Build bitmaps one "layer" at a time (bitmap in, bitmap out). Primary categories are:
* Frame (background, axis labels, axis ticks)
* Data (line plot, scatter plot, etc)
* Markers (shaded regions, vertical lines, horizontal lines, etc.)
  
### Core `ScottPlot` Class Theoretical Interaction

**DOGMA:** ScottPlot will never _store_ any of your data. It will just briefly let you graph it. When you tell it to draw or plot something, it just takes what you already told it about your axis and perform axis-unit-to-pixel mapping then draw some lines and returns the bitmap. Done.

Create the figure and indicate axis:

```c#
// init a figure (any of these functions auto-renders `sp.bmpFrame` with axis info)
sp = ScottPlot(800, 600);
sp.Resize(640,480);
sp.Axis(X1, X2, Y1, Y2);
sp.Xlabel("horizontal units");
sp.Ylabel("vertical units");
sp.Padding(10,10,40,40);
```

Adding things to the figure:

```c#
sp.Clear() // clear the figure and init with the frame bitmap
sp.SpanH(X1, X2);
sp.SpanV(Y1, Y2);
sp.LineH(Y);
sp.LineV(Y);
sp.Scatter(Xs, Ys);
sp.Line(Xs, Ys);
sp.Line(Ys, Xspacing);
sp.LinePixels(YsLow, YsHigh);
sp.Bitmap(); // returns the final image
```

How do you adjust the colors, transparency, and thickness? There is an _active_ color, and that's always able to be defined manually.
```C#
sp.color=Color.Blue;
sp.alpha=.7;
sp.lineWidth=3;
```

### Considerations
* **Pixel madness:** `Y1 < Y2` and `Y2px <= Y1px` always!
