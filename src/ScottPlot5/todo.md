# ScottPlot 5 Triage/TODO List

The following is a collection of items added here so they won't be forgotten

* Improve design time appearance ([#2101](https://github.com/ScottPlot/ScottPlot/issues/2101))
* Multiplot ([#1337](https://github.com/ScottPlot/ScottPlot/issues/1337))

## API Considerations

### Saving Images

Option 1 has fewer function names but the supported formats are not obvious and using an unsupported extension throws an exception at runtime

```cs
// Option 1: encoding format detected by filename extension
plt.SaveImage("test.jpg", width: 500, height: 400, quality: 85);

// Option 2: encoding format defined by function name
plt.SaveJpeg("test.jpg", width: 500, height: 400, quality: 85);
plt.SavePng("test.png", width: 500, height: 400);
plt.SaveWebp("test.png", width: 500, height: 400);
```