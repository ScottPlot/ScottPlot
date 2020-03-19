using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Experimental
{
    /* this file allows for development of experimental classes to make bar plots */

    class BarGraphTests
    {
        [Test]
        public void Test_ExperimentalBar_SingleSeries()
        {
            double[,] values = new double[,] { { 7, 12, 40, 40, 100, 125, 172, 550, 560, 600, 2496, 2789 } };
            string[] rowLabels = new string[] { null };
            string[] colLabels = new string[] { "ant", "bird", "mouse", "human", "cat", "dog", "frog", "lion", "elephant", "horse", "shark", "hippo" };

            Assert.AreEqual(values.GetLength(0), rowLabels.Length);
            Assert.AreEqual(values.GetLength(1), colLabels.Length);

            Console.WriteLine("How often do you read online reviews?");
            Console.WriteLine();

            Console.WriteLine("Brain-to-Body Mass Ratio:");
            for (int colIndex = 0; colIndex < values.GetLength(1); colIndex++)
            {
                double value = values[0, colIndex];
                Console.WriteLine($"  {colLabels[colIndex]} = {value}");
            }
        }

        [Test]
        public void Test_ExperimentalBar_MultiSeries()
        {
            double[,] values = new double[,] { { 15, 22, 45, 17 }, { 37, 21, 29, 13 } };
            string[] rowLabels = new string[] { "men", "women" };
            string[] colLabels = new string[] { "always", "regularly", "sometimes", "never" };

            Assert.AreEqual(values.GetLength(0), rowLabels.Length);
            Assert.AreEqual(values.GetLength(1), colLabels.Length);

            Console.WriteLine("How often do you read online reviews?");
            Console.WriteLine();

            for (int colIndex = 0; colIndex < values.GetLength(1); colIndex++)
            {
                Console.WriteLine($"Group: {colLabels[colIndex]}");
                for (int rowIndex = 0; rowIndex < values.GetLength(0); rowIndex++)
                {
                    double value = values[rowIndex, colIndex];
                    Console.WriteLine($"  {rowLabels[rowIndex]} = {value}");
                }
                Console.WriteLine();
            }
        }
    }
}
