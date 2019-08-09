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
        //all comments to be stripped out when done
        //right click on output window and uncheck "Exception messages" to clean output
        {

            double bigNumber = Math.Pow(23.456, 9);
            double smallNumber = Math.Pow(23.456, -9);

            DisplayTicks(-bigNumber, bigNumber);
            DisplayTicks(9876 * bigNumber, 9877 * bigNumber);
            DisplayTicks(-smallNumber, smallNumber);
            DisplayTicks(9876 * smallNumber, 9877 * smallNumber);

            DisplayTicks(-10, 10);
            DisplayTicks(-50, 50);
            DisplayTicks(-77, 123);
            DisplayTicks(-12345678, -12345678 + 1);
            DisplayTicks(-1234567812345678, -1234567812345678 + 1);
            DisplayTicks(.00000010, .00000011);
            DisplayTicks(.0000001000001, .0000001000002); //does not render correctly
            DisplayTicks(100000001, 200000001);

            DisplayTicks(new DateTime(1492, 10, 12), new DateTime(1776, 10, 3)); //years
            DisplayTicks(new DateTime(1915, 1, 1), new DateTime(1918, 1, 1));  //months
            DisplayTicks(new DateTime(2019, 6, 1, 6, 0, 0), new DateTime(2019, 6, 1, 11, 12, 15)); //hours
            DisplayTicks(new DateTime(2019, 6, 1, 6, 0, 0), new DateTime(2019, 6, 1, 6, 12, 15)); //minutes
            DisplayTicks(new DateTime(2019, 6, 1, 6, 12, 0), new DateTime(2019, 6, 1, 6, 12, 15)); //seconds
            DisplayTicks(DateTime.Now, DateTime.Now);

        }

        static void DisplayTicks(double low, double high)
        {
            var ticks = new ScottPlot.TickCollection(low, high);
            Console.WriteLine(ticks);
        }

        static void DisplayTicks(DateTime low, DateTime high)
        {
            var ticks = new ScottPlot.TickCollection(low, high);
            Console.WriteLine(ticks);
        }

    }
}
