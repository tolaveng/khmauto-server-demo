using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.Utils
{
    public static class StringExtension
    {
        public static string CleanText(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;

            return Regex.Replace(str, @"[^\w]", "").Trim();
        }

        public static string CleanWhiteSpace(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;

            return Regex.Replace(str, @"[^\w\s\-]", "").Trim();
        }

        public static string GetShaHash(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            using (var sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}
