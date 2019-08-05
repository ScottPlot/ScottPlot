/* EXPERIMENTAL TICK CODE
 * Code here relates to the generation of tick mark positions and labels.
 */

using System;

namespace ScottPlot
{
    public class TicksExperimental
    {
        private static double[] intervals = { 1.0, 2.0, 2.5, 3.0, 5.0, 10.0 };

        public static double[] GetTicks(double lo, double hi, int ticks = 5, double basee = 10.0)
        {

            if (lo > hi)
                throw new Exception("Low value greater than high value.");

            double delta_x = hi - lo;
            if (delta_x == 0)
            {
                lo = GetFloor(lo, intervals, basee);
                hi = GetCeiling(hi, intervals, basee);
                delta_x = hi - lo;
                if (delta_x == 0)
                {
                    lo = lo - 0.5;
                    hi = hi + 0.5;
                    delta_x = hi - lo;
                }
            }

            double delta_t = GetRound(delta_x / (ticks - 1), intervals, basee);
            double lo_t;
            double hi_t;
            bool ticksMustBeBewtweenLowAndHigh = true;
            if (ticksMustBeBewtweenLowAndHigh)
            {
                lo_t = Math.Ceiling(lo / delta_t) * delta_t;
                hi_t = Math.Floor(hi / delta_t) * delta_t;
            }
            else
            {
                lo_t = Math.Floor(lo / delta_t) * delta_t;
                hi_t = Math.Ceiling(hi / delta_t) * delta_t;

            }

            double[] tickPositions = { };
            double t = lo_t;
            while (t <= hi_t)
            {
                Array.Resize(ref tickPositions, tickPositions.Length + 1);
                tickPositions[tickPositions.Length - 1] = t;
                t = t + delta_t;
            }

            return tickPositions;
        }

        private static double GetCeiling(double x, double[] intervals, double basee = 10.0)
        {
            if (intervals == null)
                intervals = TicksExperimental.intervals;
            if (x == 0)
                return 0;
            if (x < 0)
                return GetFloor(x * -1, intervals, basee) * -1;
            double z = Math.Pow(basee, Math.Floor(Math.Log(x, basee)));
            double result = 0;
            for (int i = 0; i < intervals.Length; i++)
                result = intervals[i] * z;
            if (x <= result)
                return result;
            return intervals[intervals.Length - 1] * z;
        }

        private static double GetFloor(double x, double[] intervals, double basee = 10.0)
        {
            if (intervals == null)
                intervals = TicksExperimental.intervals;
            if (x == 0)
                return 0;
            if (x < 0)
                return GetCeiling(x * -1, intervals, basee * -1);
            double z = Math.Pow(basee, Math.Ceiling(Math.Log(x, basee)) - 1.0);
            double r = x / z;
            for (int i = intervals.Length - 1; i > 0; i--)
            {
                double result = intervals[i] * z;
                if (x >= result)
                    return result;
            }
            return intervals[0] * z;
        }

        private static double GetRound(double x, double[] intervals, double basee = 10.0)
        {
            if (intervals == null)
                intervals = TicksExperimental.intervals;
            if (x == 0)
                return 0;
            double z = Math.Pow(basee, Math.Ceiling(Math.Log(x, basee)) - 1.0);
            double r = x / z;
            double result;
            double cutoff;
            for (int i = 0; i < intervals.Length - 1; i++)
            {
                result = intervals[i] * z;
                cutoff = (result + intervals[i + 1] * z) / 2.0;
                if (x <= cutoff)
                    return result;
            }
            return intervals[intervals.Length - 1] * z;

        }

    }
}
