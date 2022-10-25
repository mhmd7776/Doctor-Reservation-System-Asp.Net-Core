using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReservationSystem.Application.Statics;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Application.Extensions
{
    public static class CommonExtensions
    {
        public static string ShowEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?
                .Name!;
        }

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var identifier = claimsPrincipal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);

            if (identifier == null) return 0;

            return int.Parse(identifier.Value);
        }

        public static string GetDoctorAvatarPath(this Doctor doctor)
        {
            if (!string.IsNullOrEmpty(doctor.ImageName))
            {
                return PathTools.DoctorAvatarPath + doctor.ImageName;
            }

            return PathTools.DefaultUserAvatarPath;
        }

        public static bool UploadFile(this IFormFile file, string fileName, string path,
            List<string>? validFormats = null)
        {
            if (validFormats != null && validFormats.Any())
            {
                var fileFormat = Path.GetExtension(file.FileName);

                if (validFormats.All(s => s != fileFormat))
                {
                    return false;
                }
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var finalPath = path + fileName;

            using var stream = new FileStream(finalPath, FileMode.Create);

            file.CopyTo(stream);

            return true;
        }

        public static void DeleteFile(this string fileName, string path)
        {
            var finalPath = path + fileName;

            if (File.Exists(finalPath))
            {
                File.Delete(finalPath);
            }
        }

        public static string ToStringShamsiDate(this DateTime dt)
        {
            PersianCalendar PC = new PersianCalendar();

            var intYear = PC.GetYear(dt);
            var intMonth = PC.GetMonth(dt);
            var intDayOfMonth = PC.GetDayOfMonth(dt);
            DayOfWeek enDayOfWeek = PC.GetDayOfWeek(dt);
            string strMonthName;
            string strDayName;

            switch (intMonth)
            {
                case 1:
                    strMonthName = "فروردین";
                    break;
                case 2:
                    strMonthName = "اردیبهشت";
                    break;
                case 3:
                    strMonthName = "خرداد";
                    break;
                case 4:
                    strMonthName = "تیر";
                    break;
                case 5:
                    strMonthName = "مرداد";
                    break;
                case 6:
                    strMonthName = "شهریور";
                    break;
                case 7:
                    strMonthName = "مهر";
                    break;
                case 8:
                    strMonthName = "آبان";
                    break;
                case 9:
                    strMonthName = "آذر";
                    break;
                case 10:
                    strMonthName = "دی";
                    break;
                case 11:
                    strMonthName = "بهمن";
                    break;
                case 12:
                    strMonthName = "اسفند";
                    break;
                default:
                    strMonthName = "";
                    break;
            }

            switch (enDayOfWeek)
            {
                case DayOfWeek.Friday:
                    strDayName = "جمعه";
                    break;
                case DayOfWeek.Monday:
                    strDayName = "دوشنبه";
                    break;
                case DayOfWeek.Saturday:
                    strDayName = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    strDayName = "یکشنبه";
                    break;
                case DayOfWeek.Thursday:
                    strDayName = "پنجشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    strDayName = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    strDayName = "چهارشنبه";
                    break;
                default:
                    strDayName = "";
                    break;
            }

            return ($"{strDayName} {intDayOfMonth} {strMonthName} {intYear}");
        }
    }
}
