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
            XAxis2.Title = label;
            XAxis2.TitleFont.Name = string.IsNullOrWhiteSpace(fontName) ? XAxis2.TitleFont.Name : fontName;
            XAxis2.TitleFont.Size = fontSize ?? XAxis2.TitleFont.Size;
            XAxis2.TitleFont.Color = color ?? XAxis2.TitleFont.Color;
            XAxis2.TitleFont.Bold = bold ?? XAxis2.TitleFont.Bold;
        }

        public void XLabel(
            string label = null,
            Color? color = null,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null
            )
        {
            XAxis.Title = label;
            XAxis.TitleFont.Name = string.IsNullOrWhiteSpace(fontName) ? XAxis.TitleFont.Name : fontName;
            XAxis.TitleFont.Size = fontSize ?? XAxis.TitleFont.Size;
            XAxis.TitleFont.Color = color ?? XAxis.TitleFont.Color;
            XAxis.TitleFont.Bold = bold ?? XAxis.TitleFont.Bold;
        }

        public void YLabel(
            string label = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {
            YAxis.Title = label;
            YAxis.TitleFont.Name = string.IsNullOrWhiteSpace(fontName) ? YAxis.TitleFont.Name : fontName;
            YAxis.TitleFont.Size = fontSize ?? YAxis.TitleFont.Size;
            YAxis.TitleFont.Color = color ?? YAxis.TitleFont.Color;
            YAxis.TitleFont.Bold = bold ?? YAxis.TitleFont.Bold;
        }
    }
}
