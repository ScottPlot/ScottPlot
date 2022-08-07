/* This color palette was sourced from the examples provided by Microcharts:
 * https://github.com/microcharts-dotnet/Microcharts/blob/main/Sources/Microcharts.Samples/Data.cs
 * At the time the license file was accessed (2021-09-02) the original work was
 * released under a MIT License, Copyright (c) 2017 Aloïs Deniel.
 */
namespace ScottPlot.Drawing.Colorsets
{
    public class Microcharts : HexColorset, IPalette
    {
        public override string[] hexColors => Common.HexPalettes.Microcharts.Colors;
    }
}
