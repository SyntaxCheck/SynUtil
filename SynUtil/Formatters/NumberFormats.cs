using System;

namespace SynUtil.Formatters
{
    public static class NumberFormats
    {
        public static string LargeNumberToReadableText(long num)
        {
            string formatted = String.Empty;

            if (num < 999)
            {
                formatted = num.ToString();
            }
            else if (num >= 1000 && num < 10000)
            {
                formatted = Math.Round(num / 1000.0, 2).ToString("#.##") + "k";
            }
            else if (num >= 10000 && num < 999999)
            {
                formatted = Math.Round(num / 1000.0, 0).ToString("#.##") + "k";
            }
            else if (num >= 1000000 && num < 10000000)
            {
                formatted = Math.Round(num / 1000000.0, 2).ToString("#.##") + "m";
            }
            else if (num >= 10000000 && num < 999999999)
            {
                formatted = Math.Round(num / 1000000.0, 0).ToString("#.##") + "m";
            }
            else if (num >= 1000000000 && num < 10000000000)
            {
                formatted = Math.Round(num / 1000000000.0, 2).ToString("#.##") + "b";
            }
            else if (num >= 10000000000 && num < 999999999999)
            {
                formatted = Math.Round(num / 1000000000.0, 0).ToString("#.##") + "b";
            }
            else if (num >= 1000000000000 && num < 10000000000000)
            {
                formatted = Math.Round(num / 1000000000000.0, 2).ToString("#.##") + "t";
            }
            else if (num >= 10000000000000 && num < 999999999999999)
            {
                formatted = Math.Round(num / 1000000000000.0, 0).ToString("#.##") + "t";
            }
            else
            {
                formatted = num.ToString();
            }

            return formatted;
        }
    }
}
