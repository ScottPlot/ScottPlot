# Highlight Point On Hover
After thinking about it a lot, I decided not to include this into the ScottPlot user control itself, but I added an `OnMouseMoved` event you can attach to and include this functionality yourself in any application. 

I made a sample application to demonstrate how to do this type of mouse tracking. It highlights points closest to the mouse and displays their value. The source code [Form1.cs](https://github.com/swharden/ScottPlot/blob/master/demos/ScottPlotShowValueOnHover/Form1.cs) is big, but it isn't too crazy, and can be customized to do whatever your application needs to do.

### Demo Program
* [source code](https://github.com/swharden/ScottPlot/tree/master/demos/ScottPlotShowValueOnHover)
* [compiled click-to-run demo](https://github.com/swharden/ScottPlot/tree/master/demos/ScottPlotShowValueOnHover/compiled) (ZIP)

### Screenshot
![](screenshot.png)

### Theory

The program first makes a plot and adds an extra scatter point and text which will later get moved around to represent the hover-over point

https://github.com/swharden/ScottPlot/blob/7a8043838b3f5b896e2435dd66cb266476163f1b/demos/ScottPlotShowValueOnHover/Form1.cs#L20-L30

The `OnMouseMoved` event is then attached to so it can call an `Update()` function

https://github.com/swharden/ScottPlot/blob/7a8043838b3f5b896e2435dd66cb266476163f1b/demos/ScottPlotShowValueOnHover/Form1.cs#L90-L93

In the `Update()` function first re-attach to the 3 plot objects relevant to this project by pulling them back out of the ScottPlot UC
https://github.com/swharden/ScottPlot/blob/7a8043838b3f5b896e2435dd66cb266476163f1b/demos/ScottPlotShowValueOnHover/Form1.cs#L34-L37

The distance between the cursor and _every point on the scatter plot_ is calculated. The single point with the shortest distance is remembered. This can get slow if you have thousands of points.
https://github.com/swharden/ScottPlot/blob/7a8043838b3f5b896e2435dd66cb266476163f1b/demos/ScottPlotShowValueOnHover/Form1.cs#L50-L66

Finally set the position and visibility of the marker and text based on what we found:
https://github.com/swharden/ScottPlot/blob/7a8043838b3f5b896e2435dd66cb266476163f1b/demos/ScottPlotShowValueOnHover/Form1.cs#L68-L87
