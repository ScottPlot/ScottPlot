using ScottPlot.Plottable;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class RepeatingAxisLine : IRecipe
    {
        public string Category => "Plottable: Repeating Axis Line";
        public string ID => "repeatingAxisLine_basics";
        public string Title => "Repeating Axis Line";
        public string Description =>
            "Repeating axis lines allows to plot several axis lines, either horizontal or vertical, draggable or not, whose positions are linked";

        public void ExecuteRecipe(Plot plt)
        {
            //Generate signal with 3 harmonics
            int samples = 500;
            var signal_1 = ScottPlot.DataGen.Sin(samples, 10);
            var signal_2 = ScottPlot.DataGen.Sin(samples, 20);
            var signal_3 = ScottPlot.DataGen.Sin(samples, 30);
            var signal = new double[samples];
            for (int index = 0; index < samples; index++)
            {
                signal[index] = signal_1[index] + signal_2[index] + signal_3[index];
            }

            // Plot the signal
            plt.AddSignal(signal);

            // Create a draggable RepeatingVLine with 5 lines spaced evenly by 50 X units, starting at position 0
            // The first line will be thicker than the others
            RepeatingVLine myPlottableX = new();
            myPlottableX.DragEnabled = true;
            myPlottableX.count = 5;
            myPlottableX.shift = 50;
            myPlottableX.Color = System.Drawing.Color.Red;
            myPlottableX.PositionLabel = true;
            myPlottableX.PositionLabelBackground = myPlottableX.Color;
            myPlottableX.relativeposition = false;
            plt.Add(myPlottableX);

            // Create a draggable RepeatingVLine with 5 lines spaced evenly by 50 X units, starting at position 0, with a -4 offset
            // The first line will be thicker than the others
            RepeatingVLine myPlottableX2 = new();
            myPlottableX2.DragEnabled = true;
            myPlottableX2.count = 3;
            myPlottableX2.shift = 50;
            myPlottableX2.offset = -1;
            myPlottableX2.Color = System.Drawing.Color.DarkGreen;
            myPlottableX2.PositionLabel = true;
            myPlottableX2.PositionLabelBackground = myPlottableX2.Color;
            myPlottableX2.relativeposition = false;
            plt.Add(myPlottableX2);

        }
    }

}
