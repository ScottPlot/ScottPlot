namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class DataGen : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Sample Data";
    public string CategoryDescription => "ScottPlot has many " +
        "built-in utilities for generating sample data.";

    public class MultipleSineWaves : RecipeBase
    {
        public override string Name => "Multiple Sine Waves";
        public override string Description => "This recipe demonstrates creation of a noisy " +
            "waveform containing multiple sine waves with different frequencies.";

        [Test]
        public override void Execute()
        {
            double[] values = Generate.RandomNormal(500, stdDev: 0.2);

            for (int i = 1; i < 10; i++)
            {
                var sig = myPlot.Add.Signal(values);
                sig.Data.YOffset = i * 3;
                sig.LineWidth = 1.5f;
                values = Generate.AddSin(values, oscillations: i);
            }
        }
    }
}
