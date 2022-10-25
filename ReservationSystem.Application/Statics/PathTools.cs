using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Application.Statics
{
    public static class PathTools
    {
        public static string DefaultUserAvatarPath = "/images/defaults/avatar.jpeg";

        public static readonly string DoctorAvatarServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/doctors/");
        public static readonly string DoctorAvatarPath = "/images/doctors/";
    }
}
