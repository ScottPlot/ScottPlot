﻿using System;
using System.Drawing;
using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot
{
    // Variation of PlottableSignal that uses a segmented tree for faster min/max range queries
    // - frequent min/max lookups are a bottleneck displaying large signals
    // - double[] limited to 60M points (250M in x64 mode) due to memory (tree uses from 2X to 4X memory)
    // - smaller data types have higher points count limits
    // - in x64 mode limit can be up to maximum array size (2G points) with special solution and 64 GB RAM (not tested)
    // - if source array is changed UpdateTrees() must be called
    // - source array can be change by call updateData(), updating by ranges much faster.
    public class PlottableSignalConst<T> : PlottableSignalBase<T> where T : struct, IComparable
    {
        public bool TreesReady => true;
        public PlottableSignalConst(T[] ys, double sampleRate, double xOffset, double yOffset, Color color,
            double lineWidth, double markerSize, string label, Color[] colorByDensity,
            int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel)
            : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label, colorByDensity,
                 minRenderIndex, maxRenderIndex, lineStyle, useParallel, new SegmentedTreeMinMaxSearchStrategy<T>())
        {
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignalConst{label} with {GetPointCount()} points ({typeof(T).Name})";
        }
    }
}
