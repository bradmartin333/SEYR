using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SEYRDesktop
{
    public static class Extension
    {
        public static IEnumerable<string> AlphanumericSort(this IEnumerable<string> me)
        {
            List<(double, double)> result = new List<(double, double)>();
            foreach (string item in me)
            {
                FileInfo fileInfo = new FileInfo(item);
                string name = fileInfo.Name.Replace(fileInfo.Extension, "");
                string[] cols = name.Split('_');
                bool match = double.TryParse(cols[0], out double x);
                bool match2 = double.TryParse(cols[1], out double y);
                if (match && match2) result.Add((x, y));
            }
            if (result.Count > 0)
            {
                result = result.OrderBy(x => x.Item1).OrderBy(x => x.Item2).ToList();
                List<string> output = new List<string>();
                foreach ((double, double) r in result)
                    output.Add(me.Where(x => x.Contains($"{r.Item1}_{r.Item2}")).First());
                return output;
            }
            else
                return me.OrderBy(x => Regex.Replace(x, @"\d+", m => m.Value.PadLeft(50, '0')));
        }
    }
}
