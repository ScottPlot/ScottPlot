# ScottPlot 4.1.13-beta API

This documentation is specific to ScottPlot version `4.1.13-beta`. 

Information about other versions and additional documentation can be found at **http://swharden.com/scottplot**

See the [**ScottPlot Cookbook**](https://swharden.com/scottplot/cookbook) and [**ScottPlot FAQ**](https://swharden.com/scottplot/faq) for additional documentation.

### The Plot Module

* Plots are managed by interacting with the [**`ScottPlot.Plot`**](xref:ScottPlot.Plot) class

### Plottables

* Plot types are defined in the [**`ScottPlot.Plottable`**](xref:ScottPlot.Plottable) namespace

### User Controls

* [**`ScottPlot.FormsPlot`**](xref:ScottPlot.FormsPlot) - Windows Forms
* [**`ScottPlot.WpfPlot`**](xref:ScottPlot.WpfPlot) - Windows Presentation Format
* [**`ScottPlot.AvaPlot`**](xref:ScottPlot.Avalonia.AvaPlot) - Avalonia

User controls have common public fields/methods:
* [`Plot`](xref:ScottPlot.Plot) - interact with this to manage data
* [`Configuration`](xref:ScottPlot.Control.Configuration) - interact with this to manage interactive behavior
* `Render()` - redraw the plot on the screen
* `RenderRequest()` - non-blocking request to redraw the plot