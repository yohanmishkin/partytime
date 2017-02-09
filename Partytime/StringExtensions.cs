using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Partytime
{
    static class StringExtensions
    {
        public static string Dasherize(this string source)
        {
            var parts = SplitAndLower(source);

            return string.Join("-", parts.ToArray());
        }

        private static IEnumerable<string> SplitAndLower(string source)
        {
            var strings = new List<string>();
            var builder = new StringBuilder();

            foreach (var character in source)
            {
                if (IsSeparator(character))
                {
                    if (builder.Length > 0)
                    {
                        strings.Add(builder.ToString());
                    }

                    builder.Clear();
                    continue;
                }
                else if (char.IsUpper(character) && builder.Length > 0)
                {
                    strings.Add(builder.ToString());
                    builder.Clear();
                }

                builder.Append(char.ToLowerInvariant(character));
            }

            if (builder.Length > 0)
            {
                strings.Add(builder.ToString());
            }

            return strings;
        }

        private static bool IsSeparator(char value)
        {
            return value == '-' || value == '_' || value == ' ';
        }
    }
}
