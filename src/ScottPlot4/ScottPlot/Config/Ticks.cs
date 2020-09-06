﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Ticks
    {
        public string fontName = Fonts.GetDefaultFontName();
        public float fontSize = 12;
        public Font font { get { return new Font(fontName, fontSize, GraphicsUnit.Pixel); } }

        public bool displayYlabels = true;
        public bool displayXlabels = true;

        public readonly TickCollection x = new TickCollection(false);
        public readonly TickCollection y = new TickCollection(true);
        public int size = 5;
        public Color color = Color.Black;

        public bool displayXmajor = true;
        public bool displayXminor = true;

        public double manualSpacingX = 0;
        public double manualSpacingY = 0;
        public Config.DateTimeUnit? manualDateTimeSpacingUnitX = null;
        public Config.DateTimeUnit? manualDateTimeSpacingUnitY = null;

        public bool rulerModeX = false;
        public bool rulerModeY = false;

        public double rotationX = 0;

        public bool displayYmajor = true;
        public bool displayYminor = true;

        public bool useMultiplierNotation = false;
        public bool useOffsetNotation = false;
        public bool useExponentialNotation = true;

        public bool snapToNearestPixel = true;
    }
}
