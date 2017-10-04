using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MushROMs.Properties;

namespace MushROMs
{
    public static class IOHelper
    {
        public const char FilterPredicate = '*';
        public const char FilterSeparator = '|';
        public const char FilterExtensionSeparator = ';';
        public const string NoExtension = ".*";

        public static string CreateFilter(IList<string> names, IList<string[]> extensions)
        {
            if (names == null)
                throw new ArgumentNullException("names");
            if (extensions == null)
                throw new ArgumentNullException("extensions");

            if (names.Count != extensions.Count)
                throw new ArgumentException(Resources.ErrorFilterMismatch);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == null)
                    throw new ArgumentNullException("names[" + i.ToString(CultureInfo.CurrentCulture) + "]");
                if (extensions[i] == null)
                    throw new ArgumentNullException("extensions[" + i.ToString(CultureInfo.CurrentCulture) + "]");

                if (extensions[i].Length == 0)
                    throw new ArgumentException("extensions[" + i.ToString(CultureInfo.CurrentCulture) + "]");

                sb.Append(names[i]);
                sb.Append(FilterSeparator);

                for (int j = 0; j < extensions[i].Length; j++)
                {
                    if (extensions[i][j] == null)
                        throw new ArgumentNullException("extensions[" + i.ToString(CultureInfo.CurrentCulture) + "][" + j.ToString(CultureInfo.CurrentCulture) + "]");

                    if (j != 0)
                        sb.Append(FilterExtensionSeparator);
                    sb.Append(FilterPredicate);
                    sb.Append(extensions[i][j]);
                }

                if (i != names.Count - 1)
                    sb.Append(FilterSeparator);
            }
            return sb.ToString();
        }
    }
}