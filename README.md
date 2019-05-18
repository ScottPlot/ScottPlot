# ScottPlot

**ScottPlot is an open-source interactive graphing library for .NET written in C#.** 
In a GUI environment ScottPlot makes it easily to display data interactively (left-click-drag pan, right-click-drag zoom). ScottPlot was designed to be fast enough to interactively display large datasets with millions of points (such as WAV files) at high framerates. In non-GUI environments ScottPlot can create graphs and save them as images.

ScottPlot is easy to integrate into .NET projects (including cross-platform solutions) because it has no dependencies outside the .NET framework libraries.

## Quickstart


### Windows Forms Application Quickstart
* Create a Windows Forms Application
* Add a reference to the existing project [ScottPlot.csproj](/src/ScottPlot.csproj)
* Rebuild the solution
* Drag/Drop the ScottPlotUC (from the toolbox) onto your form
* Add the following code to your startup sequence

```cs
// generate some data to plot
int pointCount = 100;
double[] dataXs = new double[pointCount];
double[] dataSin = new double[pointCount];
double[] dataCos = new double[pointCount];
for (int i = 0; i < pointCount; i++)
{
	dataXs[i] = i;
	dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
	dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
}

// plot the data
```

## Full Documentation
For many more examples review the [ScottPlot Documentation](doc) section.


## Compiled Demos
These click-to-run demos let you see what ScottPlot can do on your system
* [demonstration data navigator]()
* [real-time audio monitor]()

## About ScottPlot

**Author**\
ScottPlot was created by [Scott Harden](http://www.swharden.com) of [Harden Technologies, LLC](http://tech.swharden.com)

**Custom Features & Commissioned Modifications**\
The author of this project may be available to create customized versions of ScottPlot which incorporate requested features. Inquiries may be sent to [SWHarden@gmail.com](mailto:swharden@gmail.com).