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
        public static string NumFormatLargeAndDecimalsCurrency(double num)
        {
            string rtn = NumFormatLargeAndDecimals(num, true);
            if (rtn != "N/A")
                return "$" + rtn;
            else
                return rtn;
        }
        public static string NumFormatLarge(double num)
        {
            return NumFormatLargeAndDecimals(Math.Round(num, 0), false);
        }
        public static string NumFormatLargeAndDecimals(double num)
        {
            return NumFormatLargeAndDecimals(num, true);
        }
        public static string NumFormatLargeAndDecimals(double num, bool decimals)
        {
            string formatted = String.Empty;
            bool isNegative = false;

            if (num == -1d)
            {
                return "N/A";
            }

            if (num < 0)
            {
                num = Math.Abs(num);
                isNegative = true;
            }

            if (num == 0)
            {
                formatted = num.ToString();
            }
            else if (num < 999)
            {
                if (decimals)
                {
                    if (num >= 10)
                    {
                        formatted = Math.Round(num, 0).ToString();
                    }
                    else if (num >= 1)
                    {
                        formatted = Math.Round(num, 2).ToString("#.00");
                    }
                    else if (num >= 0.01)
                    {
                        formatted = Math.Round(num, 3).ToString("0.000");
                    }
                    else if (num >= 0.001)
                    {
                        formatted = Math.Round(num, 4).ToString("0.0000");
                    }
                    else if (num >= 0.0001)
                    {
                        formatted = Math.Round(num, 5).ToString("0.00000");
                    }
                    else
                    {
                        formatted = Math.Round(num, 6).ToString("0.000000");
                    }
                }
                else
                {
                    formatted = num.ToString();
                }
            }
            else if (num >= 1000 && num < 10000)
            {
                formatted += Math.Round(num / 1000.0, 2).ToString("#.##") + "k";
            }
            else if (num >= 10000 && num < 999999)
            {
                formatted += Math.Round(num / 1000.0, 0).ToString("#.##") + "k";
            }
            else if (num >= 1000000 && num < 10000000)
            {
                formatted += Math.Round(num / 1000000.0, 2).ToString("#.##") + "m";
            }
            else if (num >= 10000000 && num < 999999999)
            {
                formatted += Math.Round(num / 1000000.0, 0).ToString("#.##") + "m";
            }
            else if (num >= 1000000000 && num < 10000000000)
            {
                formatted += Math.Round(num / 1000000000.0, 2).ToString("#.##") + "b";
            }
            else if (num >= 10000000000 && num < 999999999999)
            {
                formatted += Math.Round(num / 1000000000.0, 0).ToString("#.##") + "b";
            }
            else if (num >= 1000000000000 && num < 10000000000000)
            {
                formatted += Math.Round(num / 1000000000000.0, 2).ToString("#.##") + "t";
            }
            else if (num >= 10000000000000 && num < 999999999999999)
            {
                formatted += Math.Round(num / 1000000000000.0, 0).ToString("#.##") + "t";
            }
            else
            {
                formatted += num.ToString();
            }

            if (isNegative)
                formatted = "-" + formatted;

            return formatted;
        }
    }
}
