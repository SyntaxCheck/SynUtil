using System;

namespace SynUtil.TextHelpers
{
    public static class NameGenerator
    {
        public static string GenerateName(Random rand, int nameLength)
        {
            //string[] c = { "b", "bb", "c", "ch", "ck", "d", "dd", "dg", "dw", "f", "ff", "g", "gh", "h", "j", "k", "l", "lb", "lv", "m", "n", "ng", "nj", "nz", "p", "ph", "q", "r", "rc", "rl", "rn", "rv", "s", "sh", "st", "t", "thz", "v", "w", "x", "xt", "zl" };
            string[] c = { "b", "k", "d", "f", "g", "h", "j", "l", "m", "n", "p", "r", "s", "t", "v", "w", "y", "z", "bl", "cl", "fl", "pl", "gl", "br", "cr", "dr", "fr", "gr", "pr", "tr", "sk", "sl", "sp", "st", "sw", "spr", "str", "ch", "sh", "th", "wh", "ng", "nk" };
            string[] v = { "a", "ae", "e", "i", "o", "oo", "u", "ar", "er", "or", "y", "oi", "ow", "ey", "aw" };
            string name = String.Empty;

            name += c[rand.Next(c.Length)];
            name += v[rand.Next(v.Length)];

            //Capitolize the first letter
            name = (name[0].ToString()).ToUpper() + name.Substring(1);

            int curLen = name.Length;
            bool lastWasDouble = false;
            while (curLen < nameLength)
            {
                string newC = c[rand.Next(c.Length)];
                string newV = v[rand.Next(v.Length)];

                if (newC.Length > 1)
                {
                    if (lastWasDouble)
                    {
                        while (newC.Length > 1)
                            newC = c[rand.Next(c.Length)];

                        lastWasDouble = false;
                    }
                    else
                    {
                        lastWasDouble = true;
                    }
                }
                else
                {
                    lastWasDouble = false;
                }

                if (newV.Length > 1)
                {
                    if (lastWasDouble)
                    {
                        while (newV.Length > 1)
                            newV = v[rand.Next(v.Length)];

                        lastWasDouble = false;
                    }
                    else
                    {
                        lastWasDouble = true;
                    }
                }
                else
                {
                    lastWasDouble = false;
                }

                name += newC;
                curLen += newC.Length;
                if (curLen < nameLength)
                {
                    name += newV;
                    curLen += newV.Length;
                }
            }

            return name;
        }

        public static string GenerateNameOld(Random rand, int nameLength)
        {
            string[] c = { "b", "bb", "c", "ch", "ck", "d", "dd", "dg", "dw", "f", "ff", "g", "gh", "h", "j", "k", "l", "lb", "lf", "lv", "lz", "m", "n", "ng", "nj", "nz", "p", "ph", "q", "r", "rc", "rl", "rn", "rv", "s", "sh", "st", "t", "thz", "v", "w", "x", "xt", "zl" };
            string[] v = { "a", "ae", "e", "i", "o", "u", "y" };
            string name = String.Empty;

            name += c[rand.Next(c.Length)];
            name += v[rand.Next(v.Length)];

            //Capitolize the first letter
            name = (name[0].ToString()).ToUpper() + name.Substring(1);

            int curLen = name.Length;
            while (curLen < nameLength)
            {
                name += c[rand.Next(c.Length)];
                curLen++;
                name += v[rand.Next(v.Length)];
                curLen++;
            }

            return name;
        }
    }
}
