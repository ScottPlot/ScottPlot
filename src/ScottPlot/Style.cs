using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public static class StyleTools
    {
        public static void SetStyle(Plot existingPlot, Style style)
        {
            // TODO: this system really needs refactoring...
            switch (style)
            {
                case (Style.Black):
                    existingPlot.Style(
                        figureBackgroundColor: Color.Black,
                        dataBackgroundColor: Color.Black,
                        gridColor: ColorTranslator.FromHtml("#2d2d2d"),
                        tickColor: ColorTranslator.FromHtml("#757575"),
                        axisLabelColor: ColorTranslator.FromHtml("#b9b9ba"),
                        titleLabelColor: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Blue1):
                    existingPlot.Style(
                        figureBackgroundColor: ColorTranslator.FromHtml("#07263b"),
                        dataBackgroundColor: ColorTranslator.FromHtml("#0b3049"),
                        gridColor: ColorTranslator.FromHtml("#0e3d54"),
                        tickColor: ColorTranslator.FromHtml("#145665"),
                        axisLabelColor: ColorTranslator.FromHtml("#b5bec5"),
                        titleLabelColor: ColorTranslator.FromHtml("#d0dae2")
                        );
                    break;

                case (Style.Blue2):
                    existingPlot.Style(
                        figureBackgroundColor: ColorTranslator.FromHtml("#1b2138"),
                        dataBackgroundColor: ColorTranslator.FromHtml("#252c48"),
                        gridColor: ColorTranslator.FromHtml("#2c334e"),
                        tickColor: ColorTranslator.FromHtml("#bbbdc4"),
                        axisLabelColor: ColorTranslator.FromHtml("#bbbdc4"),
                        titleLabelColor: ColorTranslator.FromHtml("#d8dbe3")
                        );
                    break;

                case (Style.Blue3):
                    existingPlot.Style(
                        figureBackgroundColor: ColorTranslator.FromHtml("#001021"),
                        dataBackgroundColor: ColorTranslator.FromHtml("#021d38"),
                        gridColor: ColorTranslator.FromHtml("#273c51"),
                        tickColor: ColorTranslator.FromHtml("#d3d3d3"),
                        axisLabelColor: ColorTranslator.FromHtml("#d3d3d3"),
                        titleLabelColor: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Gray1):
                    existingPlot.Style(
                        figureBackgroundColor: ColorTranslator.FromHtml("#31363a"),
                        dataBackgroundColor: ColorTranslator.FromHtml("#3a4149"),
                        gridColor: ColorTranslator.FromHtml("#444b52"),
                        tickColor: ColorTranslator.FromHtml("#757a80"),
                        axisLabelColor: ColorTranslator.FromHtml("#d6d7d8"),
                        titleLabelColor: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Gray2):
                    existingPlot.Style(
                        figureBackgroundColor: ColorTranslator.FromHtml("#131519"),
                        dataBackgroundColor: ColorTranslator.FromHtml("#262626"),
                        gridColor: ColorTranslator.FromHtml("#2d2d2d"),
                        tickColor: ColorTranslator.FromHtml("#757575"),
                        axisLabelColor: ColorTranslator.FromHtml("#b9b9ba"),
                        titleLabelColor: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Light1):
                    existingPlot.Style(
                        figureBackgroundColor: ColorTranslator.FromHtml("#FFFFFF"),
                        dataBackgroundColor: ColorTranslator.FromHtml("#FFFFFF"),
                        gridColor: ColorTranslator.FromHtml("#ededed "),
                        tickColor: ColorTranslator.FromHtml("#7f7f7f"),
                        axisLabelColor: ColorTranslator.FromHtml("#000000"),
                        titleLabelColor: ColorTranslator.FromHtml("#000000")
                        );
                    break;

                case (Style.Light2):
                    existingPlot.Style(
                        figureBackgroundColor: ColorTranslator.FromHtml("#e4e6ec"),
                        dataBackgroundColor: ColorTranslator.FromHtml("#f1f3f7"),
                        gridColor: ColorTranslator.FromHtml("#e5e7ea"),
                        tickColor: ColorTranslator.FromHtml("#77787b"),
                        axisLabelColor: ColorTranslator.FromHtml("#000000"),
                        titleLabelColor: ColorTranslator.FromHtml("#000000")
                        );
                    break;

                case (Style.Control):
                    existingPlot.Style(
                        figureBackgroundColor: SystemColors.Control,
                        dataBackgroundColor: Color.White,
                        gridColor: ColorTranslator.FromHtml("#efefef"),
                        tickColor: Color.Black,
                        axisLabelColor: Color.Black,
                        titleLabelColor: Color.Black
                        );
                    break;

                case (Style.Seaborn):
                    existingPlot.Style(
                        figureBackgroundColor: Color.White,
                        dataBackgroundColor: ColorTranslator.FromHtml("#eaeaf2"),
                        gridColor: Color.White,
                        tickColor: ColorTranslator.FromHtml("#AAAAAA"),
                        axisLabelColor: Color.Black,
                        titleLabelColor: Color.Black
                        );
                    existingPlot.Frame(false);
                    existingPlot.XAxis.MinorLogScale(true);
                    existingPlot.YAxis.MinorLogScale(true);
                    break;

                default:
                    existingPlot.Style(
                        figureBackgroundColor: Color.White,
                        dataBackgroundColor: Color.White,
                        gridColor: ColorTranslator.FromHtml("#efefef"),
                        tickColor: Color.Black,
                        axisLabelColor: Color.Black,
                        titleLabelColor: Color.Black
                        );
                    break;
            }
        }
    }

}
