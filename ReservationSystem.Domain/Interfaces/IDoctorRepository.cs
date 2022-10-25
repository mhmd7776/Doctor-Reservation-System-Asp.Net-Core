using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.ViewModels;

namespace ReservationSystem.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        IQueryable<Doctor> GetDoctorsAsQueryable();

        Task AddDoctor(Doctor doctor);

        Task AddDoctorTiming(DoctorTiming doctorTiming);

        Task AddReservation(Reservation reservation);

        Task<Doctor?> GetDoctorById(int id);

        Task UpdateDoctor(Doctor doctor);

        IQueryable<DoctorTiming> GetDoctorTimingsAsQueryable();

        IQueryable<Reservation> GetReservationsAsQueryable();

        Task<DoctorTiming?> GetDoctorTimingById(int id);

        Task RemoveDoctorTiming(DoctorTiming doctorTiming);

        Task RemoveDoctor(Doctor doctor);

        Task RemoveRangeDoctorTiming(List<DoctorTiming> doctorTimings);
    }
}
