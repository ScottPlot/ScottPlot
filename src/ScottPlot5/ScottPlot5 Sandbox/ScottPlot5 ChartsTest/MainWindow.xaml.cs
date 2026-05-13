using System;
using System.Globalization;
using System.Windows;
using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Statistics;
using ScottPlot.TickGenerators;

namespace ChartsTest;

public partial class MainWindow : Window
{
    private enum AxisScale
    {
        Linear,
        Log,
        Biex,
    }

    private double[]? HistogramValues;

    public MainWindow()
    {
        InitializeComponent();
        DrawScatterPlot(AxisScale.Linear, AxisScale.Linear);
        DrawHeatmapPlot();
        DrawContourPlot();
        DrawHistogramPlot(GetHistogramXAxisScale());
    }

    /// <summary>
    /// 散点图：使用随机正态分布数据绘制点云
    /// </summary>
    private void DrawScatterPlot(AxisScale xScale, AxisScale yScale)
    {
        double[] xs = Generate.RandomNormal(200, 5, 3);
        double[] ys = Generate.RandomNormal(200, 7, 2);

        double xLogShift = 0, xBiexScale = 1;
        double yLogShift = 0, yBiexScale = 1;

        double[] plotXs = TransformByScale(xs, xScale, ref xLogShift, ref xBiexScale);
        double[] plotYs = TransformByScale(ys, yScale, ref yLogShift, ref yBiexScale);

        ScatterPlot.Plot.Clear();

        var sp = ScatterPlot.Plot.Add.ScatterPoints(plotXs, plotYs);
        sp.MarkerSize = 7;
        sp.MarkerShape = MarkerShape.FilledCircle;
        sp.LegendText = "正态分布散点";

        ApplyAxisScale(ScatterPlot.Plot.Axes.Bottom, xScale, xLogShift, xBiexScale);
        ApplyAxisScale(ScatterPlot.Plot.Axes.Left, yScale, yLogShift, yBiexScale);

        ScatterPlot.Plot.Title("散点图 — 正态分布点云");
        ScatterPlot.Plot.XLabel(AxisLabelText("X (μ=5, σ=3)", xScale, xLogShift, xBiexScale));
        ScatterPlot.Plot.YLabel(AxisLabelText("Y (μ=7, σ=2)", yScale, yLogShift, yBiexScale));
        ScatterPlot.Plot.ShowLegend();
        ScatterPlot.Refresh();
    }

