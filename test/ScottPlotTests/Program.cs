using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var txt = new ScottPlotTester();
            txt.RunAllTests();
        }
    }
}
