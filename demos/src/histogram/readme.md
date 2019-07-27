## Thoughts on Histogram API
How should ScottPlot allow the user to create the histogram they want? 

A good discussion on this was led by [issue #30](https://github.com/swharden/ScottPlot/issues/30)

### API Sandbox
A fully-default histogram is created like this.
```
// a histogram (with default behavior) can be plotted like this
var hist = new ScottPlot.Histogram(values);
plt.PlotBar(hist.bins, hist.counts);
```

What histogram properties would the user desire to control? It seems there are 3 core settings, all of which can be constructor arguments...

* `double? min = null` and `double? max = null`
  * if set, use that number
  * if null, use mean +/- stdev * 3
* `double? binSize = null` and `double? binCount = null`
  * if only `binSize` is set calculate `binCount`
  * if only `binCount` is set calculate `binSize`
  * if both are null default to `binCount = 100` and calculate `binSize`
  * if both are set throw an exception
* `bool catchOutOfBounds = false`
  * if `false` totally ignore out-of-bound values
  * if `true` place those values in two extra bins