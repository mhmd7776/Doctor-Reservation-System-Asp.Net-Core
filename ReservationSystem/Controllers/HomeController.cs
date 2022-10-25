using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Application.Extensions;
using ReservationSystem.Application.Services.Interfaces;
using ReservationSystem.Domain.ViewModels;

namespace ReservationSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Ctor

        private readonly IDoctorService _doctorService;

        public HomeController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index(FilterDoctorsViewModel filterDoctorsViewModel)
        {
            var result = await _doctorService.FilterDoctors(filterDoctorsViewModel);

            return View(result);
        }

        #endregion

        #region Doctor Detail

        [HttpGet("DoctorDetail/{doctorId:int}")]
        public async Task<IActionResult> DoctorDetail(int doctorId)
        {
            var result = await _doctorService.FillDoctorDetailViewModel(doctorId);

            if (result == null) return NotFound();

            ViewData["Dates"] = await _doctorService.GetDoctorTimingDatesList(doctorId);

            return View(result);
        }

        #endregion

        #region Final Reservation Modal

        [HttpGet("LoadFinalReservationPartial")]
        public async Task<IActionResult> LoadFinalReservationPartial(int doctorId, string date)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return new JsonResult(new { status = "Failed", message = "ابتدا وارد حساب کاربری خود شوید" });
            }

            #region Get and validation time ranges of selected date

            var timeRanges = await _doctorService.GetTimingsOfDay(doctorId, date);

            if (timeRanges == null || !timeRanges.Any())
            {
                return new JsonResult(new { status = "Failed", message = "هیچ زمان بندی برای روز مورد نظر وجود ندارد" });
            }

            ViewData["TimeRanges"] = timeRanges;

            #endregion

            // fill page model
            var result = await _doctorService.FillFinalReservationViewModel(doctorId, User.GetUserId(), date);

            if (result == null)
            {
                return new JsonResult(new { status = "Failed", message = "اطلاعات وارد شده معتبر نمی باشد" });
            }

            return PartialView("_FinalReservationPartial", result);
        }

        [HttpPost("FinalizeReservation")]
        public async Task<IActionResult> FinalizeReservation(FinalReservationViewModel finalReservationViewModel)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return new JsonResult(new { status = "Failed", message = "ابتدا وارد حساب کاربری خود شوید" });
            }

            var reserveResult = await _doctorService.FinalizeReservation(finalReservationViewModel, User.GetUserId());

            if (reserveResult == null)
            {
                return new JsonResult(new { status = "Failed", message = "اطلاعات وارد شده معتبر نمی باشد" });
            }

            return PartialView("_SuccessReservationPartial", reserveResult);
        }

        #endregion
    }
}
