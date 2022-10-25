using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Application.Services.Interfaces;
using ReservationSystem.Domain.ViewModels;
using ReservationSystem.Web.FilterAttributes;

namespace ReservationSystem.Web.Controllers
{
    [Authorize]
    [AuthorizeAdminAccessFilter]
    public class DoctorController : BaseController
    {
        #region Ctor

        private readonly IDoctorService _doctorService;
        private readonly IUserService _userService;

        public DoctorController(IDoctorService doctorService, IUserService userService)
        {
            _doctorService = doctorService;
            _userService = userService;
        }

        #endregion

        #region Filter Doctors

        [HttpGet("Doctors")]
        public async Task<IActionResult> FilterDoctors(FilterDoctorsViewModel filterDoctorsViewModel)
        {
            var result = await _doctorService.FilterDoctors(filterDoctorsViewModel);

            return View(result);
        }

        #endregion

        #region Create Doctor

        [HttpGet("CreateDoctor")]
        public async Task<IActionResult> CreateDoctor()
        {
            ViewData["NotDoctorUsers"] = await _userService.GetNotDoctorUsersSelectList();

            return View();
        }

        [HttpPost("CreateDoctor"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDoctor(CreateDoctorViewModel createDoctorViewModel, IFormFile? doctorAvatar)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر ورودی معتبر نمی باشد";
                ViewData["NotDoctorUsers"] = await _userService.GetNotDoctorUsersSelectList();
                return View(createDoctorViewModel);
            }

            var createDoctorResult = await _doctorService.CreateDoctor(createDoctorViewModel, doctorAvatar);

            switch (createDoctorResult)
            {
                case CreateDoctorResult.Success:
                    TempData[SuccessMessage] = "دکتر مورد نظر با موفقیت افزوده شد";
                    return RedirectToAction("FilterDoctors", "Doctor");
                case CreateDoctorResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر انتخابی معتبر نمی باشد";
                    break;
                case CreateDoctorResult.ImageNotValid:
                    TempData[ErrorMessage] = "تصویر وارد شده معتبر نمی باشد";
                    break;
            }

            ViewData["NotDoctorUsers"] = await _userService.GetNotDoctorUsersSelectList();
            return View(createDoctorViewModel);
        }

        #endregion

        #region Edit Doctor

        [HttpGet("EditDoctor/{id:int}")]
        public async Task<IActionResult> EditDoctor(int id)
        {
            var result = await _doctorService.FillEditDoctorViewModel(id);

            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost("EditDoctor/{id:int}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(EditDoctorViewModel editDoctorViewModel, IFormFile? doctorAvatar)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر ورودی معتبر نمی باشد";
                return View(editDoctorViewModel);
            }

            var updateDoctorResult = await _doctorService.EditDoctor(editDoctorViewModel, doctorAvatar);

            switch (updateDoctorResult)
            {
                case EditDoctorResult.Success:
                    TempData[SuccessMessage] = "دکتر مورد نظر با موفقیت ویرایش شد";
                    return RedirectToAction("FilterDoctors", "Doctor");
                case EditDoctorResult.DoctorNotFound:
                    TempData[ErrorMessage] = "دکتر مورد نظر یافت نشد";
                    break;
                case EditDoctorResult.NotValidImage:
                    TempData[ErrorMessage] = "تصویر وارد شده معتبر نمی باشد";
                    break;
            }

            return View(editDoctorViewModel);
        }

        #endregion

        #region Delete Doctor

        [HttpPost("DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            var result = await _doctorService.DeleteDoctor(doctorId);

            if (!result)
            {
                return new JsonResult(new { status = "Failed", message = "کاربران برای دکتر مورد نظر نوبت ثبت کرده اند" });
            }

            return new JsonResult(new { status = "Success", message = "دکتر با موفقیت حذف شد" });
        }

        #endregion

        #region Doctor Timing Modal

        [HttpGet("LoadCreateDoctorTimingPartial/{doctorId:int}")]
        public IActionResult LoadCreateDoctorTimingPartial(int doctorId)
        {
            var result = new CreateDoctorTimingViewModel
            {
                DoctorId = doctorId
            };

            return PartialView("_CreateDoctorTimingPartial", result);
        }

        [HttpPost("CreateDoctorTiming")]
        public async Task<IActionResult> CreateDoctorTiming(CreateDoctorTimingViewModel createDoctorTimingViewModel)
        {
            if (createDoctorTimingViewModel.Duration == TimeSpan.Zero)
            {
                return new JsonResult(new { status = "Failed", message = "مدت زمان هر نوبت نمی تواند صفر باشد" });
            }

            var result = await _doctorService.CreateDoctorTiming(createDoctorTimingViewModel);

            if (!result)
            {
                return new JsonResult(new { status = "Failed", message = "دکتر مورد نظر یافت نشد" });
            }

            return new JsonResult(new { status = "Success", message = "زمان بندی با موفقیت اضافه شد" });
        }

        #endregion

        #region Delete Doctor Timing

        [HttpPost("DeleteDoctorTiming")]
        public async Task<IActionResult> DeleteDoctorTiming(int timingId)
        {
            var result = await _doctorService.DeleteDoctorTiming(timingId);

            if (!result)
            {
                return new JsonResult(new { status = "Failed", message = "زمان بندی مورد نظر یافت نشد" });
            }

            return new JsonResult(new { status = "Success", message = "زمان بندی با موفقیت حذف شد" });
        }

        #endregion
    }
}
