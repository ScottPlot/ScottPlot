using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    /// <summary>
    /// Miscellaneous standalone functions
    /// </summary>
    public class Misc
    {
        /// <summary>
        /// write the start and finish of an array to the console
        /// </summary>
        /// <param name="data"></param>
        /// <param name="numberToShow"></param>
        public static void ArrayShow(double[] data, int numberToShow=10)
        {
            const string format = "[{0}] {1}";
            if (data.Length <= numberToShow)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    System.Console.WriteLine(format, i, data[i]);
                }
                return;
            }
            else
            {
                for (int i = 0; i < numberToShow/2; i++)
                {
                    System.Console.WriteLine(format, i, data[i]);
                }
                System.Console.WriteLine(" ... ");
                for (int i = data.Length-(numberToShow / 2); i < data.Length; i++)
                {
                    System.Console.WriteLine(format, i, data[i]);
                }
                return;
            }
        }
    }
}
