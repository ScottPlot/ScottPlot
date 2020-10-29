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

            settings.title.text = label ?? settings.title.text;
            settings.title.fontName = fontName ?? settings.title.fontName;
            settings.title.fontSize = fontSize ?? settings.title.fontSize;
            settings.title.color = color ?? settings.title.color;
            settings.title.bold = bold ?? settings.title.bold;

            TightenLayout();
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

            settings.xLabel.text = label ?? settings.xLabel.text;
            settings.xLabel.color = color ?? settings.xLabel.color;
            settings.xLabel.fontName = fontName ?? settings.xLabel.fontName;
            settings.xLabel.fontSize = fontSize ?? settings.xLabel.fontSize;
            settings.xLabel.bold = bold ?? settings.xLabel.bold;

            TightenLayout();
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

            settings.yLabel.text = label ?? settings.yLabel.text;
            settings.yLabel.color = color ?? settings.yLabel.color;
            settings.yLabel.fontName = fontName ?? settings.yLabel.fontName;
            settings.yLabel.fontSize = fontSize ?? settings.yLabel.fontSize;
            settings.yLabel.bold = bold ?? settings.yLabel.bold;

            TightenLayout();
        }
    }
}
