/* This color palette was sourced from the examples provided by Microcharts:
 * https://github.com/microcharts-dotnet/Microcharts/blob/main/Sources/Microcharts.Samples/Data.cs
 * At the time the license file was accessed (2021-09-02) the original work was
 * released under a MIT License, Copyright (c) 2017 Aloïs Deniel.
 */
namespace ScottPlot.Drawing.Colorsets
{
    public class Microcharts : HexColorset, IPalette
    {
        public override string[] hexColors => new string[]
        {
            "#266489", "#68B9C0", "#90D585", "#F3C151", "#F37F64",
            "#424856", "#8F97A4", "#DAC096", "#76846E", "#DABFAF",
            "#A65B69", "#97A69D",
        };
    }
}
