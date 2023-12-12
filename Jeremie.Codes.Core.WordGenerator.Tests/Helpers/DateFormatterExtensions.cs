using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeremie.Codes.Core.WordGenerator.Tests.Helpers
{
    internal static class DateFormatterExtensions
    {
        internal static string ToLocaleFormattedDate(this DateTime date, string locale = "fr-FR")
        {
            CultureInfo culture = new CultureInfo(locale);
            var formattedDate = date.ToString("d", culture);
            return formattedDate;
        }
    }
}
