using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
        public void Title(
            string label = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {
            settings.XAxis2.Title.Label = label;
            settings.XAxis2.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? settings.XAxis2.Title.Font.Name : fontName;
            settings.XAxis2.Title.Font.Size = fontSize ?? settings.XAxis2.Title.Font.Size;
            settings.XAxis2.Title.Font.Color = color ?? settings.XAxis2.Title.Font.Color;
            settings.XAxis2.Title.Font.Bold = bold ?? settings.XAxis2.Title.Font.Bold;
        }

        public void XLabel(
            string label = null,
            Color? color = null,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null
            )
        {
            settings.XAxis.Title.Label = label;
            settings.XAxis.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? settings.XAxis.Title.Font.Name : fontName;
            settings.XAxis.Title.Font.Size = fontSize ?? settings.XAxis.Title.Font.Size;
            settings.XAxis.Title.Font.Color = color ?? settings.XAxis.Title.Font.Color;
            settings.XAxis.Title.Font.Bold = bold ?? settings.XAxis.Title.Font.Bold;
        }

        public void YLabel(
            string label = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {
            settings.YAxis.Title.Label = label;
            settings.YAxis.Title.Font.Name = string.IsNullOrWhiteSpace(fontName) ? settings.YAxis.Title.Font.Name : fontName;
            settings.YAxis.Title.Font.Size = fontSize ?? settings.YAxis.Title.Font.Size;
            settings.YAxis.Title.Font.Color = color ?? settings.YAxis.Title.Font.Color;
            settings.YAxis.Title.Font.Bold = bold ?? settings.YAxis.Title.Font.Bold;
        }
    }
}
