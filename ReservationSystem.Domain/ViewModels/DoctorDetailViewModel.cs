using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Domain.ViewModels
{
    public class DoctorDetailViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Specialty { get; set; }

        public string? ImageName { get; set; }

        public string Biography { get; set; }

        public string Address { get; set; }
    }
}
