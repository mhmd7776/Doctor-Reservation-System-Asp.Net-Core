using Microsoft.AspNetCore.Http;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.ViewModels;

namespace ReservationSystem.Application.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<FilterDoctorsViewModel> FilterDoctors(FilterDoctorsViewModel filterDoctorsViewModel);

        Task<CreateDoctorResult> CreateDoctor(CreateDoctorViewModel createDoctorViewModel, IFormFile? doctorAvatar);

        Task<EditDoctorViewModel?> FillEditDoctorViewModel(int doctorId);

        Task<EditDoctorResult> EditDoctor(EditDoctorViewModel editDoctorViewModel, IFormFile? doctorAvatar);

        Task<List<DoctorTiming>> GetDoctorTimingsList(int doctorId);

        Task<bool> CreateDoctorTiming(CreateDoctorTimingViewModel createDoctorTimingViewModel);

        Task<bool> DeleteDoctorTiming(int timingId);

        Task<bool> DeleteDoctor(int doctorId);

        Task<DoctorDetailViewModel?> FillDoctorDetailViewModel(int doctorId);

        Task<List<DateTime>> GetDoctorTimingDatesList(int doctorId);

        bool IsDateTimeReserved(DateTime dateTime, TimeSpan time, int doctorId);

        bool IsDateTimeValidForReservation(DateTime dateTime, TimeSpan time, int doctorId);

        bool IsDateTimeValidForReservation(DateTime dateTime, int doctorId);

        List<string> GetAllTimingInRange(TimeSpan start, TimeSpan end, TimeSpan step);

        Task<List<string>?> GetTimingsOfDay(int doctorId, string date);

        Task<List<DoctorTiming>> GetDoctorTiming(int doctorId, int day);

        Task<FinalReservationViewModel?> FillFinalReservationViewModel(int doctorId, int userId, string date);

        Task<ReservationResultViewModel?> FinalizeReservation(FinalReservationViewModel finalReservationViewModel, int userId);
    }
}
