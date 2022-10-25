using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Application.Generators
{
    public class CodeGenerator
    {
        public static string GenerateTrackingCode()
        {
            var generator = new Random();

            var result = generator.Next(0, 100000).ToString("D5");

            return result;
        }
    }
}
