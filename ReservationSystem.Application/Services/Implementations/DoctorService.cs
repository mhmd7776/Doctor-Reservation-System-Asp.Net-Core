using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Application.Extensions;
using ReservationSystem.Application.Generators;
using ReservationSystem.Application.Services.Interfaces;
using ReservationSystem.Application.Statics;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Domain.ViewModels;

namespace ReservationSystem.Application.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        #region Ctor

        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserRepository _userRepository;

        public DoctorService(IDoctorRepository doctorRepository, IUserRepository userRepository)
        {
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
        }

        #endregion

        public async Task<FilterDoctorsViewModel> FilterDoctors(FilterDoctorsViewModel filterDoctorsViewModel)
        {
            // Get doctors queryable
            var doctorsQueryable = _doctorRepository.GetDoctorsAsQueryable();

            // Apply filter
            if (!string.IsNullOrEmpty(filterDoctorsViewModel.FullName))
            {
                doctorsQueryable =
                    doctorsQueryable.Where(s => s.User.FullName.Contains(filterDoctorsViewModel.FullName));
            }

            // Apply paging
            await filterDoctorsViewModel.SetPaging(doctorsQueryable);

            return filterDoctorsViewModel;
        }

        public async Task<CreateDoctorResult> CreateDoctor(CreateDoctorViewModel createDoctorViewModel, IFormFile? doctorAvatar)
        {
            // Upload doctor avatar
            var fileName = string.Empty;
            if (doctorAvatar != null)
            {
                fileName = Guid.NewGuid() + Path.GetExtension(doctorAvatar.FileName);
                var validFormats = new List<string> { ".png", ".jpg", ".jpeg" };
                var uploadResult = doctorAvatar.UploadFile(fileName, PathTools.DoctorAvatarServerPath, validFormats);
                if (!uploadResult) return CreateDoctorResult.ImageNotValid;
            }

            // Get user and check userId validation
            var user = await _userRepository.GetUserById(createDoctorViewModel.UserId);
            if (user == null || user.Doctor != null)
            {
                return CreateDoctorResult.UserNotFound;
            }

            // Create doctor
            var doctor = new Doctor
            {
                Address = createDoctorViewModel.Address,
                Biography = createDoctorViewModel.Biography,
                ImageName = fileName,
                Specialty = createDoctorViewModel.Specialty,
                UserId = createDoctorViewModel.UserId
            };

            await _doctorRepository.AddDoctor(doctor);

            return CreateDoctorResult.Success;
        }

        public async Task<EditDoctorViewModel?> FillEditDoctorViewModel(int doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorById(doctorId);

            if (doctor == null) return null;

            var result = new EditDoctorViewModel
            {
                Address = doctor.Address,
                Biography = doctor.Biography,
                Id = doctor.Id,
                ImageName = doctor.ImageName,
                Specialty = doctor.Specialty,
                DoctorDisplayName = doctor.User.FullName
            };

            return result;
        }

        public async Task<EditDoctorResult> EditDoctor(EditDoctorViewModel editDoctorViewModel, IFormFile? doctorAvatar)
        {
            // Get and validate doctor
            var doctor = await _doctorRepository.GetDoctorById(editDoctorViewModel.Id);

            if (doctor == null) return EditDoctorResult.DoctorNotFound;

            // Upload doctor avatar and delete old avatar
            if (doctorAvatar != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(doctorAvatar.FileName);
                var validFormats = new List<string> { ".png", ".jpg", ".jpeg" };
                var uploadResult = doctorAvatar.UploadFile(fileName, PathTools.DoctorAvatarServerPath, validFormats);
                if (!uploadResult) return EditDoctorResult.NotValidImage;
                doctor.ImageName?.DeleteFile(PathTools.DoctorAvatarServerPath);
                doctor.ImageName = fileName;
            }

            doctor.Address = editDoctorViewModel.Address;
            doctor.Specialty = editDoctorViewModel.Specialty;
            doctor.Biography = editDoctorViewModel.Biography;

            await _doctorRepository.UpdateDoctor(doctor);

            return EditDoctorResult.Success;
        }

        public async Task<List<DoctorTiming>> GetDoctorTimingsList(int doctorId)
        {
            var timingsQueryable = _doctorRepository.GetDoctorTimingsAsQueryable();

            return await timingsQueryable.Where(s => s.DoctorId == doctorId).ToListAsync();
        }

        public async Task<bool> CreateDoctorTiming(CreateDoctorTimingViewModel createDoctorTimingViewModel)
        {
            var doctor = await _doctorRepository.GetDoctorById(createDoctorTimingViewModel.DoctorId);

            if (doctor == null) return false;

            var timing = new DoctorTiming
            {
                DoctorId = createDoctorTimingViewModel.DoctorId,
                Day = createDoctorTimingViewModel.Day,
                StartTime = createDoctorTimingViewModel.StartTime,
                EndTime = createDoctorTimingViewModel.EndTime,
                Duration = createDoctorTimingViewModel.Duration
            };

            await _doctorRepository.AddDoctorTiming(timing);

            return true;
        }

        public async Task<bool> DeleteDoctorTiming(int timingId)
        {
            var timing = await _doctorRepository.GetDoctorTimingById(timingId);

            if (timing == null) return false;

            await _doctorRepository.RemoveDoctorTiming(timing);

            return true;
        }

        public async Task<bool> DeleteDoctor(int doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorById(doctorId);

            if (doctor == null) return false;

            var reservations = await _doctorRepository.GetReservationsAsQueryable()
                .Where(s => s.DoctorId == doctorId).ToListAsync();

            if (reservations.Any()) return false;

            var timings = await _doctorRepository.GetDoctorTimingsAsQueryable()
                .Where(s => s.DoctorId == doctorId).ToListAsync();

            await _doctorRepository.RemoveRangeDoctorTiming(timings);
            await _doctorRepository.RemoveDoctor(doctor);

            return true;
        }

        public async Task<DoctorDetailViewModel?> FillDoctorDetailViewModel(int doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorById(doctorId);

            if (doctor == null) return null;

            var result = new DoctorDetailViewModel
            {
                Address = doctor.Address,
                Biography = doctor.Biography,
                FullName = doctor.User.FullName,
                Id = doctor.Id,
                ImageName = doctor.ImageName,
                Specialty = doctor.Specialty
            };

            return result;
        }

        public async Task<List<DateTime>> GetDoctorTimingDatesList(int doctorId)
        {
            var dates = new List<DateTime>();

            #region Get and validation all timings of doctor

            var timings = await _doctorRepository.GetDoctorTimingsAsQueryable()
                .Where(s => s.DoctorId == doctorId).ToListAsync();

            if (!timings.Any()) return dates;

            #endregion

            #region add valid days of doctor from today to 2 weeks later

            var date = DateTime.Today.AddDays(1);
            while (date <= DateTime.Today.AddDays(14))
            {
                if (timings.Select(s => s.Day).Any(s => (int)s == (int)date.DayOfWeek))
                {
                    dates.Add(date);
                }

                date = date.AddDays(1);
            }

            #endregion

            return dates;
        }

        public bool IsDateTimeReserved(DateTime dateTime, TimeSpan time, int doctorId)
        {
            return _doctorRepository.GetReservationsAsQueryable()
                .Any(s => s.DoctorId == doctorId 
                          && s.ReserveDate.Year == dateTime.Year 
                          && s.ReserveDate.Month == dateTime.Month 
                          && s.ReserveDate.Day == dateTime.Day
                          && s.Time == time);
        }

        public bool IsDateTimeValidForReservation(DateTime dateTime, TimeSpan time, int doctorId)
        {
            // Validate datetime for reserve
            if (dateTime <= DateTime.Now) return false;

            #region Get and validate all timings of doctor

            var timings = _doctorRepository.GetDoctorTimingsAsQueryable()
                .Where(s => s.DoctorId == doctorId).ToList();

            if (!timings.Any()) return false;

            #endregion

            // check selected day exists in doctor timings
            if (timings.Select(s => s.Day).All(s => (int)s != (int)dateTime.DayOfWeek))
            {
                return false;
            }

            // filter doctor timings for current day
            var filteredTimings = timings.Where(s => (int)s.Day == (int)dateTime.DayOfWeek);

            #region add time ranges to list for current day

            var result = new List<string>();
            foreach (var timing in filteredTimings)
            {
                var timingsOfDay = GetAllTimingInRange(timing.StartTime, timing.EndTime, timing.Duration);
                result.AddRange(timingsOfDay);
            }

            #endregion

            // check valid times of day has the selected time
            return result.Any(s => s == time.ToString());
        }

        public bool IsDateTimeValidForReservation(DateTime dateTime, int doctorId)
        {
            // Validate datetime for reserve
            if (dateTime <= DateTime.Now) return false;

            #region Get and validate all timings of doctor

            var timings = _doctorRepository.GetDoctorTimingsAsQueryable()
                .Where(s => s.DoctorId == doctorId).ToList();

            if (!timings.Any()) return false;

            #endregion

            // check selected day exists in doctor timings
            if (timings.Select(s => s.Day).All(s => (int)s != (int)dateTime.DayOfWeek))
            {
                return false;
            }

            return true;
        }

        public List<string> GetAllTimingInRange(TimeSpan start, TimeSpan end, TimeSpan step)
        {
            var result = new List<string>();

            while (start <= end)
            {
                result.Add(start.ToString());
                start = start.Add(step);
            }

            return result;
        }

        public async Task<List<string>?> GetTimingsOfDay(int doctorId, string date)
        {
            #region Validate selected date

            DateTime dateTime;

            try
            {
                dateTime = DateTime.Parse(date);
            }
            catch
            {
                return null;
            }

            #endregion

            // check date is valid for doctor
            if (!IsDateTimeValidForReservation(dateTime, doctorId))
            {
                return null;
            }

            // get all timings of doctor
            var timings = await GetDoctorTiming(doctorId, (int)dateTime.DayOfWeek);

            #region fill time ranges of selected day into list

            var totalTimingsOfDay = new List<string>();
            foreach (var timing in timings)
            {
                var timingsOfDay = GetAllTimingInRange(timing.StartTime, timing.EndTime, timing.Duration);
                totalTimingsOfDay.AddRange(timingsOfDay);
            }

            #endregion

            #region Delete reserved times from list
            
            foreach (var item in totalTimingsOfDay.ToList())
            {
                if (IsDateTimeReserved(dateTime, TimeSpan.Parse(item), doctorId))
                {
                    totalTimingsOfDay.Remove(item);
                }
            }
            
            #endregion

            return totalTimingsOfDay;
        }

        public async Task<List<DoctorTiming>> GetDoctorTiming(int doctorId, int day)
        {
            return await _doctorRepository.GetDoctorTimingsAsQueryable()
                .Where(s => s.DoctorId == doctorId && (int)s.Day == day)
                .ToListAsync();
        }

        public async Task<FinalReservationViewModel?> FillFinalReservationViewModel(int doctorId, int userId, string date)
        {
            var doctor = await _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null) return null;

            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;

            var result = new FinalReservationViewModel
            {
                Date = date,
                DoctorId = doctorId,
                NationalCode = user.NationalCode,
                PhoneNumber = user.PhoneNumber,
                UserDisplayName = user.FullName
            };

            return result;
        }

        public async Task<ReservationResultViewModel?> FinalizeReservation(FinalReservationViewModel finalReservationViewModel, int userId)
        {
            // get and validation doctor
            var doctor = await _doctorRepository.GetDoctorById(finalReservationViewModel.DoctorId);
            if (doctor == null) return null;

            #region Check time and date

            DateTime dateTime;
            try
            {
                dateTime = DateTime.Parse(finalReservationViewModel.Date);
            }
            catch
            {
                return null;
            }

            if (!IsDateTimeValidForReservation(dateTime, finalReservationViewModel.Time, doctor.Id))
            {
                return null;
            }

            if (IsDateTimeReserved(dateTime, finalReservationViewModel.Time, doctor.Id))
            {
                return null;
            }

            #endregion

            var reservation = new Reservation
            {
                DoctorId = doctor.Id,
                NationalCode = finalReservationViewModel.NationalCode!,
                PhoneNumber = finalReservationViewModel.PhoneNumber!,
                ReserveDate = dateTime,
                Time = finalReservationViewModel.Time,
                TrackingCode = CodeGenerator.GenerateTrackingCode(),
                UserId = userId
            };

            await _doctorRepository.AddReservation(reservation);

            return new ReservationResultViewModel
            {
                Time = reservation.Time.ToString(),
                Date = dateTime.ToStringShamsiDate(),
                DoctorDisplayName = doctor.User.FullName,
                TrackingCode = reservation.TrackingCode
            };
        }
    }
}
