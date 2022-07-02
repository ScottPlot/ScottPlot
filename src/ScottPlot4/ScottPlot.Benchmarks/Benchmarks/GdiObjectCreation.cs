using BenchmarkDotNet.Attributes;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlotTests.Performance
{
    [SimpleJob(launchCount: 2, warmupCount: 1, targetCount: 3)]
    [IterationTime(200)]
    [MemoryDiagnoser]
    public class GdiObjectCreation
    {
        [Benchmark]
        public void CreateAndDestroy_Brush()
        {
            var item = new SolidBrush(Color.Black);
            item.Dispose();
        }

        [Benchmark]
        public void CreateAndDestroy_Pen()
        {
            var item = new Pen(Color.Black, 3);
            item.Dispose();
        }

        [Benchmark]
        public void CreateAndDestroy_Font()
        {
            var item = new Font(FontFamily.GenericSansSerif, 16);
            item.Dispose();
        }

        [Benchmark]
        public void CreateAndDestroy_FontFamily()
        {
            var item = new FontFamily("Arial");
            item.Dispose();
        }

        [Benchmark]
        public void CreateAndDestroy_StringFormat()
        {
            var item = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            item.Dispose();
        }
    }
}
