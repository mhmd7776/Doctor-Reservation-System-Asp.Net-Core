using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Domain.ViewModels
{
    public class CreateDoctorTimingViewModel
    {
        [Display(Name = "دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DoctorId { get; set; }

        [Display(Name = "روز هفته")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Days Day { get; set; }

        [Display(Name = "ساعت شروع کار دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))?$", ErrorMessage = "زمان وارد شده معتبر نمی باشد")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "ساعت پایان کار دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))?$", ErrorMessage = "زمان وارد شده معتبر نمی باشد")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "مدت زمان هر نوبت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))?$", ErrorMessage = "زمان وارد شده معتبر نمی باشد")]
        public TimeSpan Duration { get; set; }
    }
}
