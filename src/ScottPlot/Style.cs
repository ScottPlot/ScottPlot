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
                        figureBackground: Color.Black,
                        dataBackground: Color.Black,
                        grid: ColorTranslator.FromHtml("#2d2d2d"),
                        tick: ColorTranslator.FromHtml("#757575"),
                        axisLabel: ColorTranslator.FromHtml("#b9b9ba"),
                        titleLabel: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Blue1):
                    existingPlot.Style(
                        figureBackground: ColorTranslator.FromHtml("#07263b"),
                        dataBackground: ColorTranslator.FromHtml("#0b3049"),
                        grid: ColorTranslator.FromHtml("#0e3d54"),
                        tick: ColorTranslator.FromHtml("#145665"),
                        axisLabel: ColorTranslator.FromHtml("#b5bec5"),
                        titleLabel: ColorTranslator.FromHtml("#d0dae2")
                        );
                    break;

                case (Style.Blue2):
                    existingPlot.Style(
                        figureBackground: ColorTranslator.FromHtml("#1b2138"),
                        dataBackground: ColorTranslator.FromHtml("#252c48"),
                        grid: ColorTranslator.FromHtml("#2c334e"),
                        tick: ColorTranslator.FromHtml("#bbbdc4"),
                        axisLabel: ColorTranslator.FromHtml("#bbbdc4"),
                        titleLabel: ColorTranslator.FromHtml("#d8dbe3")
                        );
                    break;

                case (Style.Blue3):
                    existingPlot.Style(
                        figureBackground: ColorTranslator.FromHtml("#001021"),
                        dataBackground: ColorTranslator.FromHtml("#021d38"),
                        grid: ColorTranslator.FromHtml("#273c51"),
                        tick: ColorTranslator.FromHtml("#d3d3d3"),
                        axisLabel: ColorTranslator.FromHtml("#d3d3d3"),
                        titleLabel: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Gray1):
                    existingPlot.Style(
                        figureBackground: ColorTranslator.FromHtml("#31363a"),
                        dataBackground: ColorTranslator.FromHtml("#3a4149"),
                        grid: ColorTranslator.FromHtml("#444b52"),
                        tick: ColorTranslator.FromHtml("#757a80"),
                        axisLabel: ColorTranslator.FromHtml("#d6d7d8"),
                        titleLabel: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Gray2):
                    existingPlot.Style(
                        figureBackground: ColorTranslator.FromHtml("#131519"),
                        dataBackground: ColorTranslator.FromHtml("#262626"),
                        grid: ColorTranslator.FromHtml("#2d2d2d"),
                        tick: ColorTranslator.FromHtml("#757575"),
                        axisLabel: ColorTranslator.FromHtml("#b9b9ba"),
                        titleLabel: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Light1):
                    existingPlot.Style(
                        figureBackground: ColorTranslator.FromHtml("#FFFFFF"),
                        dataBackground: ColorTranslator.FromHtml("#FFFFFF"),
                        grid: ColorTranslator.FromHtml("#ededed "),
                        tick: ColorTranslator.FromHtml("#7f7f7f"),
                        axisLabel: ColorTranslator.FromHtml("#000000"),
                        titleLabel: ColorTranslator.FromHtml("#000000")
                        );
                    break;

                case (Style.Light2):
                    existingPlot.Style(
                        figureBackground: ColorTranslator.FromHtml("#e4e6ec"),
                        dataBackground: ColorTranslator.FromHtml("#f1f3f7"),
                        grid: ColorTranslator.FromHtml("#e5e7ea"),
                        tick: ColorTranslator.FromHtml("#77787b"),
                        axisLabel: ColorTranslator.FromHtml("#000000"),
                        titleLabel: ColorTranslator.FromHtml("#000000")
                        );
                    break;

                case (Style.Control):
                    existingPlot.Style(
                        figureBackground: SystemColors.Control,
                        dataBackground: Color.White,
                        grid: ColorTranslator.FromHtml("#efefef"),
                        tick: Color.Black,
                        axisLabel: Color.Black,
                        titleLabel: Color.Black
                        );
                    break;

                case (Style.Seaborn):
                    existingPlot.Style(
                        figureBackground: Color.White,
                        dataBackground: ColorTranslator.FromHtml("#eaeaf2"),
                        grid: Color.White,
                        tick: ColorTranslator.FromHtml("#AAAAAA"),
                        axisLabel: Color.Black,
                        titleLabel: Color.Black
                        );
                    existingPlot.Frame(false);
                    existingPlot.XAxis.MinorLogScale(true);
                    existingPlot.YAxis.MinorLogScale(true);
                    break;

                default:
                    existingPlot.Style(
                        figureBackground: Color.White,
                        dataBackground: Color.White,
                        grid: ColorTranslator.FromHtml("#efefef"),
                        tick: Color.Black,
                        axisLabel: Color.Black,
                        titleLabel: Color.Black
                        );
                    break;
            }
        }
    }

}
