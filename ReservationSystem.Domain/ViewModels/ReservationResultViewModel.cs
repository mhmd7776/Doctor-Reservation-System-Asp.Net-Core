using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Domain.ViewModels
{
    public class ReservationResultViewModel
    {
        public string DoctorDisplayName { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string TrackingCode { get; set; }
    }
}
