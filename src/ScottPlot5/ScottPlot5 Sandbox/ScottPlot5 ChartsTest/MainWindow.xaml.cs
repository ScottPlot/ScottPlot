using System;
using System.Windows;
using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Statistics;

namespace ChartsTest;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DrawScatterPlot();
        DrawHeatmapPlot();
        DrawContourPlot();
        DrawHistogramPlot();
    }

    /// <summary>
    /// 散点图：使用随机正态分布数据绘制点云
    /// </summary>
    private void DrawScatterPlot()
    {
        double[] xs = Generate.RandomNormal(200, 5, 3);
        double[] ys = Generate.RandomNormal(200, 7, 2);

        var sp = ScatterPlot.Plot.Add.ScatterPoints(xs, ys);
        sp.MarkerSize = 7;
        sp.MarkerShape = MarkerShape.FilledCircle;
        sp.LegendText = "正态分布散点";

        ScatterPlot.Plot.Title("散点图 — 正态分布点云");
        ScatterPlot.Plot.XLabel("X (μ=5, σ=3)");
        ScatterPlot.Plot.YLabel("Y (μ=7, σ=2)");
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
    private void DrawHistogramPlot()
    {
        double[] values = Generate.RandomNormal(1000, 0, 2);
        Histogram histogram = Histogram.WithBinCount(20, values);

        var hb = HistogramPlot.Plot.Add.Histogram(histogram, disableBottomPadding: true);
        hb.BarWidthFraction = 0.85;

        HistogramPlot.Plot.Title("直方图 — 正态分布 (n=1000, μ=0, σ=2)");
        HistogramPlot.Plot.XLabel("值");
        HistogramPlot.Plot.YLabel("计数");
        HistogramPlot.Refresh();
    }
}
