using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Domain.Entities
{
    public class User
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "شماره تماس")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "{0} باید 11 رقمی باشد")]
        [RegularExpression(@"[\u0600-\u06FF]*[ـ]?[\d{5}]+", ErrorMessage = "{0} باید فقط شامل اعداد باشد")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "کد ملی")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0} باید 10 رقمی باشد")]
        [RegularExpression(@"[\u0600-\u06FF]*[ـ]?[\d{5}]+", ErrorMessage = "{0} باید فقط شامل اعداد باشد")]
        public string? NationalCode { get; set; }

        [Display(Name = "ادمین")]
        public bool IsAdmin { get; set; } = false;

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        #endregion

        #region Relations

        public Doctor? Doctor { get; set; }

        #endregion
    }
}
