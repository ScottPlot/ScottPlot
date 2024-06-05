using ScottPlot.Styles;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ScottPlot
{
    public static class Style
    {
        public static IStyle Black => new Black();
        public static IStyle Blue1 => new Blue1();
        public static IStyle Blue2 => new Blue2();
        public static IStyle Blue3 => new Blue3();
        public static IStyle Burgundy => new Burgundy();
        public static IStyle Control => new Styles.Control();
        public static IStyle Default => new Default();
        public static IStyle Gray1 => new Gray1();
        public static IStyle Gray2 => new Gray2();
        public static IStyle Hazel => new Hazel();
        public static IStyle Light1 => new Light1();
        public static IStyle Light2 => new Light2();
        public static IStyle Monospace => new Monospace();
        public static IStyle Pink => new Pink();
        public static IStyle Seaborn => new Seaborn();

        /// <summary>
        /// Return an array containing every available style
        /// </summary>
        public static IStyle[] GetStyles()
        {
#if NET5_0_OR_GREATER
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetInterfaces().Contains(typeof(IStyle)))
                .Select(x => (IStyle)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(x))
                .ToArray();
#else
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetInterfaces().Contains(typeof(IStyle)))
                .Select(x => (IStyle)FormatterServices.GetUninitializedObject(x))
                .ToArray();
#endif
        }
    }

}
