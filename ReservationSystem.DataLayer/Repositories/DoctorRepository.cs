using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.DataLayer.Context;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Interfaces;

namespace ReservationSystem.DataLayer.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        #region Ctor

        private readonly ReservationSystemDbContext _context;

        public DoctorRepository(ReservationSystemDbContext context)
        {
            _context = context;
        }

        #endregion

        public IQueryable<Doctor> GetDoctorsAsQueryable()
        {
            return _context.Doctors
                .Include(s => s.User)
                .AsQueryable();
        }

        public async Task AddDoctor(Doctor doctor)
        {
            await _context.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task AddDoctorTiming(DoctorTiming doctorTiming)
        {
            await _context.AddAsync(doctorTiming);
            await _context.SaveChangesAsync();
        }

        public async Task AddReservation(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<Doctor?> GetDoctorById(int id)
        {
            return await _context.Doctors
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateDoctor(Doctor doctor)
        {
            _context.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public IQueryable<DoctorTiming> GetDoctorTimingsAsQueryable()
        {
            return _context.DoctorTimings
                .Include(s => s.Doctor)
                .AsQueryable();
        }

        public IQueryable<Reservation> GetReservationsAsQueryable()
        {
            return _context.Reservations
                .Include(s => s.Doctor)
                .Include(s => s.User)
                .AsQueryable();
        }

        public async Task<DoctorTiming?> GetDoctorTimingById(int id)
        {
            return await _context.DoctorTimings
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task RemoveDoctorTiming(DoctorTiming doctorTiming)
        {
            _context.Remove(doctorTiming);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDoctor(Doctor doctor)
        {
            _context.Remove(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeDoctorTiming(List<DoctorTiming> doctorTimings)
        {
            _context.RemoveRange(doctorTimings);
            await _context.SaveChangesAsync();
        }
    }
}
