﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;
using System;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class ShowValueOnHover : Window
    {
        AvaPlot avaPlot1;
        private readonly ScatterPlot MyScatterPlot;
        private readonly ScatterPlot HighlightedPoint;
        private int LastHighlightedIndex = -1;

        public ShowValueOnHover()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            // create a scatter plot from some random data and save it
            Random rand = new Random(0);
            int pointCount = 20;
            double[] xs = DataGen.Random(rand, pointCount);
            double[] ys = DataGen.Random(rand, pointCount, multiplier: 1_000);
            MyScatterPlot = avaPlot1.Plot.AddScatterPoints(xs, ys);

            // Add a red circle we can move around later as a highlighted point indicator
            HighlightedPoint = avaPlot1.Plot.AddPoint(0, 0);
            HighlightedPoint.Color = System.Drawing.Color.Red;
            HighlightedPoint.MarkerSize = 10;
            HighlightedPoint.MarkerShape = ScottPlot.MarkerShape.openCircle;
            HighlightedPoint.IsVisible = false;

            avaPlot1.PointerMoved += MouseMove;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void MouseMove(object sender, PointerEventArgs e)
        {
            // determine point nearest the cursor
            (double mouseCoordX, double mouseCoordY) = avaPlot1.GetMouseCoordinates();
            double xyRatio = avaPlot1.Plot.XAxis.Dims.PxPerUnit / avaPlot1.Plot.YAxis.Dims.PxPerUnit;
            (double pointX, double pointY, int pointIndex) = MyScatterPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);

            // place the highlight over the point of interest
            HighlightedPoint.Xs[0] = pointX;
            HighlightedPoint.Ys[0] = pointY;
            HighlightedPoint.IsVisible = true;

            // render if the highlighted point chnaged
            if (LastHighlightedIndex != pointIndex)
            {
                LastHighlightedIndex = pointIndex;
                avaPlot1.Render();
            }

            // update the GUI to describe the highlighted point
            (double mouseX, double mouseY) = avaPlot1.GetMouseCoordinates();
            this.Find<TextBlock>("label1").Text = $"Closest point to ({mouseX:N0}, {mouseY:N0}) " +
                $"is index {pointIndex} ({pointX:N2}, {pointY:N2})";
        }
    }
}
