using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot_Sandbox_Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var plt = new ScottPlot.Plot(640, 480);
            Console.WriteLine(plt);

            Console.WriteLine("\npress ENTER to exit...");
            Console.ReadLine();
        }
    }
}
