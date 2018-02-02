using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using ScottPlot;

namespace ConsoleApp1
{
    class Cookbook
    {
        public static Random rand = new Random();
        
        /// <summary>
        /// Create a plot of data from two arrays of doubles (Xs and Ys)
        /// </summary>
        public static void demo_001()
        {
            // create a new ScottPlot figure
            Figure fig = new Figure(640, 480);
            fig.title = "Super Special Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // generate data
            int pointCount = 123;
            double[] Xs = fig.gen.Sequence(pointCount);
            double[] Ys = fig.gen.RandomWalk(pointCount);

            // adjust the axis to fit the data (then zoom out slightly)
            fig.ResizeToData(Xs, Ys, .9, .9);

            // make the plot
            fig.BenchmarkThis();
            fig.PlotLines(Xs, Ys, 1, Color.Red);
            fig.PlotScatter(Xs, Ys, 5, Color.Blue);

            // save the file
            fig.Save("output/demo_001.png");
        }


        /// <summary>
        /// Zooming
        /// </summary>
        public static void demo_002()
        {
            Figure fig = new Figure(640, 480);
            fig.title = "Super Special Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            double[] Xs = fig.gen.Sequence(123);
            double[] Ys = fig.gen.RandomWalk(123);

            fig.ResizeToData(Xs, Ys, null, null); // fit data precisely            
            fig.Zoom(2, .5);  // now zoom in horizontally and out vertically

            fig.PlotLines(Xs, Ys, 1, Color.Red);
            fig.PlotScatter(Xs, Ys, 5, Color.Blue);

            fig.Save("output/demo_002.png");
        }


        /// <summary>
        /// Changing colors
        /// </summary>
        public static void demo_003()
        {
            Figure fig = new Figure(640, 480);
            fig.title = "Super Special Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // go to town changing colors
            fig.colorAxis = Color.Yellow;
            fig.colorBg = Color.FromArgb(255,30,30,30);
            fig.colorGrid = Color.FromArgb(255, 55, 55, 55);
            fig.colorGraph = Color.FromArgb(255, 40, 40, 40);

            double[] Xs = fig.gen.Sequence(123);
            double[] Ys = fig.gen.RandomWalk(123);
            fig.ResizeToData(Xs, Ys, .9, .9);

            fig.BenchmarkThis();
            fig.PlotLines(Xs, Ys, 1, Color.Gray);
            fig.PlotScatter(Xs, Ys, 5, Color.White);

            fig.Save("output/demo_003.png");
        }

        /// <summary>
        /// Overlapping plots of different sizes and colors
        /// </summary>
        public static void demo_004()
        {
            Figure fig = new Figure(640, 480);
            fig.title = "Super Special Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // use the same Xs every time
            double[] Xs = fig.gen.Sequence(123);

            // manually define axis
            fig.Axis(-5, 130, -10, 10); 

            fig.BenchmarkThis();
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 1, Color.Red);
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 2, Color.Orange);
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 3, Color.Yellow);
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 4, Color.Green);
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 5, Color.Blue);
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 6, Color.Indigo);
            fig.PlotLines(Xs, fig.gen.RandomWalk(123), 7, Color.Violet);

            fig.Save("output/demo_004.png");
        }


        /// <summary>
        /// Demonstrate transparency
        /// </summary>
        public static void demo_005()
        {
            Figure fig = new Figure(640, 480);
            fig.title = "Super Special Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // use the same Xs every time
            double[] Xs = fig.gen.Sequence(123);

            // manually define axis
            fig.Axis(-5, 130, -10, 10);

            // plot lines with different colors
            Color[] colors = new Color[] { Color.FromArgb(100, 255, 0, 0),  // red
                                           Color.FromArgb(100, 0, 150, 0),  // green
                                           Color.FromArgb(100, 0, 0, 255)}; // blue

            fig.BenchmarkThis();
            for (int i=0; i<colors.Length; i++) // for each color
            {
                for (int j=0; j<3; j++) // draw 3 lines
                {
                    fig.PlotLines(Xs, fig.gen.RandomWalk(123), 5, colors[i]);
                }
            }

            fig.Save("output/demo_005.png");
        }


        /// <summary>
        /// Scatter plot features
        /// </summary>
        public static void demo_006()
        {
            // create a new ScottPlot figure
            Figure fig = new Figure(640, 480);
            fig.title = "Super Special Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // generate data
            int pointCount = 40;
            double[] Xs = fig.gen.Sequence(pointCount);

            // manually define axis
            fig.Axis(-3, 43, -2, 4);

            // make the plot
            fig.BenchmarkThis();
            fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 2, Color.Black);
            fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 5, Color.Red);
            fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 10, Color.Green);
            fig.PlotScatter(Xs, fig.gen.RandomWalk(pointCount), 20, Color.FromArgb(100,0,0,255));

            // save the file
            fig.Save("output/demo_006.png");
        }

        /// <summary>
        /// THIS CODE BLOCK IS LEFT HERE FOR PARSING PURPOSES
        /// </summary>

        /*
         * 
         * 
         *  THESE EXAMPLES ARE VERY COMPLEX AND/OR LOW LEVEL
         *  
         *  
         */

        /// <summary>
        /// Draw by directly interacting with the graphics object and position/pixel conversion
        /// </summary>
        public static void demo_101()
        {
            // create a new ScottPlot figure
            Figure fig = new Figure(640, 480);
            fig.title = "Direct Graphics Drawing";
            fig.yLabel = "Pure Awesomeness";
            fig.xLabel = "Relative Time (years)";
            fig.Axis(-15, 35, -10, 110); // x1, x2, y1, y2

            // draw a line directly on the Graphics object in AXIS units
            Point pt1 = new Point(fig.xAxis.UnitToPx(0), fig.yAxis.UnitToPx(13));
            Point pt2 = new Point(fig.xAxis.UnitToPx(32), fig.yAxis.UnitToPx(98));
            fig.gfxGraph.DrawLine(new Pen(new SolidBrush(Color.Blue), 5), pt1, pt2);

            // save the file
            fig.Save("output/demo_101.png");
        }

        /// <summary>
        /// THIS CODE BLOCK IS LEFT HERE FOR PARSING PURPOSES
        /// </summary>

    }
}
