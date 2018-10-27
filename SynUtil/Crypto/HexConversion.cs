using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.Crypto
{
    public class HexConversion
    {
        public static string TextToHex(string textString)
        {
            return TextToHex(textString, 16);
        }
        public static string TextToHex(string textString, int hexBase)
        {
            string stringHexed = String.Empty;
            char[] chars = textString.ToCharArray();

            if (String.IsNullOrEmpty(textString))
                throw new Exception("Blank Text passed in");

            for (int x = 0; x < chars.Length; x++)
            {
                if ((chars.Length - x) > 1)
                {
                    stringHexed += Convert.ToString((int)chars[x], hexBase) + " ";
                }
                else
                {
                    stringHexed += Convert.ToString((int)chars[x], hexBase);
                }
            }

            if (String.IsNullOrEmpty(stringHexed))
                throw new Exception("Blank hex created");

            return stringHexed;
        }
        public static string HexToText(string hexString)
        {
            return HexToText(hexString, 16);
        }
        public static string HexToText(string hexString, int hexBase)
        {
            string rtn = String.Empty;
            string[] parts = hexString.Trim().Split(' ');
            int[] numbers = new int[parts.Length];

            if (String.IsNullOrEmpty(hexString))
                throw new Exception("Blank Hex passed in");

            for (int x = 0; x < parts.Length; x++)
            {
                int number = Convert.ToInt32(parts[x], hexBase);
                numbers[x] = number;
                rtn += (char)number;
            }

            if (String.IsNullOrEmpty(rtn))
                throw new Exception("Blank text created");

            return rtn;
        }
        public static string BytesToHex(byte[] toConvert)
        {
            if (toConvert == null)
                throw new Exception("Null bytes passed in");

            StringBuilder s = new StringBuilder(toConvert.Length * 2);
            foreach (byte b in toConvert)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }
    }
}
