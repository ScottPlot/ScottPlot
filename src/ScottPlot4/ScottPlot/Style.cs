﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public enum Style
    {
        Default,
        Control,
        Blue1,
        Blue2,
        Blue3,
        Light1,
        Light2,
        Gray1,
        Gray2,
        Black,
        Seaborn
    }

    public enum TextAlignment
    {
        // TODO: capitolize these in ScottPlot 4.1
        upperLeft,
        upperRight,
        upperCenter,
        middleLeft,
        middleRight,
        lowerLeft,
        lowerRight,
        lowerCenter,
        middleCenter
    }

    public enum LineStyle
    {
        None,
        Solid,
        Dash,
        DashDot,
        DashDotDot,
        Dot
    }

    public static class StyleTools
    {
        public static void SetStyle(Plot existingPlot, Style style)
        {
            switch (style)
            {
                case (Style.Black):
                    existingPlot.Style(
                        figBg: Color.Black,
                        dataBg: Color.Black,
                        grid: ColorTranslator.FromHtml("#2d2d2d"),
                        tick: ColorTranslator.FromHtml("#757575"),
                        label: ColorTranslator.FromHtml("#b9b9ba"),
                        title: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Blue1):
                    existingPlot.Style(
                        figBg: ColorTranslator.FromHtml("#07263b"),
                        dataBg: ColorTranslator.FromHtml("#0b3049"),
                        grid: ColorTranslator.FromHtml("#0e3d54"),
                        tick: ColorTranslator.FromHtml("#145665"),
                        label: ColorTranslator.FromHtml("#b5bec5"),
                        title: ColorTranslator.FromHtml("#d0dae2")
                        );
                    break;

                case (Style.Blue2):
                    existingPlot.Style(
                        figBg: ColorTranslator.FromHtml("#1b2138"),
                        dataBg: ColorTranslator.FromHtml("#252c48"),
                        grid: ColorTranslator.FromHtml("#2c334e"),
                        tick: ColorTranslator.FromHtml("#bbbdc4"),
                        label: ColorTranslator.FromHtml("#bbbdc4"),
                        title: ColorTranslator.FromHtml("#d8dbe3")
                        );
                    break;

                case (Style.Blue3):
                    existingPlot.Style(
                        figBg: ColorTranslator.FromHtml("#001021"),
                        dataBg: ColorTranslator.FromHtml("#021d38"),
                        grid: ColorTranslator.FromHtml("#273c51"),
                        tick: ColorTranslator.FromHtml("#d3d3d3"),
                        label: ColorTranslator.FromHtml("#d3d3d3"),
                        title: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Gray1):
                    existingPlot.Style(
                        figBg: ColorTranslator.FromHtml("#31363a"),
                        dataBg: ColorTranslator.FromHtml("#3a4149"),
                        grid: ColorTranslator.FromHtml("#444b52"),
                        tick: ColorTranslator.FromHtml("#757a80"),
                        label: ColorTranslator.FromHtml("#d6d7d8"),
                        title: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Gray2):
                    existingPlot.Style(
                        figBg: ColorTranslator.FromHtml("#131519"),
                        dataBg: ColorTranslator.FromHtml("#262626"),
                        grid: ColorTranslator.FromHtml("#2d2d2d"),
                        tick: ColorTranslator.FromHtml("#757575"),
                        label: ColorTranslator.FromHtml("#b9b9ba"),
                        title: ColorTranslator.FromHtml("#FFFFFF")
                        );
                    break;

                case (Style.Light1):
                    existingPlot.Style(
                        figBg: ColorTranslator.FromHtml("#FFFFFF"),
                        dataBg: ColorTranslator.FromHtml("#FFFFFF"),
                        grid: ColorTranslator.FromHtml("#ededed "),
                        tick: ColorTranslator.FromHtml("#7f7f7f"),
                        label: ColorTranslator.FromHtml("#000000"),
                        title: ColorTranslator.FromHtml("#000000")
                        );
                    break;

                case (Style.Light2):
                    existingPlot.Style(
                        figBg: ColorTranslator.FromHtml("#e4e6ec"),
                        dataBg: ColorTranslator.FromHtml("#f1f3f7"),
                        grid: ColorTranslator.FromHtml("#e5e7ea"),
                        tick: ColorTranslator.FromHtml("#77787b"),
                        label: ColorTranslator.FromHtml("#000000"),
                        title: ColorTranslator.FromHtml("#000000")
                        );
                    break;

                case (Style.Control):
                    existingPlot.Style(
                        figBg: SystemColors.Control,
                        dataBg: Color.White,
                        grid: ColorTranslator.FromHtml("#efefef"),
                        tick: Color.Black,
                        label: Color.Black,
                        title: Color.Black
                        );
                    break;

                case (Style.Seaborn):
                    existingPlot.Style(
                        figBg: Color.White,
                        dataBg: ColorTranslator.FromHtml("#eaeaf2"),
                        grid: Color.White,
                        tick: ColorTranslator.FromHtml("#AAAAAA"),
                        label: Color.Black,
                        title: Color.Black
                        );
                    existingPlot.Frame(false);
                    existingPlot.Ticks(displayTicksXminor: false, displayTicksYminor: false);
                    break;

                default:
                    existingPlot.Style(
                        figBg: Color.White,
                        dataBg: Color.White,
                        grid: ColorTranslator.FromHtml("#efefef"),
                        tick: Color.Black,
                        label: Color.Black,
                        title: Color.Black
                        );
                    break;
            }
        }
    }

}
