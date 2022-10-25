using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Domain.ViewModels
{
    public class FinalReservationViewModel
    {
        [Display(Name = "دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DoctorId { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Date { get; set; }

        [Display(Name = "زمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))?$", ErrorMessage = "زمان وارد شده معتبر نمی باشد")]
        public TimeSpan Time { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "{0} باید 11 رقمی باشد")]
        [RegularExpression(@"[\u0600-\u06FF]*[ـ]?[\d{5}]+", ErrorMessage = "{0} باید فقط شامل اعداد باشد")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0} باید 10 رقمی باشد")]
        [RegularExpression(@"[\u0600-\u06FF]*[ـ]?[\d{5}]+", ErrorMessage = "{0} باید فقط شامل اعداد باشد")]
        public string? NationalCode { get; set; }

        [NotMapped]
        public string UserDisplayName { get; set; }
    }
}
