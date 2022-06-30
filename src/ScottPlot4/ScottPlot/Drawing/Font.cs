using System;
using System.Drawing;

namespace ScottPlot.Drawing
{
    public class Font
    {
        public float Size = 12;
        public Color Color = Color.Black;
        public Alignment Alignment = Alignment.UpperLeft;
        public bool Bold = false;
        public float Rotation = 0;

        public string Name
        {
            get => Family.Name;
            set => Family = InstalledFont.ValidFontFamily(value); // ensure only valid font names can be assigned
        }

        public FontFamily Family { get; set; }

        public Font()
        {
            Family = InstalledFont.SansFamily;
        }
    }
}
