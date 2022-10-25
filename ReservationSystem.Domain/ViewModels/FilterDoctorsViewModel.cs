using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Domain.ViewModels
{
    public class FilterDoctorsViewModel : Paging<Doctor>
    {
        public string? FullName { get; set; }
    }
}
