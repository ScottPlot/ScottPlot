﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Drawing
{
    public class Font
    {
        private string _Name;
        public string Name { get => _Name; set => _Name = InstalledFont.ValidFontName(value); }
        public float Size = 12;
        public Color Color = Color.Black;
        public Alignment Alignment = Alignment.UpperLeft;
        public bool Bold = false;

        public Font() { Name = InstalledFont.Sans(); }

        public Font(string name, float size, Color color, Alignment align = Alignment.UpperLeft) =>
            (Name, Size, Color, Alignment) = (name, size, color, align);
    }
}
