using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public static class GlobalFont
    {
        /// <summary>
        ///     This method will check for several preffered fonts
        ///     to exist on the system and return the exact name.
        /// </summary>
        /// <returns></returns>
        public static string GetDefault()
        {
            foreach (FontFamily font in FontFamily.Families)
            {
                var fntu = font.Name.ToUpper();
                if (fntu.Contains("SEGOE") || fntu.Contains("DEJAVU") || fntu.Contains("SANS"))
                    return font.Name;
            }
            // Debug.WriteLine("No preffered Font found! Using any as fallback..");
            foreach (FontFamily font in FontFamily.Families)
            {
                return font.Name;
            }
            // uh oh
            throw new Exception("No Fonts found on the System!");
        }
    }
}
