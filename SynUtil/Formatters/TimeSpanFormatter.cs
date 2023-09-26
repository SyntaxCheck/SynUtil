using System;

namespace SynUtil.Formatters
{
    public static class TimeSpanFormatter
    {
        public static string TimeSpanFormattedFull(TimeSpan ts)
        {
            return TimeSpanFormattedFull(ts, 5);
        }
        public static string TimeSpanFormattedFull(TimeSpan ts, int maxLevels)
        {
            return TimeSpanFormatted(ts, true, true, true, true, true, maxLevels);
        }
        public static string TimeSpanFormatted(TimeSpan ts, bool showDay, bool showHour, bool showMinute, bool showSecond, bool showMillisecond, int maxLevels)
        {
            string timespFormatted = String.Empty;
            int levelsShown = 0;

            if (showDay && levelsShown < maxLevels && ts.Days > 0)
            {
                timespFormatted += ts.Days.ToString() + " day" + AppendS(ts.Days);
                levelsShown++;
            }
            if (showHour && levelsShown < maxLevels && (ts.Days > 0 || ts.Hours > 0))
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Hours.ToString() + " hour" + AppendS(ts.Hours);
                levelsShown++;
            }
            if (showMinute && levelsShown < maxLevels && (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0))
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Minutes.ToString() + " minute" + AppendS(ts.Minutes);
                levelsShown++;
            }
            if (showSecond && levelsShown < maxLevels && (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0))
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Seconds.ToString() + " second" + AppendS(ts.Seconds);
                levelsShown++;
            }
            if (showMillisecond && levelsShown < maxLevels && (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 || ts.Milliseconds > 0))
            {
                if (!String.IsNullOrEmpty(timespFormatted))
                {
                    timespFormatted += ", ";
                }

                timespFormatted += ts.Milliseconds.ToString() + " millisecond" + AppendS(ts.Milliseconds);
                levelsShown++;
            }

            return timespFormatted;
        }
        public static string TimeSpanHighestDecimal(TimeSpan timeSpan)
        {
            return TimeSpanHighestDecimal(timeSpan, false);
        }
        public static string TimeSpanHighestDecimal(TimeSpan timeSpan, bool condensedNames)
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
                if(condensedNames)
                    unit = "min";
                else
                    unit = "minute";

                num = timeSpan.TotalMinutes;
            }
            else if (timeSpan.TotalSeconds >= 1)
            {
                if(condensedNames)
                    unit = "sec";
                else
                    unit = "second";

                num = timeSpan.TotalSeconds;
            }
            else if (timeSpan.TotalMilliseconds >= 1)
            {
                if(condensedNames)
                    unit = "ms";
                else
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
