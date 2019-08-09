using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    public class IncorrectTime : Exception
    {
    }
    public class TickCalculator
    {
        private static double[] intervals = { 1.0, 2.0, 2.5, 5.0, 10.0 };
        private static double[] int_intervals = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 8.0, 10.0 };
        private static double[] int_12_intervals = { 1.0, 2.0, 3.0, 4.0, 6.0, 12.0 };
        private static double[] int_60_intervals = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 10.0, 12.0, 15.0, 20.0, 30.0 };

        public static double[] GetTicks(double lo, double hi, int ticks = 5, double[] intervals = null, double basee = 10.0)
        {

            if (intervals == null)
                intervals = TickCalculator.intervals;

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

            double t = lo_t;
            double[] ticksPosition = new double[] { };
            while (t <= hi_t)
            {
                Array.Resize(ref ticksPosition, ticksPosition.Length + 1);
                ticksPosition[ticksPosition.Length - 1] = t;
                t += delta_t;
            }

            return ticksPosition;
        }
        public static void GetMantissasExponentOffset(double[] tickPositions, out double[] tickPositionsMantissas, out int tickPositionsExponent, out double offset)
        {
            tickPositionsMantissas = new double[] { };
            int[] Exponents = new int[] { };
            Array.Resize<double>(ref tickPositionsMantissas, tickPositions.Length);
            Array.Resize<int>(ref Exponents, tickPositions.Length);
            char[] charseparator = new char[] { 'E' };


            string tick;
            for (int i = 0; i < tickPositions.Length; i++)
            {
                tick = (tickPositions[i]).ToString("E3");
                string[] result = tick.Split(charseparator);
                tickPositionsMantissas[i] = double.Parse(result[0]);
                Exponents[i] = int.Parse(result[1]);
            }

            //Make sure Exponents are all the same within the tickPositions and Exponent > 3
            tickPositionsExponent = Exponents.Max();
            if (Math.Abs(tickPositionsExponent) > 3)
            {
                if (Exponents.Max() != Exponents.Min())
                {
                    for (int i = 0; i < tickPositions.Length; i++)
                    {
                        tickPositionsMantissas[i] = Math.Round((tickPositions[i]) / Math.Pow(10,tickPositionsExponent), 3);
                    }
                }
            }
            else
            {
                tickPositionsExponent = 0;
                for (int i = 0; i < tickPositions.Length; i++)
                {
                    tickPositionsMantissas[i] = Math.Round(tickPositions[i], 3);
                }
            }

            //Check if offset is needed
            if (Math.Round(tickPositionsMantissas.Max(), 2) - Math.Round(tickPositionsMantissas.Min(), 2) == 0)
            {
                offset = tickPositions.Min();
                //for (int i = 0; i < tickPositions.Length; i++)
                //{
                //    tickPositionsMantissas[i] = Math.Round((tickPositions[i] - offset) / Math.Pow(10,tickPositionsExponent), 3);
                //}
                while (Math.Round(tickPositionsMantissas.Max(),2) - Math.Round(tickPositionsMantissas.Min(),2) == 0)
                {
                    for (int i = 0; i < tickPositionsMantissas.Length; i++)
                    {
                        tickPositionsMantissas[i]= (tickPositions[i]-offset)/Math.Pow(10,tickPositionsExponent);
                    }
                    tickPositionsExponent -= 1;
                }

                for (int i = 0; i < tickPositionsMantissas.Length; i++)
                {
                    tickPositionsMantissas[i] = Math.Round(tickPositionsMantissas[i], 3);
                }


            }

            else
            {
                offset = 0;
            }
        }

        private static double GetCeiling(double x, double[] intervals, double basee = 10.0)
        {
            if (intervals == null)
                intervals = TickCalculator.intervals;
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
                intervals = TickCalculator.intervals;
            if (x == 0)
                return 0;
            if (x < 0)
                return GetCeiling(x * -1, intervals, basee * -1);
            double z = Math.Pow(basee, Math.Ceiling(Math.Log(x, basee)) - 1.0);
            //double r = x / z;
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
                intervals = TickCalculator.intervals;
            if (x == 0)
                return 0;
            double z = Math.Pow(basee, Math.Ceiling(Math.Log(x, basee)) - 1.0);
            //double r = x / z;
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

            return GetTicks(lo_year, hi_year, ticks, intervals: int_intervals, basee);
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

            return GetTicks(lo_month, delta_year * 12 + hi_month, ticks, intervals: int_12_intervals, basee);
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

            return GetTicks(0, delta_weeks, ticks, intervals: int_intervals, basee);
        }

        private static double[] GetDayTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_day = myCalendar.GetDayOfYear(lo);
            int hi_day = myCalendar.GetDayOfYear(hi);

            if (hi.Day > hi_day)
                hi_day += 1;

            int delta_days = hi_day - lo_day;

            return GetTicks(0, delta_days, ticks, intervals: int_intervals, basee);
        }

        private static double[] GetHourTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_hour = myCalendar.GetHour(lo);
            int hi_hour = myCalendar.GetHour(hi);

            if (hi.Hour > hi_hour)
                hi_hour += 1;

            int delta_hours = hi_hour - lo_hour;

            return GetTicks(0, delta_hours, ticks, intervals: int_12_intervals, basee: 24.0);
        }

        private static double[] GetMinuteTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 60.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_minute = myCalendar.GetMinute(lo);
            int hi_minute = myCalendar.GetMinute(hi);

            if (hi.Minute > hi_minute)
                hi_minute += 1;

            int delta_minute = hi_minute - lo_minute;

            return GetTicks(0, delta_minute, ticks, intervals: int_60_intervals, basee: 60.0);
        }

        private static double[] GetSecondTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 60.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_second = myCalendar.GetSecond(lo);
            int hi_second = myCalendar.GetSecond(hi);

            if (hi.Second > hi_second)
                hi_second += 1;

            int delta_second = hi_second - lo_second;

            return GetTicks(0, delta_second, ticks, intervals: int_60_intervals, basee: 60.0);
        }

        private static double[] GetMillisecondTicks(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            Calendar myCalendar = CultureInfo.InvariantCulture.Calendar;
            int lo_millisecond = (int)myCalendar.GetMilliseconds(lo);
            int hi_millisecond = (int)myCalendar.GetMilliseconds(hi);

            if (hi.Millisecond > hi_millisecond)
                hi_millisecond += 1;

            int delta_millisecond = hi_millisecond - lo_millisecond;

            return GetTicks(0, delta_millisecond, ticks, intervals: int_intervals, basee: 10.0);
        }

        public static double[] GetTicksForTime(DateTime lo, DateTime hi, int ticks = 5, double basee = 10.0)
        {
            TimeSpan span = hi.Subtract(lo);
            if (span.TotalDays / 365 > ticks)
                return GetYearTicks(lo, hi, ticks, basee);
            else if (span.TotalDays / 365 * 12 > ticks)
                return GetMonthTicks(lo, hi, ticks, basee);
            else if (span.TotalDays / 7 > ticks)
                return GetWeekTicks(lo, hi, ticks, basee);
            else if (span.TotalDays > ticks)
                return GetDayTicks(lo, hi, ticks, basee);
            else if (span.TotalHours > ticks)
                return GetHourTicks(lo, hi, ticks, basee);
            else if (span.TotalMinutes > ticks)
                return GetMinuteTicks(lo, hi, ticks, basee);
            else if (span.TotalSeconds > ticks)
                return GetSecondTicks(lo, hi, ticks, basee);
            else
                return GetMillisecondTicks(lo, hi, ticks, basee);
        }

        private static string GetSuperscript(int digit)
        {
            switch (digit)
            {
                case 0:
                    return "\x2070";

                case 1:
                    return "\x00B9";

                case 2:
                    return "\x00B2";

                case 3:
                    return "\x00B3";

                case 4:
                    return "\x2074";

                case 5:
                    return "\x2075";

                case 6:
                    return "\x2076";

                case 7:
                    return "\x2077";

                case 8:
                    return "\x2078";

                case 9:
                    return "\x2079";

                default:
                    return string.Empty;
            }
        }
        public static string GetMultiplierString(double offset, int exp)
        {

            if (Math.Abs(exp) > 3)
            {

                var sb = new StringBuilder();
                var sbOffset = new StringBuilder();
                bool isNegative = false;
                bool isOffsetNegative = false;

                if (exp < 0)
                {
                    isNegative = true;
                    exp = -exp;
                }
                while (exp != 0)
                {
                    sb.Insert(0, GetSuperscript(exp % 10));
                    exp /= 10;
                }

                if (isNegative)
                {
                    sb.Insert(0, "\x207B");
                }
                if (offset != 0)
                {
                    if (Math.Abs(offset) < 1000)
                    {
                        return Math.Round(offset, 3).ToString() + "+10" + sb.ToString();
                    }
                    else //if offset requires exponent
                    {
                        string offsetTick = (offset).ToString("E3");
                        char[] charseparator = new char[] { 'E' };
                        string[] result = offsetTick.Split(charseparator);
                        double offsetMantissa = double.Parse(result[0]);
                        int offsetExponent = int.Parse(result[1]);

                        if (offsetExponent < 0)
                        {
                            isOffsetNegative = true;
                            offsetExponent = -offsetExponent;
                        }

                        while (offsetExponent != 0)
                        {
                            sbOffset.Insert(0, GetSuperscript(offsetExponent % 10));
                            offsetExponent /= 10;
                        }

                        if (isOffsetNegative)
                        {
                            sbOffset.Insert(0, "\x207B");
                        }
                        return "x10" + sb.ToString() + Math.Round(offsetMantissa, 3).ToString() + "x10" + sbOffset.ToString();
                    }

                }
                else
                {
                    return "x10" + sb.ToString();
                }
            }
            if (offset != 0)
            {
                return offset.ToString();
            }
            return "";
        }
    }
}
