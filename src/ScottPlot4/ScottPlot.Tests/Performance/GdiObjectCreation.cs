using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.Performance
{
    class GdiObjectCreation
    {
        readonly int repetitions = 100_000;

        [Test]
        public void Test_CreateAndDestroyBenchmark_Brush()
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
            {
                var item = new SolidBrush(Color.Black);
                item.Dispose();
            }
            sw.Stop();
            double usElapsed = 1e6 * sw.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine($"Mean create and destroy time: {usElapsed / repetitions} µs"); // 0.71669 µs
        }

        [Test]
        public void Test_CreateAndDestroyBenchmark_Pen()
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
            {
                var item = new Pen(Color.Black, 3);
                item.Dispose();
            }
            sw.Stop();
            double usElapsed = 1e6 * sw.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine($"Mean create and destroy time: {usElapsed / repetitions} µs"); // 0.360778 µs
        }

        [Test]
        public void Test_CreateAndDestroyBenchmark_Font()
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
            {
                var item = new Font(FontFamily.GenericSansSerif, 16);
                item.Dispose();
            }
            sw.Stop();
            double usElapsed = 1e6 * sw.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine($"Mean create and destroy time: {usElapsed / repetitions} µs"); // 0.596981 µs
        }

        [Test]
        public void Test_CreateAndDestroyBenchmark_FontFamily()
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
            {
                var item = new FontFamily("Arial");
                item.Dispose();
            }
            sw.Stop();
            double usElapsed = 1e6 * sw.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine($"Mean create and destroy time: {usElapsed / repetitions} µs"); // 0.163119 µs
        }

        [Test]
        public void Test_CreateAndDestroyBenchmark_StringFormat()
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < repetitions; i++)
            {
                var item = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                item.Dispose();
            }
            sw.Stop();
            double usElapsed = 1e6 * sw.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine($"Mean create and destroy time: {usElapsed / repetitions} µs"); // 0.232935 µs
        }
    }
}
