using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.Formatters
{
    public static class FilesystemFormats
    {
        public static string FormatBytes(long bytes)
        {
            string[] strArray = new string[5]
            {
                "B",
                "KB",
                "MB",
                "GB",
                "TB"
            };
            double num = bytes;
            int i;
            for (i = 0; i < strArray.Length && bytes >= 1024; bytes /= 1024)
            {
                num = bytes / 1024.0;
                ++i;
            }
            return string.Format("{0:0.##} {1}", num, strArray[i]);
        }

        public static string FormatBits(long bytes)
        {
            long bits = bytes * 8L;
            string[] strArray = new string[5]
            {
                "b",
                "Kb",
                "Mb",
                "Gb",
                "Tb"
            };
            double working = bits;
            int i;
            for (i = 0; i < strArray.Length && bits >= 1024; bits /= 1024)
            {
                working = bits / 1024.0;
                ++i;
            }
            return string.Format("{0:0.##} {1}", working, strArray[i]);
        }
    }
}