    /// <summary>
    /// 热力图 / 密度图：50×50 随机数据矩阵，开启 Smooth 平滑渲染模拟密度图
    /// </summary>
    private void DrawHeatmapPlot()
    {
        int rows = 50;
        int cols = 50;
        double[,] intensities = new double[rows, cols];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // 叠加两个高斯峰，产生有趣的密度分布
                double dx1 = (x - 15.0) / 8.0;
                double dy1 = (y - 20.0) / 8.0;
                double dx2 = (x - 35.0) / 6.0;
                double dy2 = (y - 32.0) / 6.0;

                intensities[y, x] = Math.Exp(-(dx1 * dx1 + dy1 * dy1))
                    + 0.7 * Math.Exp(-(dx2 * dx2 + dy2 * dy2))
                    + Generate.RandomNormalNumber(0, 0.05);
            }
        }

        var hm = HeatmapPlot.Plot.Add.Heatmap(intensities);
        hm.Colormap = new Viridis();
        hm.Smooth = true;

        HeatmapPlot.Plot.Add.ColorBar(hm);

        HeatmapPlot.Plot.Title("热力图 — 二维密度分布 (Smooth)");
        HeatmapPlot.Refresh();
    }

    /// <summary>
    /// 等高线图：50×50 矩形网格，Z = sin(x) * cos(y) + 噪声
    /// </summary>
    private void DrawContourPlot()
    {
        int gridSize = 50;
        Coordinates3d[,] grid = new Coordinates3d[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            double cy = (y - gridSize / 2.0) / (gridSize / 8.0);
            for (int x = 0; x < gridSize; x++)
            {
                double cx = (x - gridSize / 2.0) / (gridSize / 8.0);
                double z = Math.Sin(cx) * Math.Cos(cy) * 3
                    + Math.Cos(cx * 1.5) * Math.Sin(cy * 1.3) * 2
                    + Math.Sin(cx * 0.8 + cy * 0.6) * 2.5;

                grid[y, x] = new Coordinates3d(cx, cy, z);
            }
        }

        var contour = ContourPlot.Plot.Add.ContourLines(grid, count: 12);
        contour.Colormap = new Turbo();
        contour.LineWidth = 1.5f;

        ContourPlot.Plot.Title("等高线图 — ContourLines (Turbo 色图)");
        ContourPlot.Refresh();
    }

    /// <summary>
    /// 直方图：1000 个正态分布值，20 个 bins
    /// </summary>
    private void DrawHistogramPlot(AxisScale scale)
    {
        HistogramValues ??= Generate.RandomNormal(1000, 0, 2);

        HistogramPlot.Plot.Clear();

        double logShift = 0;
        double biexScale = 1;

        double[] plotValues = scale switch
        {
            AxisScale.Linear => HistogramValues,
            AxisScale.Log => TransformLog(HistogramValues, out logShift),
            AxisScale.Biex => TransformBiex(HistogramValues, out biexScale),
            _ => throw new ArgumentOutOfRangeException(nameof(scale)),
        };

        Histogram histogram = Histogram.WithBinCount(20, plotValues);

        var hb = HistogramPlot.Plot.Add.Histogram(histogram, disableBottomPadding: true);
        hb.BarWidthFraction = 0.85;

        ApplyHistogramXAxis(scale, logShift, biexScale);

        HistogramPlot.Plot.Title("直方图 — 正态分布 (n=1000, μ=0, σ=2)");
        HistogramPlot.Plot.YLabel("计数");
        HistogramPlot.Plot.Axes.AutoScale();
        if (scale == AxisScale.Log && HistogramPlot.Plot.Axes.Bottom.Min < 0)
            HistogramPlot.Plot.Axes.Bottom.Min = 0;
        HistogramPlot.Refresh();
    }

    private void ApplyHistogramXAxis(AxisScale scale, double logShift, double biexScale)
    {
        IAxis xAxis = HistogramPlot.Plot.Axes.Bottom;
        NumericAutomatic tickGen = new();

        if (scale == AxisScale.Linear)
        {
            xAxis.TickGenerator = tickGen;
            HistogramPlot.Plot.XLabel("值");
            return;
        }

        if (scale == AxisScale.Log)
        {
            tickGen.LabelFormatter = (pos) => FormatNumber(InverseLog(pos, logShift));
            xAxis.TickGenerator = tickGen;
            HistogramPlot.Plot.XLabel($"值 (Log10(x + {FormatNumber(logShift)}))");
            return;
        }

        if (scale == AxisScale.Biex)
        {
            tickGen.LabelFormatter = (pos) => FormatNumber(InverseBiex(pos, biexScale));
            xAxis.TickGenerator = tickGen;
            HistogramPlot.Plot.XLabel($"值 (Biex, scale={FormatNumber(biexScale)})");
            return;
        }

        throw new ArgumentOutOfRangeException(nameof(scale));
    }

    private AxisScale GetHistogramXAxisScale()
    {
        int idx = HistogramXAxisTypeComboBox?.SelectedIndex ?? 0;
        return idx switch
        {
            0 => AxisScale.Linear,
            1 => AxisScale.Log,
            2 => AxisScale.Biex,
            _ => AxisScale.Linear,
        };
    }

    private void HistogramXAxisTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (HistogramPlot is null)
            return;

        DrawHistogramPlot(GetHistogramXAxisScale());
    }

    // ==================== 散点图轴类型切换 ====================

    private void ScatterXAxisTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ScatterPlot is null)
            return;

        DrawScatterPlot(GetScatterXAxisScale(), GetScatterYAxisScale());
    }

    private void ScatterYAxisTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ScatterPlot is null)
            return;

        DrawScatterPlot(GetScatterXAxisScale(), GetScatterYAxisScale());
    }

    private AxisScale GetScatterXAxisScale()
    {
        int idx = ScatterXAxisTypeComboBox?.SelectedIndex ?? 0;
        return idx switch
        {
            0 => AxisScale.Linear,
            1 => AxisScale.Log,
            2 => AxisScale.Biex,
            _ => AxisScale.Linear,
        };
    }

    private AxisScale GetScatterYAxisScale()
    {
        int idx = ScatterYAxisTypeComboBox?.SelectedIndex ?? 0;
        return idx switch
        {
            0 => AxisScale.Linear,
            1 => AxisScale.Log,
            2 => AxisScale.Biex,
            _ => AxisScale.Linear,
        };
    }

    // ==================== 散点图轴类型辅助方法 ====================

    private static double[] TransformByScale(double[] values, AxisScale scale, ref double logShift, ref double biexScale)
    {
        return scale switch
        {
            AxisScale.Log => TransformLog(values, out logShift),
            AxisScale.Biex => TransformBiex(values, out biexScale),
            _ => values,
        };
    }

    private static void ApplyAxisScale(IAxis axis, AxisScale scale, double logShift, double biexScale)
    {
        NumericAutomatic tickGen = new();

        if (scale == AxisScale.Log)
        {
            tickGen.LabelFormatter = (pos) => FormatNumber(InverseLog(pos, logShift));
        }
        else if (scale == AxisScale.Biex)
        {
            tickGen.LabelFormatter = (pos) => FormatNumber(InverseBiex(pos, biexScale));
        }

        axis.TickGenerator = tickGen;
    }

    private static string AxisLabelText(string baseLabel, AxisScale scale, double logShift, double biexScale)
    {
        return scale switch
        {
            AxisScale.Linear => baseLabel,
            AxisScale.Log => $"{baseLabel} [Log10(x + {FormatNumber(logShift)})]",
            AxisScale.Biex => $"{baseLabel} [Biex, scale={FormatNumber(biexScale)}]",
            _ => baseLabel,
        };
    }

    // ==================== 数学变换 ====================

    private static double[] TransformLog(double[] xs, out double shift)
    {
        double min = double.PositiveInfinity;
        for (int i = 0; i < xs.Length; i++)
            min = Math.Min(min, xs[i]);

        shift = min <= 0
            ? -min + 1
            : 0;

        double[] ys = new double[xs.Length];
        for (int i = 0; i < xs.Length; i++)
        {
            double v = xs[i] + shift;
            v = Math.Max(v, double.Epsilon);
            ys[i] = Math.Log10(v);
        }

        return ys;
    }

    private static double InverseLog(double log10Value, double shift)
    {
        double v = Math.Pow(10, log10Value) - shift;
        return v;
    }

    private static double[] TransformBiex(double[] xs, out double scale)
    {
        scale = GetDefaultBiexScale(xs);

        double[] ys = new double[xs.Length];
        for (int i = 0; i < xs.Length; i++)
            ys[i] = Asinh(xs[i] / scale);

        return ys;
    }

    private static double InverseBiex(double axisValue, double scale)
    {
        return scale * Sinh(axisValue);
    }

    private static double GetDefaultBiexScale(double[] xs)
    {
        double medianAbs = MedianAbs(xs);
        if (medianAbs <= 0)
            return 1;

        double s = medianAbs / 2;
        return Math.Max(s, 0.1);
    }

    private static double MedianAbs(double[] xs)
    {
        double[] abs = new double[xs.Length];
        for (int i = 0; i < xs.Length; i++)
            abs[i] = Math.Abs(xs[i]);

        Array.Sort(abs);
        int mid = abs.Length / 2;
        return abs.Length % 2 == 0
            ? (abs[mid - 1] + abs[mid]) / 2.0
            : abs[mid];
    }

    private static double Asinh(double x)
    {
        double xx = x * x;
        return Math.Log(x + Math.Sqrt(xx + 1));
    }

    private static double Sinh(double x)
    {
        double ex = Math.Exp(x);
        double emx = 1.0 / ex;
        return (ex - emx) / 2.0;
    }

    private static string FormatNumber(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            return "NaN";

        double abs = Math.Abs(value);
        if (abs > 0 && (abs >= 1_000_000 || abs < 0.001))
            return value.ToString("0.###e+0", CultureInfo.InvariantCulture);

        return value.ToString("0.###", CultureInfo.InvariantCulture);
    }
}
