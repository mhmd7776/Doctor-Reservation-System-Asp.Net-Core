using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Domain.Entities
{
    public class DoctorTiming
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        [Display(Name = "دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DoctorId { get; set; }

        [Display(Name = "روز هفته")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Days Day { get; set; }

        [Display(Name = "ساعت شروع کار دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "ساعت پایان کار دکتر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "مدت زمان هر نوبت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan Duration { get; set; }

        #endregion

        #region Relations

        public Doctor Doctor { get; set; }

        #endregion
    }

    public enum Days
    {
        [Display(Name = "شنبه")] Saturday = 6,
        [Display(Name = "یکشنبه")] Sunday = 0,
        [Display(Name = "دوشنبه")] Monday = 1,
        [Display(Name = "سه شنبه")] Tuesday = 2,
        [Display(Name = "چهارشنبه")] Wednesday = 3,
        [Display(Name = "پنجشنبه")] Thursday = 4,
        [Display(Name = "جمعه")] Friday = 5
    }
}
