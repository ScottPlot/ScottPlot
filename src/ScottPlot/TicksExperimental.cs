/* EXPERIMENTAL TICK CODE
 * Code here relates to the generation of tick mark positions and labels.
 */

using System;
using System.Globalization;

namespace ScottPlot
{
    public class IncorrectTime : Exception
    {
    }
        public class TicksExperimental
    {
        private static double[] intervals = { 1.0, 2.0, 2.5, 3.0, 5.0, 10.0 };
        private static double[] int_intervals = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 8.0, 10.0 };
        private static double[] int_12_intervals = { 1.0, 2.0, 3.0, 4.0, 6.0, 12.0 };
        private static double[] int_60_intervals = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 10.0, 12.0, 15.0, 20.0, 30.0 };



        public static double[] GetTicks(double lo, double hi, int ticks = 5, double[] intervals = null, double basee = 10.0)
        {

            if (intervals == null)
                intervals = TicksExperimental.intervals;

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
                    lo -= 0.5;
                    hi += 0.5;
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
                t += delta_t;
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

        private static double[] GetYearTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            int lo_year = lo.Year;
            int hi_year = hi.Year;

            if (hi > new DateTime(hi_year, 1, 1))
                hi_year += 1;
            if (hi_year - lo_year < (ticks - 1))
                throw new IncorrectTime();
            double[] result = GetTicks(lo_year, hi_year, ticks, intervals: int_intervals, basee);

            return result;
        }

        private static double[] GetMonthTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            int lo_year = lo.Year;
            int hi_year = hi.Year;
            int lo_month = lo.Month;
            int hi_month = hi.Month;

            if (hi > new DateTime(hi_year, hi_month, 1))
                if (hi_month == 11)
                {
                    hi_month = 0;
                    hi_year += 1;
                }
                else
                {
                    hi_month += 1;
                }
            int delta_year = hi_year - lo_year;
            if (delta_year * 12 + hi_month - lo_month < (ticks - 1))
                throw new IncorrectTime();

            double[] result = GetTicks(lo_month, delta_year * 12 + hi_month, ticks, intervals: int_12_intervals, basee);

            return result;
        }

        private static double[] GetWeekTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_week = myCalendar.GetWeekOfYear(lo, rule: CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek: DayOfWeek.Monday);
            int hi_week = myCalendar.GetWeekOfYear(hi, rule: CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek: DayOfWeek.Monday);

            int ts_lo = lo.Subtract(new DateTime(1970, 01, 01)).Days;
            int ts_hi = hi.Subtract(new DateTime(1970, 01, 01)).Days;

            lo_week += ts_lo / 7;
            hi_week += ts_hi / 7;

            int delta_weeks = hi_week - lo_week;
            if (delta_weeks < (ticks - 1))
                throw new IncorrectTime();

            double[] result = GetTicks(0, delta_weeks, ticks, intervals: int_intervals, basee);

            return result;
        }

        private static double[] GetDayTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_day = myCalendar.GetDayOfYear(lo);
            int hi_day = myCalendar.GetDayOfYear(hi);

            if (hi.Day > hi_day)
                hi_day += 1;

            int delta_days = hi_day - lo_day;
            if (delta_days < (ticks - 1))
                throw new IncorrectTime();

            double[] result = GetTicks(0, delta_days, ticks, intervals: int_intervals, basee);

            return result;
        }

        private static double[] GetHourTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_hour = myCalendar.GetHour(lo);
            int hi_hour = myCalendar.GetHour(hi);

            if (hi.Hour > hi_hour)
                hi_hour += 1;

            int delta_hours = hi_hour - lo_hour;
            if (delta_hours < (ticks - 1))
                throw new IncorrectTime();

            double[] result = GetTicks(0, delta_hours, ticks, intervals: int_12_intervals, basee: 24.0);

            return result;
        }

        private static double[] GetMinuteTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 60.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_minute = myCalendar.GetMinute(lo);
            int hi_minute = myCalendar.GetMinute(hi);

            if (hi.Minute > hi_minute)
                hi_minute += 1;

            int delta_minute = hi_minute - lo_minute;
            if (delta_minute < (ticks - 1))
                throw new IncorrectTime();

            double[] result = GetTicks(0, delta_minute, ticks, intervals: int_60_intervals, basee: 60.0);

            return result;
        }

        private static double[] GetSecondTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 60.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_second = myCalendar.GetSecond(lo);
            int hi_second = myCalendar.GetSecond(hi);

            if (hi.Second > hi_second)
                hi_second += 1;

            int delta_second = hi_second - lo_second;
            if (delta_second < (ticks - 1))
                throw new IncorrectTime();

            double[] result = GetTicks(0, delta_second, ticks, intervals: int_60_intervals, basee: 60.0);

            return result;
        }

        private static double[] GetMillisecondTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_millisecond = (int)myCalendar.GetMilliseconds(lo);
            int hi_millisecond = (int)myCalendar.GetMilliseconds(hi);

            if (hi.Millisecond > hi_millisecond)
                hi_millisecond += 1;

            int delta_millisecond = hi_millisecond - lo_millisecond;
            if (delta_millisecond < (ticks - 1))
                throw new ArgumentException();

            double[] result = GetTicks(0, delta_millisecond, ticks, intervals: int_intervals, basee: 10.0);

            return result;
        }



        public static double[] GetTicksForTime(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            try
            {
                return GetYearTicks(lo, hi, ticks, basee);
            }
            catch (IncorrectTime)
            {
                try
                {
                    return GetMonthTicks(lo, hi, ticks, basee);
                }
                catch (IncorrectTime)
                {
                    try
                    {
                        return GetWeekTicks(lo, hi, ticks, basee);
                    }
                    catch (IncorrectTime)
                    {
                        try
                        {
                            return GetDayTicks(lo, hi, ticks, basee);
                        }
                        catch (IncorrectTime)
                        {
                            try
                            {
                                return GetHourTicks(lo, hi, ticks, basee);
                            }
                            catch (IncorrectTime)
                            {
                                try
                                {
                                    return GetMinuteTicks(lo, hi, ticks, basee);
                                }
                                catch (IncorrectTime)
                                {
                                    try
                                    {
                                        return GetSecondTicks(lo, hi, ticks, basee);
                                    }
                                    catch (IncorrectTime)
                                    {
                                        try
                                        {
                                            return GetMillisecondTicks(lo, hi, ticks, basee);
                                        }
                                        catch (ArgumentException)
                                        {
                                            throw new ArgumentException();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
