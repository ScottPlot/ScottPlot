using System;
using System.Collections.Generic;

namespace SharedTests;

internal class SharedPaletteTests
{
    [Test]
    public void Test_SharedPalette_GetPalettes()
    {
        ScottPlot.SharedPalette.GetPalettes().Should().NotBeNullOrEmpty();
    }

    [Test]
    public void Test_PalleteTitle_ShouldBePopulated()
    {
        foreach (var palette in ScottPlot.SharedPalette.GetPalettes())
        {
            if (string.IsNullOrEmpty(palette.Name))
                throw new InvalidOperationException($"Palette has invalid title: {palette}");
        }
    }

    [Test]
    public void Test_Palletes_ShouldHaveUniqueTitles()
    {
        HashSet<string> titles = new();
        foreach (var palette in ScottPlot.SharedPalette.GetPalettes())
        {
            if (titles.Contains(palette.Name))
                throw new InvalidOperationException($"duplicate Palette title: {palette.Name}");

            titles.Add(palette.Name);
        }
    }
}
