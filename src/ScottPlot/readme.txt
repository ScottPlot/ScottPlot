
### ScottPlot ###

	Project page:
	https://github.com/swharden/ScottPlot

	Documentation:
	https://github.com/swharden/ScottPlot/tree/master/doc

	Cookbook:
	https://github.com/swharden/ScottPlot/tree/master/doc/cookbook


### Quickstart: Windows Forms ###

	1) Drag/Drop ScottPlotUC (from the toolbox) onto your form
	
	2) Add this code to your startup sequence:

	double[] xs = new double[] {1, 2, 3, 4, 5};
	double[] ys = new double[] {1, 4, 9, 16, 25};
	scottPlotUC1.plt.PlotScatter(xs, ys);
	scottPlotUC1.plt.AxisAuto();
	scottPlotUC1.Render();
		

### Quickstart: Console Application ###
	
	double[] xs = new double[] { 1, 2, 3, 4, 5 };
	double[] ys = new double[] { 1, 4, 9, 16, 25 };
	var plt = new ScottPlot.Plot(600, 400);
	plt.PlotScatter(xs, ys);
	plt.AxisAuto();
	plt.SaveFig("demo.png");