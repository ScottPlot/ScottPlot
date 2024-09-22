using ScottPlot.DataSources;

namespace ScottPlotTests.UnitTests;

internal class IDataSourceTests
{
    // Data that tests should use when creating the IDataSource

    #region < Setup >

    public static readonly double[] Xs = Generate.Consecutive(51, Period, 0);
    public static readonly double[] Ys = Generate.Sin(51);
    public static readonly Coordinates[] Cs = Xs.Zip(Ys).Select(pair => new Coordinates(pair.First, pair.Second)).ToArray();
    
    public static readonly int[] GenericXs = Xs.Select(i => (int)i).ToArray();
    public static readonly int[] GenericYs = Ys.Select(i => (int)i).ToArray();

    public const int PlotWidth = 600;
    public const int PlotHeight = 400;

    public const double Period = 1;
    public const int IndexToCheck = 20;
    public static readonly int Length = Xs.Length;
    public static readonly double XOffSet = (double)Random.Shared.Next(1, 50);
    public static readonly double YOffSet = (double)Random.Shared.Next(51, 100);
    public static readonly double XScaling = 3;
    public static readonly double YScaling = 3;

    public static readonly Coordinates MouseLocation = new(Period * IndexToCheck, Ys[IndexToCheck]);
    public static readonly Coordinates ScaledMouseLocation = DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet);

    private static double GetValidScaling()
    {
        double i = Random.Shared.NextDouble();
        while (i is 0d or 1d) 
        {
            i = Random.Shared.NextDouble();
        }
        return i * 3;
    }

    private static Plot GetPlot()
    {
        Plot plot = new Plot();
        plot.Axes.Top.IsVisible = false;
        plot.Axes.Right.IsVisible = false;
        plot.Axes.Bottom.IsVisible = false;
        plot.Axes.Left.IsVisible = false;
        plot.HideLegend();
        return plot;
    }

    #endregion

    #region < GetNearest | GetNearestFast >

    [Test]
    public void Test_IsAscending()
    {
        Assert.That(Xs.IsAscending(null), Is.True);
        Assert.That(Ys.IsAscending(null), Is.False);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearest(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);
        
        var nearest = DataSourceUtilities.GetNearest(dataSource, ignoreOffsetsAndScaling ? MouseLocation : ScaledMouseLocation, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(Xs[IndexToCheck]);
        nearest.Y.Should().BeApproximately(Ys[IndexToCheck], .001);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearestFast(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling 
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        var signal = plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);

        var nearest = DataSourceUtilities.GetNearestFast(dataSource, ignoreOffsetsAndScaling ? MouseLocation : ScaledMouseLocation, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(Xs[IndexToCheck]);
        nearest.Y.Should().BeApproximately(Ys[IndexToCheck], .001);

        //Native function should also be using this method
        nearest = signal.GetNearest(ignoreOffsetsAndScaling ? MouseLocation : ScaledMouseLocation, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(Xs[IndexToCheck]);
        nearest.Y.Should().BeApproximately(Ys[IndexToCheck], .001);
    }

    #endregion

    #region #region < GetNearestX | GetNearestXFast >

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearestX(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);
        
        var nearest = DataSourceUtilities.GetNearestX(dataSource, ignoreOffsetsAndScaling ? MouseLocation : ScaledMouseLocation, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(Xs[IndexToCheck]);
        nearest.Y.Should().BeApproximately(Ys[IndexToCheck], .001);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearestXFast(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        var signal = plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);
        
        var nearest = DataSourceUtilities.GetNearestXFast(dataSource, ignoreOffsetsAndScaling ? MouseLocation : ScaledMouseLocation, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(Xs[IndexToCheck]);
        nearest.Y.Should().BeApproximately(Ys[IndexToCheck], .001);

        //Native function should also be using this method
        nearest = signal.GetNearestX(ignoreOffsetsAndScaling ? MouseLocation : ScaledMouseLocation, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(Xs[IndexToCheck]);
        nearest.Y.Should().BeApproximately(Ys[IndexToCheck], .001);
    }

    #endregion

    [Test]
    public void Test_CoordinateDataSource()
    {
        // Test IDataSource
        var ds = new ScottPlot.DataSources.CoordinateDataSource(Cs);
        Test_IDataSource(ds);
    }

    [Test] public void Test_SignalSourceDouble() => TestSignal(new ScottPlot.DataSources.SignalSourceDouble(Ys, Period));
    [Test] public void Test_SignalSourceGenericArray() => TestSignal(new ScottPlot.DataSources.SignalSourceGenericArray<int>(GenericYs, Period));
    [Test] public void Test_SignalSourceGenericList() => TestSignal(new ScottPlot.DataSources.SignalSourceGenericList<int>(GenericYs, Period));
    [Test] public void Test_FastSignalSourceDouble() => TestSignal(new ScottPlot.DataSources.FastSignalSourceDouble(Ys, Period));

    private static void TestSignal<T>(T signalSource) where T : ISignalSource, IDataSource
    {
        signalSource.XOffset = XOffSet;
        signalSource.YOffset = YOffSet;
        signalSource.YScale = YScaling;
        Console.Write($" - Testing {signalSource.GetType().Name} as {nameof(IDataSource)} ");
        Test_IDataSource(signalSource);
        Console.WriteLine($"- Passed");

        var plot = GetPlot();

        var signal = plot.Add.Signal(signalSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);

        if (signalSource is IGetNearest gn)
        {
            Console.Write($" - Testing {signalSource.GetType().Name} as {nameof(IGetNearest)} ");
            Test_IGetNearest(gn, plot);
            Console.WriteLine($"- Passed");
        }

        if (signal is IDataSource ds)
        {
            Console.Write($" - Testing {signal.GetType().Name} as {nameof(IDataSource)} ");
            Test_IDataSource(ds);
            Console.WriteLine($"- Passed");
        }

        Console.Write($" - Testing Signal as {nameof(IGetNearest)}");
        Test_IGetNearest(signal, plot);
        Console.WriteLine($"- Passed");
    }

    [Test] public void Test_SignalXYSourceDoubleArray() => TestSignalXY(new ScottPlot.DataSources.SignalXYSourceDoubleArray(Xs, Ys));
    [Test] public void Test_SignalXYSourceGenericArray() => TestSignalXY(new ScottPlot.DataSources.SignalXYSourceGenericArray<int, int>(GenericXs, GenericYs));

    private static void TestSignalXY<T>(T signalXYSource) where T: ISignalXYSource, IDataSource
    {
        signalXYSource.XOffset = XOffSet;
        signalXYSource.YOffset = YOffSet;
        signalXYSource.XScale = XScaling;
        signalXYSource.YScale = YScaling;
        Console.Write($" - Testing {signalXYSource.GetType().Name} as {nameof(IDataSource)} ");
        Test_IDataSource(signalXYSource);
        Console.WriteLine($"- Passed");

        var plot = GetPlot();

        var signal = plot.Add.SignalXY(signalXYSource);

        plot.RenderInMemory(PlotWidth, PlotHeight);

        if (signalXYSource is IGetNearest gn)
        {
            Console.Write($" - Testing {signalXYSource.GetType().Name} as {nameof(IGetNearest)} ");
            Test_IGetNearest(gn, plot);
            Console.WriteLine($"- Passed");
        }

        if (signal is IDataSource ds)
        {
            Console.Write($" - Testing {signal.GetType().Name} as {nameof(IDataSource)} ");
            Test_IDataSource(ds);
            Console.WriteLine($"- Passed");
        }

        Console.Write($" - Testing SignalXY as {nameof(IGetNearest)}");
        Test_IGetNearest(signal, plot);
        Console.WriteLine($"- Passed");
    }

    [Test] public void Test_ScatterSourceDoubleArray() => TestScatter(new ScottPlot.DataSources.ScatterSourceDoubleArray(Xs, Ys));
    [Test] public void Test_ScatterSourceGenericArray() => TestScatter(new ScottPlot.DataSources.ScatterSourceGenericArray<int, int>(GenericXs, GenericYs));
    [Test] public void Test_ScatterSourceGenericList() => TestScatter(new ScottPlot.DataSources.ScatterSourceGenericList<int, int>(GenericXs.ToList(), GenericYs.ToList()));
    [Test] public void Test_ScatterSourceCoordinatesArray() => TestScatter(new ScottPlot.DataSources.ScatterSourceCoordinatesArray(Cs));
    [Test] public void Test_ScatterSourceCoordinatesList() => TestScatter(new ScottPlot.DataSources.ScatterSourceCoordinatesList(Cs.ToList()));

    private static void TestScatter<T>(T scatterSource) where T : IScatterSource, IDataSource
    {
        Console.Write($" - Testing {scatterSource.GetType().Name} as {nameof(IDataSource)} ");
        Test_IDataSource(scatterSource);
        Console.WriteLine($"- Passed");

        var plot = GetPlot();

        var scatter = plot.Add.Scatter(scatterSource);
        scatter.OffsetX = XOffSet;
        scatter.OffsetY = YOffSet;
        scatter.ScaleX = XScaling;
        scatter.ScaleY = YScaling;
        

        Console.Write($" - Testing Scatter as {nameof(IDataSource)}");
        Test_IDataSource(scatter);
        Console.WriteLine($"- Passed");

        plot.RenderInMemory(PlotWidth, PlotHeight);

        if (scatterSource is IGetNearest gn)
        {
            Console.Write($" - Testing {scatterSource.GetType().Name} as {nameof(IGetNearest)} ");
            Test_IGetNearest(gn, plot);
            Console.WriteLine($"- Passed");
        }

        Console.Write($" - Testing Scatter as {nameof(IGetNearest)}");
        Test_IGetNearest(scatter, plot);
        Console.WriteLine($"- Passed");
    }

    private static void Test_IDataSource(IDataSource dataSource)
    {
        Assert.That(dataSource, Is.Not.Null);
        
        // If using the supplied dataset, all are sorted!
        Assert.That(dataSource.IsSorted, Is.True);

        // Get Coordinate
        var coordinate = dataSource.GetCoordinate(IndexToCheck);
        Assert.That(coordinate.X, Is.EqualTo(Cs[IndexToCheck].X), $"{nameof(IDataSource)}.{nameof(IDataSource.GetCoordinate)} returned a different X result than coordinate at same index");
        Assert.That(coordinate.Y, Is.EqualTo(Cs[IndexToCheck].Y), $"{nameof(IDataSource)}.{nameof(IDataSource.GetCoordinate)} returned a different Y result than coordinate at same index");

        // Get XY
        Assert.That(dataSource.GetX(IndexToCheck), Is.EqualTo(coordinate.X), $"{nameof(IDataSource)}.{nameof(IDataSource.GetX)} returned a different result than coordinate at same index");
        Assert.That(dataSource.GetY(IndexToCheck), Is.EqualTo(coordinate.Y), $"{nameof(IDataSource)}.{nameof(IDataSource.GetY)} returned a different result than coordinate at same index");

        // GetXY Scaled
        var scaledCoordinate = dataSource.GetCoordinateScaled(IndexToCheck); // cannot check that scaling occurred, only that all values return same result
        Assert.That( dataSource.GetXScaled(IndexToCheck), Is.EqualTo(scaledCoordinate.X), $"{nameof(IDataSource)}.{nameof(IDataSource.GetXScaled)} returned a different result than the scaled coordinate at same index");
        Assert.That(dataSource.GetYScaled(IndexToCheck), Is.EqualTo(scaledCoordinate.Y), $"{nameof(IDataSource)}.{nameof(IDataSource.GetYScaled)} returned a different result than scaled coordinate at same index");

        // GetClosestIndex
        Coordinates mouseLocation = new Coordinates(Xs[IndexToCheck], Ys[IndexToCheck]); // Use same XY here to validate any Rotated items
        Assert.That(dataSource.GetXClosestIndex(mouseLocation), Is.EqualTo(IndexToCheck), $"{nameof(IDataSource)}.{nameof(IDataSource.GetXClosestIndex)} returned an unexpected value");
    }

    private static void Test_IGetNearest(IGetNearest dataSource, Plot plot)
    {
        // TO-DO - Figure out correct mouse coordinates.

        return;
        Assert.That(dataSource, Is.Not.Null);

        float maxDistance = 10;
        float mouseShift = maxDistance * 0.7f;
        var dataPoint = new DataPoint(Cs[IndexToCheck], IndexToCheck);

        // X and Y are slightly apart, but should result in same X getting selected, even when rotated
        var mouseLocation = new Coordinates((dataPoint.X - mouseShift) * plot.LastRender.PxPerUnitX, (dataPoint.Y + (maxDistance - mouseShift)) * plot.LastRender.PxPerUnitY);

        var nearestX = dataSource.GetNearestX(mouseLocation, plot.LastRender, maxDistance);
        Assert.That(nearestX.Index, Is.EqualTo(IndexToCheck), $"{dataSource.GetType().Name} failed to get correct index using {nameof(IGetNearest)}.{nameof(IGetNearest.GetNearestX)}");

        var nearest = dataSource.GetNearest(mouseLocation, plot.LastRender, maxDistance);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck), $"{dataSource.GetType().Name} failed to get correct index using {nameof(IGetNearest)}.{nameof(IGetNearest.GetNearest)}");
        

        Assert.That(nearest.X, Is.EqualTo(nearestX.X), $"nearestX.X != nearest.X");
        Assert.That(nearest.Y, Is.EqualTo(nearestX.Y), $"nearestX.Y != nearest.Y");
        Assert.That(nearest.Index, Is.EqualTo(nearestX.Index), $"nearestX.Index != nearest.Index");
    }
}

