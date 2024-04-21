using FluentAssertions;

namespace ScottPlotTests.UnitTests;

internal class AxisRuleTests
{
    [Test]
    public void Test_AxisRule_LockedBottom()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedBottom(plt.Axes.Left, -123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Bottom.Should().Be(AxisLimits.Unset.Bottom);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);
    }

    [Test]
    public void Test_AxisRule_LockedCenterX()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedCenterX(plt.Axes.Bottom, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Bottom.Should().Be(AxisLimits.Unset.Bottom);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.HorizontalCenter.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.HorizontalCenter.Should().Be(123);
    }

    [Test]
    public void Test_AxisRule_LockedCenterY()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedCenterY(plt.Axes.Left, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Left.Should().Be(AxisLimits.Unset.Left);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.VerticalCenter.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.VerticalCenter.Should().Be(123);
    }

    [Test]
    public void Test_AxisRule_LockedHorizontal()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedHorizontal(plt.Axes.Bottom, -123, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Left.Should().Be(AxisLimits.Unset.Left);
        plt.Axes.GetLimits().Right.Should().Be(AxisLimits.Unset.Right);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);
        plt.Axes.GetLimits().Right.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);
        plt.Axes.GetLimits().Right.Should().Be(123);
    }

    [Test]
    public void Test_AxisRule_LockedLeft()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedLeft(plt.Axes.Bottom, -123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Left.Should().Be(AxisLimits.Unset.Left);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);
    }

    [Test]
    public void Test_AxisRule_LockedRight()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedRight(plt.Axes.Bottom, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Right.Should().Be(AxisLimits.Unset.Right);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Right.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Right.Should().Be(123);
    }

    [Test]
    public void Test_AxisRule_LockedTop()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedTop(plt.Axes.Left, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Top.Should().Be(AxisLimits.Unset.Top);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Top.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Top.Should().Be(123);
    }

    [Test]
    public void Test_AxisRule_LockedVertical()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.LockedVertical(plt.Axes.Left, -123, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Bottom.Should().Be(AxisLimits.Unset.Bottom);
        plt.Axes.GetLimits().Top.Should().Be(AxisLimits.Unset.Top);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);
        plt.Axes.GetLimits().Top.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);
        plt.Axes.GetLimits().Top.Should().Be(123);
    }

    [Test]
    public void Test_AxisRule_MaximumBoundary()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.MaximumBoundary(
            plt.Axes.Bottom, plt.Axes.Left,
            new AxisLimits(-123, 123, -456, 456)));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-9999, 9999, -9999, 9999);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-123);
            plt.Axes.GetLimits().Right.Should().Be(123);
            plt.Axes.GetLimits().Bottom.Should().Be(-456);
            plt.Axes.GetLimits().Top.Should().Be(456);
        }
    }

    [Test]
    public void Test_AxisRule_MinimumBoundary()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.MinimumBoundary(
            plt.Axes.Bottom, plt.Axes.Left,
            new AxisLimits(-1, 1, -2, 2)));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-.5, .5, -.5, .5);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-1);
            plt.Axes.GetLimits().Right.Should().Be(1);
            plt.Axes.GetLimits().Bottom.Should().Be(-2);
            plt.Axes.GetLimits().Top.Should().Be(2);
        }
    }

    [Test]
    public void Test_AxisRule_MaximumSpan()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.MaximumSpan(plt.Axes.Bottom, plt.Axes.Left, 1, 2));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-5, 5, -5, 5);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-.5);
            plt.Axes.GetLimits().Right.Should().Be(.5);
            plt.Axes.GetLimits().Bottom.Should().Be(-1);
            plt.Axes.GetLimits().Top.Should().Be(1);
        }
    }

    [Test]
    public void Test_AxisRule_MinimumSpan()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.MinimumSpan(plt.Axes.Bottom, plt.Axes.Left, 1, 2));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-.05, .05, -.05, .05);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-.5);
            plt.Axes.GetLimits().Right.Should().Be(.5);
            plt.Axes.GetLimits().Bottom.Should().Be(-1);
            plt.Axes.GetLimits().Top.Should().Be(1);
        }
    }

    [Test]
    public void Test_AxisRule_SquarePreserveX()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.SquarePreserveX(plt.Axes.Bottom, plt.Axes.Left));

        for (int i = 0; i < 3; i++)
        {
            plt.RenderInMemory();
            plt.RenderManager.LastRender.UnitsPerPxX.Should().BeApproximately(plt.RenderManager.LastRender.UnitsPerPxY, 1e-6);
        }
    }

    [Test]
    public void Test_AxisRule_SquarePreserveY()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.SquarePreserveY(plt.Axes.Bottom, plt.Axes.Left));

        for (int i = 0; i < 3; i++)
        {
            plt.RenderInMemory();
            plt.RenderManager.LastRender.UnitsPerPxX.Should().BeApproximately(plt.RenderManager.LastRender.UnitsPerPxY, 1e-6);
        }
    }

    [Test]
    public void Test_AxisRule_SquareZoom()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.SquareZoomOut(plt.Axes.Bottom, plt.Axes.Left));

        // NOTE: ticks are not always stable across successive render requests!

        for (int i = 0; i < 3; i++)
        {
            plt.RenderInMemory();
            plt.RenderManager.LastRender.UnitsPerPxX.Should().BeApproximately(plt.RenderManager.LastRender.UnitsPerPxY, 1e-6);
        }
    }

    [Test]
    public void Test_AxisRule_SnapTicksX()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.SnapToTicksX(plt.Axes.Bottom));

        for (int i = 0; i < 3; i++)
        {
            // WARNING: CANNOT TEST TICK SNAPPING BECAUSE TICKS ARE FONT AND SYSTEM DEPENDENT
            plt.Should().RenderInMemoryWithoutThrowing();
        }
    }

    [Test]
    public void Test_AxisRule_SnapTicksY()
    {
        ScottPlot.Plot plt = new();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new ScottPlot.AxisRules.SnapToTicksY(plt.Axes.Left));

        for (int i = 0; i < 3; i++)
        {
            // WARNING: CANNOT TEST TICK SNAPPING BECAUSE TICKS ARE FONT AND SYSTEM DEPENDENT
            plt.Should().RenderInMemoryWithoutThrowing();
        }
    }
}
