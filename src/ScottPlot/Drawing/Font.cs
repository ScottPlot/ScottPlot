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

        private string _Name;
        public string Name
        {
            get => _Name;
            set => _Name = InstalledFont.ValidFontName(value); // ensure only valid font names can be assigned
        }

        public Font() => Name = InstalledFont.Sans();
        //public Font(float size) => (Name, Size) = (InstalledFont.Sans(), size);
        //public Font(float size, bool bold) => (Name, Size, Bold) = (InstalledFont.Sans(), size, bold);
        //public Font(float size, Color color) => (Name, Size, Color) = (InstalledFont.Sans(), size, color);

        // TODO: obsolete?
        public Font(string name, float size, Color color, Alignment align = Alignment.UpperLeft) => (Name, Size, Color, Alignment) = (name, size, color, align);
    }
}
