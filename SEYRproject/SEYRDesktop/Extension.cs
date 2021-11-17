using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SEYRDesktop
{
    public static class Extension
    {
        public static IEnumerable<string> AlphanumericSort(this IEnumerable<string> me)
        {
            return me.OrderBy(x => Regex.Replace(x, @"\d+", m => m.Value.PadLeft(50, '0')));
        }
    }
}
