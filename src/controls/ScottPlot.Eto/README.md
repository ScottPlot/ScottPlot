**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets. This package provides an Eto control for interactive manipulation of ScottPlot plots.

## Eto Quickstart

Use NuGet to install [`ScottPlot.Eto`](https://www.nuget.org/packages/ScottPlot.Eto), then add a `ScottPlot.Eto.PlotView` Control to your Form or Container by using the following example:  
```cs
   var eto_platform = Eto.Platform.Instance.ToString();
   var os_platform = System.Environment.OSVersion.ToString();
   this.Title = $"My ScottPlot Form - {eto_platform} - {os_platform}";

   var plotView = new ScottPlot.Eto.PlotView();

   double[] xs = new double[] { 1, 2, 3, 4, 5 };
   double[] ys = new double[] { 1, 4, 9, 16, 25 };
   plotView.Plot.AddScatter(xs, ys);

   this.Content = plotView;
```

![](../../../dev/graphics/eto-quickstart-wpf.png)
![](../../../dev/graphics/eto-quickstart-gtk.png)
![](../../../dev/graphics/eto-quickstart-osx.png)

## Notes for Linux and macOS builds
ScottPlot uses `System.Drawing.Common` which is the graphics library that ships as part of .NET Core.
On macOS and Linux, it uses [libgdiplus](https://github.com/mono/libgdiplus) as its back-end and this must be installed on the host machine.

You can acquire libgdiplus:
- For macOS, using the [mono-libgdiplus](https://formulae.brew.sh/formula/mono-libgdiplus)
Homebrew package
- For Linux, via the [libgdiplus](https://pkgs.org/download/libgdiplus) package.
 
It is also necessary that your project [explicitly include](https://github.com/ScottPlot/ScottPlot/issues/1220#issuecomment-904233580) a reference to [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common) :
```xml
  <ItemGroup>
    <PackageReference Include="System.Drawing.Common" Version="5.0.3" />
  </ItemGroup>
```

## ScottPlot Cookbook

The [**ScottPlot Cookbook**](https://swharden.com/scottplot/cookbook) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code.

[![](https://raw.githubusercontent.com/ScottPlot/ScottPlot/master/dev/graphics/cookbook.jpg)](https://swharden.com/scottplot/cookbook)