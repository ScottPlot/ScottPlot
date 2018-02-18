# API Ideas

## Plot Types
If we are going to throw a bunch of plots at a user control such that they are replotted when we click-drag resize, the user control needs to remember the types of plot, the data, and the order in which to redraw them. Keeping with the spirit of Matplotlib, I'll use

* **DataXY** - equal number of X/Y data points (makes line graphs and/or scatter plots)
* **Polygon** - equal number of X/Y data points which will get closed and filled
* **Signal** - evenly spaced data points with or without Yerr
* **Hline / Vline** - infinite horizontal or vertical line
* **Hspan / Vspan** - shade a range of horizontal or vertical area
* **Text** - some text to display at an X/Y data point
