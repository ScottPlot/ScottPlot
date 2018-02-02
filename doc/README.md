# ScottPlot Documentation
ScottPlot is under active development and evolves frequently. For now, the best way to learn how to use ScottPlot is to [review the cookbook](cookbook).

---

# Advanced Functionality

## Automatic Resizing Plots in Windows Forms
Here is the optimal strategy for automatically adjusting to window resize events. The full code for this example resides in [/src/Examples/GUI/Resizable/Form1.cs](/src/Examples/GUI/Resizable/Form1.cs).

* When Form1 loads
  * A new ScottPlot figure is created, styled, and decorated
  * Demo data is generated, and the axis is resized to fit the data
* Create a redraw function
  * Resize the ScottPlot figure based on picturebox1 dimensions
  * Redraw the frame (axis tick marks)
  * Redraw the plots
  * Render the bitmap and apply it to the picturebox
* Connect events with the redraw function
  * Form1_Shown resizes and redraws the graph when the form first loads
  * Form1_Resize resizes and redraws the graph when the window is resized

***To display ANIMATED graphs*** create a timer and every time it ticks update the `Xs` and/or `Ys` arrays then call the redraw function.

```C#
Figure fig;
double[] Xs, Ys;

public Form1()
{
    InitializeComponent();

    // create the figure and apply styling
    fig = new Figure(pictureBox1.Width, pictureBox1.Height);
    fig.styleForm();
    fig.title = "Awesome Data";
    fig.yLabel = "Random Walk";
    fig.xLabel = "Sample Number";

    // synthesize data
    int pointCount = 123;
    Xs = fig.gen.Sequence(pointCount);
    Ys = fig.gen.RandomWalk(pointCount);
    fig.ResizeToData(Xs, Ys, .9, .9);
}

public void ResizeAndRedraw()
{
    if (fig == null) return;
    fig.Resize(pictureBox1.Width, pictureBox1.Height);
    fig.RedrawFrame();
    fig.PlotLines(Xs, Ys, 1, Color.Red);
    fig.PlotScatter(Xs, Ys, 5, Color.Blue);
    pictureBox1.Image = fig.Render();
}

private void Form1_Shown(object sender, EventArgs e) {ResizeAndRedraw();}
private void Form1_Resize(object sender, EventArgs e) {ResizeAndRedraw();}
```

![](screenshots/stretchy.gif)
