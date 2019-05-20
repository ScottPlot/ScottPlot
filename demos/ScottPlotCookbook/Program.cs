using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotCookbook
{
    class Program
    {
        static void Main(string[] args)
        {
            CookBookManager cookbook = new CookBookManager();
            cookbook.GenerateAllFigures(640, 480);

            //Console.WriteLine("\npress ENTER to exit...");
            //Console.ReadLine();
        }
    }
}
