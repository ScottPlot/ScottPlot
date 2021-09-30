using System.Linq;

namespace ScottPlot
{
    public static class Style
    {
        /* PLEASE KEEP THIS LIST ALPHABETIZED */
        public static Styles.IStyle Black => new Styles.Black();
        public static Styles.IStyle Blue1 => new Styles.Blue1();
        public static Styles.IStyle Blue2 => new Styles.Blue2();
        public static Styles.IStyle Blue3 => new Styles.Blue3();
        public static Styles.IStyle Control => new Styles.Control();
        public static Styles.IStyle Default => new Styles.Default();
        public static Styles.IStyle Gray1 => new Styles.Gray1();
        public static Styles.IStyle Gray2 => new Styles.Gray2();
        public static Styles.IStyle Light1 => new Styles.Light1();
        public static Styles.IStyle Light2 => new Styles.Light2();
        public static Styles.IStyle Monospace => new Styles.Monospace();
        public static Styles.IStyle Seaborn => new Styles.Seaborn();

        /// <summary>
        /// Return an array containing every available style
        /// </summary>
        public static Styles.IStyle[] GetStyles() => typeof(Style)
            .GetProperties()
            .Select(x => x.GetValue(typeof(Style)))
            .Select(x => (Styles.IStyle)x)
            .ToArray();
    }

}
