using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{

    //    Find ‘nice’ places to put ticks tick marks for numeric data spanning from lo to hi.
    //    If inside is True, then the nice range will be contained within the input range.
    //    If inside is False, then the nice range will contain the input range.
    //    To find nice numbers for time data, use nice_time_ticks. (Not implemented yet)

    //    The result is a tuple containing the minimum value of the nice range, 
    //    the maximum value of the nice range, and an array of double over the tick marks.

    public class Nice
    {

        static readonly double[] nice_intervals = { 1.0, 2.0, 2.5, 3.0, 5.0, 10.0 };
        //readonly double[] int_intervals = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 8.0, 10.0 };
        //readonly double[] int_12_intervals = {1.0, 2.0, 3.0, 4.0, 6.0, 12.0 };
        //readonly double[] int_60_intervals = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 10.0, 12.0, 15.0, 20.0, 30.0 };

        static public double nice_ceil(double x, double[] intervals, double basee = 10.0)
        {
            if (intervals == null)
                intervals = nice_intervals;
            if (x == 0)
                return 0;
            if (x < 0)
                return nice_floor(x * -1, intervals, basee) * -1;
            double z = Math.Pow(basee, Math.Floor(Math.Log(x, basee)));
            double result = 0;
            for (int i = 0; i < intervals.Length; i++)
                result = intervals[i] * z;
            if (x <= result)
                return result;
            return intervals[intervals.Length - 1] * z;
        }
        static public double nice_floor(double x, double[] intervals, double basee = 10.0)
        {
            if (intervals == null)
                intervals = nice_intervals;
            if (x == 0)
                return 0;
            if (x < 0)
                return nice_ceil(x * -1, intervals, basee * -1);
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
        static public double nice_round(double x, double[] intervals, double basee = 10.0)
        {
            if (intervals == null)
                intervals = nice_intervals;
            if (x == 0)
                return 0;
            double z = Math.Pow(basee, Math.Ceiling(Math.Log(x, basee)) - 1.0);
            double r = x / z;
            double result ;
            double cutoff ;
            for (int i = 0; i < intervals.Length-1; i++)
            {
                result = intervals[i] * z;
                cutoff = (result + intervals[i + 1] * z) / 2.0;
                if (x <= cutoff)
                    return result;
            }
            return intervals[intervals.Length - 1] * z;

        }

        //"""
        //Find 'nice' places to put* ticks* tick marks for numeric data
        //spanning from *lo* to *hi*.  If* inside* is ``True``, then the
        //nice range will be contained within the input range.If* inside*
        //is ``False``, then the nice range will contain the input range.
        //To find nice numbers for time data, use :func:`nice_time_ticks`.

        //The result is a tuple containing the minimum value of the nice
        //range, the maximum value of the nice range, and an iterator over
        //the tick marks.

        //See also :func:`nice_ticks_seq`.
        //"""
        static public Tuple<double, double, double[]> nice_ticks(double lo, double hi,  double[] intervals = null, int ticks = 5, bool inside = false,  double basee = 10.0)
        {
            if (intervals == null)
                intervals = nice_intervals;
            if (lo > hi)
                throw new Exception("Low value greater than high value.");
            double delta_x = hi - lo;
            if (delta_x == 0)
            {
                lo = nice_floor(lo, intervals, basee);
                hi = nice_ceil(hi, intervals, basee);
                delta_x = hi - lo;
                if (delta_x == 0)
                {
                    lo = lo - 0.5;
                    hi = hi + 0.5;
                    delta_x = hi - lo;
                }
            }

            //double nice_delta_x = nice_ceil(delta_x, intervals, basee);
            double delta_t = nice_round(delta_x / (ticks - 1), intervals, basee);
            double lo_t;
            double hi_t;
            if (inside)
            {
                 lo_t = Math.Ceiling(lo / delta_t) * delta_t;
                 hi_t = Math.Floor(hi / delta_t) * delta_t;
            }
            else
            {
                 lo_t = Math.Floor(lo / delta_t) * delta_t;
                 hi_t = Math.Ceiling(hi / delta_t) * delta_t;

            }

            double[] t_iter = { };
            double t = lo_t;
            while (t <= hi_t)
            {
                Array.Resize(ref t_iter, t_iter.Length + 1);
                t_iter[t_iter.Length-1] = t;
                t = t + delta_t;
            }
            return new Tuple<double, double, double[]> (lo_t, hi_t, t_iter);

        }

        //"""
        //A convenience wrapper of :func:`nice_ticks` to return the nice
        //range as a sequence.
        //"""
        static public double[] nice_ticks_seq(double lo, double hi, double[] intervals = null, int ticks = 5, bool inside = false, double basee = 10.0)
        {
            return (nice_ticks(lo, hi, intervals, ticks, inside, basee).Item3);
        }

    }
}
