using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.Utils
{
    public static class StringExtension
    {
        public static string CleanText(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;

            return Regex.Replace(str, "\\s+\\-", "").Trim();
        }
    }
}
