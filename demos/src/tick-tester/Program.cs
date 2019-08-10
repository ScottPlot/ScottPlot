using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tick_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayTicksForRange(-10, 10);
            DisplayTicksForRange(-50, 50);
            DisplayTicksForRange(-77, 123);
            DisplayTicksForRange(-12345678, -12345678+1);
            DisplayTicksForRange(-1234567812345678, -1234567812345678 + 1);
            DisplayTicksForRange(.00000010, .00000011);
        }

        static void DisplayTicksForRange(double low, double high)
        {
            double[] tickPositions = ScottPlot.TicksExperimental.GetTicks(low, high);
            Console.Write($"{tickPositions.Length} ticks in [{low} - {high}]: ");
            foreach (double tickPosition in tickPositions)
                Console.Write(tickPosition + " ");
            Console.WriteLine();
        }
    }
}
