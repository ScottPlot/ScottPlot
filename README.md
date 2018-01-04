# ScottPlot
***ScottPlot is an open-source interactive graphing library for .NET written in C#.*** The core of this project is a dependency-free portable class which takes 2D arrays and graphs them (it can even create JPEG output in console applications). Interactive abilities are made available with a user control which is easy to drop into a windows forms application. ScottPlot was loosely inspired by matplotlib for Python

***This project is extremely early in the development cycle.*** Ultimately I want to distribute ScottPlot as a stable API (at which time its use will be documented with confidence and permanence). For now, this project should only be assessed for its experimental and educational value.

Description | Screenshot
--- | ---
When a _reference_ is added to the ScottPlot folder, a ScottPlotUC (user control) appears in the toolbox. Drag/drop it onto a Windows Forms Application, then you can assign data to it with `scottPlotUC1.SetData(Xs, Ys)` (where Xs and Ys are double arrays of the same length). | ![](/doc/uc-usage.png)
That's it! The output already has axis labels, grid lines, and is interactive (mouse click-drag pans, right-click-drag zooms, and mouse-scroll-wheel zooms). | ![](/doc/uc-output.png)

### Example Code
This is the actual code behind button1 in the above example. This code fills the graph area with a sine wave plus some noise.

```C#
private void button1_Click(object sender, EventArgs e)
{
	// create some data to plot
	int nPoints = 1000;
	double[] Xs = ScottPlot.Generate.Sequence(nPoints);
	double[] Ys = ScottPlot.Generate.Sine(nPoints, 5);
	Ys = ScottPlot.Generate.Salt(Ys, .2);

	// load the data into the ScottPlot User Control
	scottPlotUC1.SetData(Xs, Ys);
}
```

### Example Applications

Description | Screenshot
--- | ---
**ABF Browser:** Inspired by [my recent efforts toward reading ABF files](https://github.com/swharden/pyABF), I have included an ABF file browser. It is surprisingly fast and responsive (especially when compared to commercial softwared aimed at graphing the same data). I am not actively developing or supporting code to read ABF files at this time. | ![](/doc/abf-browser.jpg)
