using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Domain.ViewModels
{
    public class EditDoctorViewModel
    {
        public int Id { get; set; }

        public string? DoctorDisplayName { get; set; }

        [Display(Name = "تخصص")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Specialty { get; set; }

        [Display(Name = "تصویر")]
        public string? ImageName { get; set; }

        [Display(Name = "بایوگرافی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Biography { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Address { get; set; }
    }

    public enum EditDoctorResult
    {
        Success,
        DoctorNotFound,
        NotValidImage
    }
}
