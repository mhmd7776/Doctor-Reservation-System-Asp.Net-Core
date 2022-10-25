using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.XSS;

namespace ReservationSystem.Application.Security
{
    public static class Sanitizer
    {
        public static string SanitizeText(this string text)
        {
            var sanitize = new HtmlSanitizer();

            return sanitize.Sanitize(text);
        }
    }
}
