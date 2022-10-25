using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Application.Services.Interfaces;

namespace ReservationSystem.Web.ViewComponents
{
    public class DoctorTimingsListViewComponent : ViewComponent
    {
        #region Ctor

        private IDoctorService _doctorService;

        public DoctorTimingsListViewComponent(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(int doctorId)
        {
            var timings = await _doctorService.GetDoctorTimingsList(doctorId);

            ViewBag.DoctorId = doctorId;

            return View("DoctorTimingsList", timings);
        }
    }
}
