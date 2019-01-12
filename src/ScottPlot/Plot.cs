using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Code here contains the highest level ScottPlot.
 * Efforts are made to minimize the complexity of what is exposed to the user.
 */

namespace ScottPlot
{
    public class Plot
    {
        public Settings settings;
        public Data data;
        public Figure figure;

        public Plot(int widthPx = 640, int heightPx = 480)
        {
            data = new Data();
            settings = new Settings(widthPx, heightPx, ref data);
            figure = new Figure(settings, data);
        }

        public override string ToString()
        {
            return $"ScottPlot ({settings.width}x{settings.height}) with {data.Count()} object(s)";
        }

    }
}