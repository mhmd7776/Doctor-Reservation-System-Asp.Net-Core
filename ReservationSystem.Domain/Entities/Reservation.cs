using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Domain.Entities
{
    public class Reservation
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int UserId { get; set; }

        [Display(Name = "دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DoctorId { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "{0} باید 11 رقمی باشد")]
        [RegularExpression(@"[0-9]", ErrorMessage = "{0} باید فقط شامل اعداد باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0} باید 11 رقمی باشد")]
        [RegularExpression(@"[0-9]", ErrorMessage = "{0} باید فقط شامل اعداد باشد")]
        public string NationalCode { get; set; }

        [Display(Name = "کد پیگیری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TrackingCode { get; set; }

        [Display(Name = "ساعت نوبت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan Time { get; set; }

        [Display(Name = "تاریخ رزرو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime ReserveDate { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        #endregion

        #region Relations

        public User User { get; set; }

        public Doctor Doctor { get; set; }

        #endregion
    }
}
