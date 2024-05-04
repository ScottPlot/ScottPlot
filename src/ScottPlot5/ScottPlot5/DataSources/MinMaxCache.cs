using System.Threading.Tasks;

using ScottPlot.DataSources;

namespace ScottPlot
{
    public class MinMaxCache
    {
        private readonly IReadOnlyList<double> Data;
        private readonly IReadOnlyList<SignalRangeY> Cache;

        public int CachePeriod { get; }

        public MinMaxCache(IReadOnlyList<double> data, int cachePeriod = 1000)
        {
            this.Data = data;
            this.CachePeriod = cachePeriod;
            var cacheSize = data.Count / cachePeriod;
            var cache = new SignalRangeY[cacheSize];
            this.Cache = cache;

            // Create MinMax caches in parallel at regular intervals
            Parallel.For(0, cacheSize, (i) =>
            {
                double min = double.PositiveInfinity;
                double max = double.NegativeInfinity;

                for (var j = 0; j < cachePeriod; j++)
                {
                    var sample = data[(i * cachePeriod) + j];
                    if (sample < min)
                        min = sample;
                    if (max < sample)
                        max = sample;
                }

                cache[i] = new SignalRangeY(min, max);
            });
        }

        public SignalRangeY GetMinMax(int start, int end)
        {
            var period = this.CachePeriod;

            double min = double.PositiveInfinity;
            double max = double.NegativeInfinity;

            // Calculate Start Index Alignment Offset
            var periodStartIndex = (start / period) + 1;
            var startOffsetCount = periodStartIndex * period;

            double sample;
            if (end - start < period || startOffsetCount >= end)
            {
                // If the requested index size is less than the cache period
                for (int i = start; i < end; i++)
                {
                    sample = this.Data[i];
                    if (sample < min)
                        min = sample;
                    if (max < sample)
                        max = sample;
                }
                return new SignalRangeY(min, max);
            }

            // Calculate End Index Alignment Offset
            var periodEndIndex = end / period;
            var endOffsetIndex = periodEndIndex * period;

            // Start ~ Period Start Index
            for (int i = start; i < startOffsetCount; i++)
            {
                sample = this.Data[i];
                if (sample < min)
                    min = sample;
                if (max < sample)
                    max = sample;
            }

            SignalRangeY minMaxSample;
            // Period Start Index ~ Period End Index
            for (int i = periodStartIndex; i < periodEndIndex; i++)
            {
                minMaxSample = this.Cache[i];
                if (minMaxSample.Min < min)
                    min = minMaxSample.Min;
                if (max < minMaxSample.Max)
                    max = minMaxSample.Max;
            }

            // Period End Index ~ End
            for (int i = endOffsetIndex; i < end; i++)
            {
                sample = this.Data[i];
                if (sample < min)
                    min = sample;
                if (max < sample)
                    max = sample;
            }

            return new SignalRangeY(min, max);
        }
    }
}
