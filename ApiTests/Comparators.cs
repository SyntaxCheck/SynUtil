using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    class Comparators
    {
    }

    public class SortAscending : IComparer<TestClass>
    {
        public int Compare(TestClass mo1, TestClass mo2)
        {
            return mo1.Value1.CompareTo(mo2.Value1);
        }
    }
    public class SortDescending : IComparer<TestClass>
    {
        public int Compare(TestClass mo1, TestClass mo2)
        {
            return mo2.Value1.CompareTo(mo1.Value1);
        }
    }
}
