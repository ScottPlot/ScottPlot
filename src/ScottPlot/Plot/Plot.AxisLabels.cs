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
            settings.XAxis2.Title = label;
            settings.XAxis2.TitleFont.Name = string.IsNullOrWhiteSpace(fontName) ? settings.XAxis2.TitleFont.Name : fontName;
            settings.XAxis2.TitleFont.Size = fontSize ?? settings.XAxis2.TitleFont.Size;
            settings.XAxis2.TitleFont.Color = color ?? settings.XAxis2.TitleFont.Color;
            settings.XAxis2.TitleFont.Bold = bold ?? settings.XAxis2.TitleFont.Bold;
        }

        public void XLabel(
            string label = null,
            Color? color = null,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null
            )
        {
            settings.XAxis.Title = label;
            settings.XAxis.TitleFont.Name = string.IsNullOrWhiteSpace(fontName) ? settings.XAxis.TitleFont.Name : fontName;
            settings.XAxis.TitleFont.Size = fontSize ?? settings.XAxis.TitleFont.Size;
            settings.XAxis.TitleFont.Color = color ?? settings.XAxis.TitleFont.Color;
            settings.XAxis.TitleFont.Bold = bold ?? settings.XAxis.TitleFont.Bold;
        }

        public void YLabel(
            string label = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {
            settings.YAxis.Title = label;
            settings.YAxis.TitleFont.Name = string.IsNullOrWhiteSpace(fontName) ? settings.YAxis.TitleFont.Name : fontName;
            settings.YAxis.TitleFont.Size = fontSize ?? settings.YAxis.TitleFont.Size;
            settings.YAxis.TitleFont.Color = color ?? settings.YAxis.TitleFont.Color;
            settings.YAxis.TitleFont.Bold = bold ?? settings.YAxis.TitleFont.Bold;
        }
    }
}
