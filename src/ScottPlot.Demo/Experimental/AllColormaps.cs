using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable CS0618 // Type or member is obsolete
namespace ScottPlot.Demo.Experimental
{
    class AllColormaps
    {
        public class Algae : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Algae";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Algae(), drawAxisLabels: false);
            }
        }

        public class Amp : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Amp";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Amp(), drawAxisLabels: false);
            }
        }

        public class Balance : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Balance";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Balance(), drawAxisLabels: false);
            }
        }

        public class Curl : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Curl";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Curl(), drawAxisLabels: false);
            }
        }

        public class Deep : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Deep";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Deep(), drawAxisLabels: false);
            }
        }

        public class Delta : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Delta";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Delta(), drawAxisLabels: false);
            }
        }

        public class Dense : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Dense";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Dense(), drawAxisLabels: false);
            }
        }

        public class Diff : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Diff";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Diff(), drawAxisLabels: false);
            }
        }

        public class Grayscale : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Grayscale";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Grayscale(), drawAxisLabels: false);
            }
        }

        public class GrayscaleInverted : PlotDemo, IPlotDemo
        {
            public string name { get; } = "GrayscaleInverted";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.GrayscaleInverted(), drawAxisLabels: false);
            }
        }

        public class Haline : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Haline";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Haline(), drawAxisLabels: false);
            }
        }

        public class Ice : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Ice";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Ice(), drawAxisLabels: false);
            }
        }

        public class Inferno : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Inferno";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Inferno(), drawAxisLabels: false);
            }
        }

        public class Magma : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Magma";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Magma(), drawAxisLabels: false);
            }
        }
        public class Matter : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Matter";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Matter(), drawAxisLabels: false);
            }
        }

        public class Oxy : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Oxy";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Oxy(), drawAxisLabels: false);
            }
        }
        public class Phase : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Phase";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Phase(), drawAxisLabels: false);
            }
        }

        public class Plasma : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plasma";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Plasma(), drawAxisLabels: false);
            }
        }
        public class Rain : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Rain";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Rain(), drawAxisLabels: false);
            }
        }

        public class Solar : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Solar";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Solar(), drawAxisLabels: false);
            }
        }
        public class Speed : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Speed";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Speed(), drawAxisLabels: false);
            }
        }

        public class Tarn : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Tarn";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Tarn(), drawAxisLabels: false);
            }
        }

        public class Tempo : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Tempo";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Tempo(), drawAxisLabels: false);
            }
        }

        public class Thermal : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Thermal";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Thermal(), drawAxisLabels: false);
            }
        }
        public class Topo : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Topo";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Topo(), drawAxisLabels: false);
            }
        }

        public class Turbid : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Turbid";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Turbid(), drawAxisLabels: false);
            }
        }

        public class Turbo : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Turbo";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Turbo(), drawAxisLabels: false);
            }
        }

        public class Viridis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Viridis";
            public string description { get; } = "Demonstrates every colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                plt.PlotHeatmap(intensities, new Config.ColorMaps.Viridis(), drawAxisLabels: false);
            }
        }
    }
}
