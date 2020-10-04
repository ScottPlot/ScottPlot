using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
        public void Title(
            string title = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {

            settings.title.text = title ?? settings.title.text;
            settings.title.visible = enable ?? settings.title.visible;
            settings.title.fontName = fontName ?? settings.title.fontName;
            settings.title.fontSize = fontSize ?? settings.title.fontSize;
            settings.title.color = color ?? settings.title.color;
            settings.title.bold = bold ?? settings.title.bold;

            TightenLayout();
        }

        public void XLabel(
            string xLabel = null,
            Color? color = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null
            )
        {
            settings.xLabel.text = xLabel ?? settings.xLabel.text;
            settings.xLabel.color = color ?? settings.xLabel.color;
            settings.xLabel.visible = enable ?? settings.xLabel.visible;
            settings.xLabel.fontName = fontName ?? settings.xLabel.fontName;
            settings.xLabel.fontSize = fontSize ?? settings.xLabel.fontSize;
            settings.xLabel.bold = bold ?? settings.xLabel.bold;

            TightenLayout();
        }

        public void YLabel(
            string yLabel = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {
            settings.yLabel.text = yLabel ?? settings.yLabel.text;
            settings.yLabel.color = color ?? settings.yLabel.color;
            settings.yLabel.visible = enable ?? settings.yLabel.visible;
            settings.yLabel.fontName = fontName ?? settings.yLabel.fontName;
            settings.yLabel.fontSize = fontSize ?? settings.yLabel.fontSize;
            settings.yLabel.bold = bold ?? settings.yLabel.bold;

            TightenLayout();
        }
    }
}
