using System;

namespace SynUtil.Formatters
{
    public static class TimeSpanFormatter
    {
        public static string TimeSpanFormattedFull(TimeSpan ts)
        {
            string timespFormatted = String.Empty;

            if (ts.Days > 0)
            {
                timespFormatted += ts.Days.ToString() + " day" + AppendS(ts.Days);
            }
            if (ts.Days > 0 || ts.Hours > 0)
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Hours.ToString() + " hour" + AppendS(ts.Hours);
            }
            if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0)
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Minutes.ToString() + " minute" + AppendS(ts.Minutes);
            }
            if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0)
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Seconds.ToString() + " second" + AppendS(ts.Seconds);
            }
            if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 || ts.Milliseconds > 0)
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Milliseconds.ToString() + " millisecond" + AppendS(ts.Milliseconds);
            }

            return timespFormatted;
        }
        public static string TimeSpanHighestDecimal(TimeSpan timeSpan)
        {
            string unit = String.Empty;
            double num = 0;

            if (timeSpan.TotalDays >= 1)
            {
                unit = "day";
                num = timeSpan.TotalDays;
            }
            else if (timeSpan.TotalHours >= 1)
            {
                unit = "hour";
                num = timeSpan.TotalHours;
            }
            else if (timeSpan.TotalMinutes >= 1)
            {
                unit = "minute";
                num = timeSpan.TotalMinutes;
            }
            else if (timeSpan.TotalSeconds >= 1)
            {
                unit = "second";
                num = timeSpan.TotalSeconds;
            }
            else if (timeSpan.TotalMilliseconds >= 1)
            {
                unit = "millisecond";
                num = timeSpan.TotalMilliseconds;
            }

            if (num > 0)
            {
                return Math.Round(num, 2).ToString() + " " + unit + AppendS(num);
            }

            return String.Empty;
        }
        private static string AppendS(double number)
        {

            if (number != 1.0)
                return "s";

            return String.Empty;
        }
    }
}
