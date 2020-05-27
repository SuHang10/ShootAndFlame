using System;
using System.Collections.Generic;
using System.Text;

namespace WebManager
{
    public static class Turner
    {
        public static string ToString(Dictionary<string,string> dict)
        {
            string s = String.Empty;
            foreach(var each in dict)
            {
                s += $"{each.Key.ToString()},{each.Value.ToString()}&";
            }
            return s;
        }

        public static Dictionary<string, string> ToDict(string s)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            var l = s.Split('&');
            foreach(var each in l)
            {
                if (each == "")
                    continue;
                var _s = each.Split(',');
                dict[_s[0]] = _s[1];
            }
            return dict;
        }
    }
}
