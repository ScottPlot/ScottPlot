using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Drawing.Colorsets
{
    public class Custom : HexColorset, IPalette
    {
        public override string[] hexColors { get; } = new string[] { };

        public Custom(string[] htmlColors)
        {
            if (htmlColors is null)
                throw new ArgumentNullException("must provide at least one color");

            if (htmlColors.Length == 0)
                throw new ArgumentException("must provide at least one color");

            hexColors = htmlColors;
        }
    }
}
